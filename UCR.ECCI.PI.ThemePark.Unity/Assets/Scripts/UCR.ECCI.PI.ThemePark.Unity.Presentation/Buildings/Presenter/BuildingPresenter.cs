using System;
using System.IO;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Buildings
{
    /// <summary>
    /// Handles the visual and interactive representation of a building in the Unity scene.
    /// Responsible for rendering geometry, applying textures and colors, and instantiating triggers.
    /// </summary>
    public class BuildingPresenter : MonoBehaviour
    {
        /// <summary>
        /// UI text element used to display the building's ID above the cube.
        /// </summary>
        public Text BuildingText;

        [Header("Building Visuals")]
        /// <summary>
        /// Prefab used to visually represent the building as a cube.
        /// </summary>
        public GameObject BuildingCube;

        /// <summary>
        /// Instance of the building cube created at runtime.
        /// </summary>
        private GameObject _buildingCubeInstance;

        /// <summary>
        /// Prefab used to instantiate a door trigger collider.
        /// </summary>
        public GameObject DoorTriggerPrefab;

        /// <summary>
        /// Reference to the domain-level building entity.
        /// </summary>
        private Building _building;

        /// <summary>
        /// Unity lifecycle method called on script initialization.
        /// Currently unused.
        /// </summary>
        void Start()
        {
        }

        /// <summary>
        /// Initializes the presenter with building data and renders its visual representation.
        /// </summary>
        /// <param name="building">The domain entity representing the building.</param>
        public void SetData(Building building)
        {
            _building = building;
            RenderBuildingCube();
            ApplyColorAndTexture();
            InstantiateDoorTrigger();
        }

        /// <summary>
        /// Instantiates and positions the building cube based on domain coordinates and dimensions.
        /// Also updates the UI text label.
        /// </summary>
        private void RenderBuildingCube()
        {
            if (BuildingCube == null)
            {
                Debug.LogError("BuildingCube not assigned in the inspector.");
                return;
            }

            // Destroy previous instance if it exists
            if (_buildingCubeInstance != null)
                Destroy(_buildingCubeInstance);

            // Create new cube instance
            _buildingCubeInstance = Instantiate(BuildingCube, transform);

            // Set position based on domain coordinates
            _buildingCubeInstance.transform.position = new Vector3(
                (float)_building.RenderInfo.XCoodinate.XValue,
                (float)(_building.RenderInfo.Heigth.Value / 2),
                (float)_building.RenderInfo.ZCoodinate.ZValue
            );

            // Set scale based on building dimensions
            _buildingCubeInstance.transform.localScale = new Vector3(
                (float)_building.RenderInfo.Width.Value,
                (float)_building.RenderInfo.Heigth.Value,
                (float)_building.RenderInfo.Depth.Value
            );

            // Parent to presenter for hierarchy clarity
            _buildingCubeInstance.transform.SetParent(transform, true);

            // Update building label text
            if (BuildingText != null)
            {
                Debug.Log("Setting building text for building ID: " + _building.Id);
                BuildingText.text = _building.Id.ToString();
                BuildingText.transform.position = _buildingCubeInstance.transform.position
                    + new Vector3(0, (float)_building.RenderInfo.Heigth.Value / 2 + 1, 0);
            }
        }

        /// <summary>
        /// Instantiates a door trigger collider at a fixed position relative to the cube.
        /// </summary>
        private void InstantiateDoorTrigger()
        {
            if (DoorTriggerPrefab == null || _buildingCubeInstance == null)
                return;

            var door = Instantiate(DoorTriggerPrefab, _buildingCubeInstance.transform);

            // Set local transform properties
            door.transform.localPosition = new Vector3(-0.5f, -0.44f, 0f);
            door.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            door.transform.localScale = new Vector3(0.1f, 0.15f, 0.03f);

            // Configure collider size
            var collider = door.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.size = new Vector3(2f, 0f, 5f);
            }
            else
            {
                Debug.LogWarning("DoorTriggerPrefab does not have a BoxCollider.");
            }
        }

        /// <summary>
        /// Applies color and texture to the building cube's material.
        /// </summary>
        private void ApplyColorAndTexture()
        {
            var renderer = _buildingCubeInstance.GetComponent<Renderer>();
            if (renderer == null)
                return;

            var mat = new Material(renderer.sharedMaterial);

            ApplyColor(mat);
            ApplyTexture(mat);

            renderer.material = mat;
        }

        /// <summary>
        /// Applies the building's color to the material if available.
        /// </summary>
        /// <param name="mat">Material to modify.</param>
        private void ApplyColor(Material mat)
        {
            if (_building == null || _building.RenderInfo?.Color == null)
                return;

            if (ColorUtility.TryParseHtmlString(_building.RenderInfo.Color.Value, out Color parsedColor))
            {
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", parsedColor);
                else if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", parsedColor);
            }
        }

        /// <summary>
        /// Loads and applies a texture from disk based on the building's texture file name.
        /// Falls back to a default texture if not found.
        /// </summary>
        /// <param name="mat">Material to apply the texture to.</param>
        private void ApplyTexture(Material mat)
        {
            string textureFile = _building?.RenderInfo?.Texture?.Value;
            string texturePath = null;

            if (!string.IsNullOrEmpty(textureFile) && textureFile.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                string baseName = string.Join("_", Path.GetFileNameWithoutExtension(textureFile).Split('_').Take(3));
                texturePath = GetTexturePath(textureFile);
            }

            if (string.IsNullOrEmpty(texturePath) || !File.Exists(texturePath))
            {
                Debug.LogWarning($"Using default texture for building ID: {_building?.Id}");
                texturePath = Path.Combine(UnityEngine.Application.dataPath, "Phoenix3D", "Textures", "Default", "default.png");
            }

            texturePath = Path.GetFullPath(texturePath);

            if (File.Exists(texturePath))
            {
                try
                {
                    byte[] fileData = File.ReadAllBytes(texturePath);
                    Texture2D tex = new Texture2D(2, 2);
                    if (tex.LoadImage(fileData))
                    {
                        mat.mainTexture = tex;
                        Debug.Log($"Texture applied successfully from {texturePath}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to load texture from {texturePath}");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error loading texture: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"Texture file not found at path: {texturePath}");
            }
        }

        /// <summary>
        /// Constructs the full path to the texture file based on naming conventions.
        /// </summary>
        /// <param name="textureFile">Filename of the texture.</param>
        /// <returns>Absolute path to the texture file.</returns>
        private string GetTexturePath(string textureFile)
        {
            string baseName = string.Join("_", Path.GetFileNameWithoutExtension(textureFile).Split('_').Take(3));
            string texturePath = Path.Combine(UnityEngine.Application.dataPath, "Phoenix3D", "Textures", baseName, textureFile);
            return Path.GetFullPath(texturePath);
        }
    }
}
