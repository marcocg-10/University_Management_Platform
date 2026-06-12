using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.ValueObjects;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.LearningSpaces
{
    public class LearningSpacePresenter : MonoBehaviour
    {
        private LearningSpaceDimensions _dimensions;
        private LearningSpaceCoordinates _coordinates;
        private LearningSpaceColor _color;
        private LearningSpaceTexture _texture;

        public int LearningSpaceId { get; private set; }

        public GameObject LearningSpaceFloor;
        public GameObject LearningSpaceCeiling;

        public GameObject LearningSpaceFrontWall;
        public GameObject LearningSpaceBackWall;
        public GameObject LearningSpaceLeftWall;
        public GameObject LearningSpaceRightWall;

        public BoxCollider LearningSpaceBounds;

        [SerializeField] private float _confinerMarginXY = 1f;

        [SerializeField] private float _confinerTopMargin = 0.5f;

        // Start is called before the first frame update

        [Tooltip("Optional override. If null, spawns at room center on the floor.")]
        public Transform playerSpawn;
        
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(
            LearningSpaceColor color,
            LearningSpaceTexture texture,
            LearningSpaceDimensions dimensions,
            LearningSpaceCoordinates coordinates,
            int learningSpaceId)
        {
            _color = color;
            _texture = texture;
            LearningSpaceId = learningSpaceId;    
            _dimensions = dimensions;
            _coordinates = coordinates;

            RefreshUI();
        }

        private void RefreshUI()
        {
            // --- DIMENSIONS (unchanged from your code) ---
            float width = _dimensions.Width;    // Unity X
            float length = _dimensions.Length;   // Unity Z
            float height = _dimensions.Height;   // Unity Y

            const float wallT = 0.10f;
            const float floorT = 0.05f;

            width = Mathf.Max(0.001f, width);
            length = Mathf.Max(0.001f, length);
            height = Mathf.Max(0.001f, height);

            LearningSpaceCeiling.transform.localRotation = Quaternion.identity;
            LearningSpaceFloor.transform.localRotation = Quaternion.identity;
            LearningSpaceFrontWall.transform.localRotation = Quaternion.identity;
            LearningSpaceBackWall.transform.localRotation = Quaternion.identity;
            LearningSpaceLeftWall.transform.localRotation = Quaternion.identity;
            LearningSpaceRightWall.transform.localRotation = Quaternion.identity;

            // Ceiling
            LearningSpaceCeiling.transform.localScale = new Vector3(
                width + 0.2f,
                floorT,
                length + 0.2f
            );
            LearningSpaceCeiling.transform.localPosition = new Vector3(0f, height + floorT * 0.5f, 0f);

            // Floor
            LearningSpaceFloor.transform.localScale = new Vector3(width, floorT, length);
            LearningSpaceFloor.transform.localPosition = new Vector3(0f, floorT * 0.5f, 0f);

            // Walls
            float wallHeight = height + 2f;   // add 2 units of safety margin
            Vector3 fbScale = new Vector3(width, wallHeight, wallT);
            Vector3 lrScale = new Vector3(wallT, wallHeight, length);


            LearningSpaceFrontWall.transform.localScale = fbScale;
            LearningSpaceBackWall.transform.localScale = fbScale;
            LearningSpaceLeftWall.transform.localScale = lrScale;
            LearningSpaceRightWall.transform.localScale = lrScale;

            float halfW = width * 0.5f;
            float halfL = length * 0.5f;
            float yPos = wallHeight * 0.5f + floorT;

            LearningSpaceFrontWall.transform.localPosition = new Vector3(0f, yPos, halfL + wallT * 0.5f);
            LearningSpaceBackWall.transform.localPosition = new Vector3(0f, yPos, -halfL - wallT * 0.5f);
            LearningSpaceLeftWall.transform.localPosition = new Vector3(-halfW - wallT * 0.5f, yPos, 0f);
            LearningSpaceRightWall.transform.localPosition = new Vector3(halfW + wallT * 0.5f, yPos, 0f);

            ApplyColor();
            ApplyWallTextures();

            // --- COORDINATES (apply to the container) ---
            if (_coordinates != null)
            {
                // Map domain coords -> Unity: X stays X, Length->Y, Height->Z
                float posX = _coordinates.XCoordinate;
                float posY = _coordinates.YCoordinate; // vertical in Unity
                float posZ = _coordinates.ZCoordinate; // depth in Unity

                transform.localPosition = new Vector3(posX, posY, posZ);

                transform.localRotation = Quaternion.identity;

                // OPTIONAL: if your coordinates refer to the *min corner* rather than the room center,
                // offset by half the extents so the room lands correctly.
                /* if (_coordinates.Anchor == AnchorKind.MinCorner)
                {
                    transform.localPosition += new Vector3(halfW, 0f, halfL);
                } */
            }

            SetupLearningSpaceBoundsCollider();
        }

        private void ApplyColor()
        {


            if (_color == null || string.IsNullOrEmpty(_color.Value))
                return;

            if (ColorUtility.TryParseHtmlString(_color.Value, out Color unityColor))
            {
                var renderer = LearningSpaceFloor.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.material.color = unityColor;

                renderer = LearningSpaceCeiling.GetComponent<Renderer>();
                if (renderer != null)
                    renderer.material.color = unityColor;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Invalid color hex: {_color.Value}");
            }

        }

        private void ApplyWallTextures()
        {
            if (_texture == null || string.IsNullOrEmpty(_texture.Value))
                return;

            try
            {
                string fileName = _texture.Value;  // directly use the texture value as file name

                string fileNameRaw = Path.GetFileNameWithoutExtension(fileName);
                string[] parts = fileNameRaw.Split('_');
                string baseName = string.Join("_", parts.Take(3));

                string folderName = baseName;

                // Construimos la ruta real
                string texturePath = Path.Combine(
                    UnityEngine.Application.dataPath,   // ...\UCR.ECCI.PI.ThemePark.Unity\Assets
                    "Phoenix3D", "Textures",
                    folderName, fileName);               // Use the original file name

                texturePath = Path.GetFullPath(texturePath);

                if (!File.Exists(texturePath))
                {
                    Debug.LogWarning($"Texture file not found at path: {texturePath}");
                    return;
                }

                byte[] fileData = File.ReadAllBytes(texturePath);
                Texture2D tex = new Texture2D(2, 2);
                if (tex.LoadImage(fileData))
                {
                    ApplyTextureToWalls(tex);
                    Debug.Log($"Texture applied successfully from {texturePath}");
                }
                else
                {
                    Debug.LogWarning($"Failed to load texture from {texturePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error applying wall texture: {ex.Message}");
            }
        }


        private void ApplyTextureToWalls(Texture2D tex)
        {
            // List of walls where the texture will be applied
            var walls = new[]
            {
                LearningSpaceFrontWall,
                LearningSpaceBackWall,
                LearningSpaceLeftWall,
                LearningSpaceRightWall
            };

            // Determine if a valid color is defined in the ValueObject
            Color? tintColor = null;
            if (_color != null && ColorUtility.TryParseHtmlString(_color.Value, out Color parsedColor))
                tintColor = parsedColor;

            foreach (var wall in walls)
            {
                if (wall == null)
                    continue;

                var renderer = wall.GetComponent<Renderer>();
                if (renderer == null)
                    continue;

                // Create a new material instance so changes don�t affect other objects
                var mat = new Material(renderer.sharedMaterial);

                // Assign the selected texture
                mat.mainTexture = tex;

                // Apply color depending on shader type (URP/HDRP or Built-in)
                if (tintColor.HasValue)
                {
                    if (mat.HasProperty("_BaseColor"))
                    {
                        // For URP/HDRP shaders
                        mat.SetColor("_BaseColor", tintColor.Value);
                    }
                    else if (mat.HasProperty("_Color"))
                    {
                        // For Built-in/Standard shaders
                        mat.SetColor("_Color", tintColor.Value);
                    }
                }

                // Apply the new material to the wall
                renderer.material = mat;
            }

            Debug.Log("Wall textures and color applied successfully.");
        }
        private void SetupLearningSpaceBoundsCollider()
        {
            if (LearningSpaceBounds == null)
            {
                LearningSpaceBounds = GetComponent<BoxCollider>();
                if (LearningSpaceBounds == null)
                    LearningSpaceBounds = gameObject.AddComponent<BoxCollider>();
            }

            LearningSpaceBounds.isTrigger = true;   // used only as a volume for Cinemachine

            // These are the interior dimensions coming from the backend
            float width  = _dimensions.Width;
            float length = _dimensions.Length;
            float height = _dimensions.Height;

            const float floorT = 0.05f;

            // --- Apply margins so the camera stays slightly inside the room ---

            // Shrink on X/Z so the camera stays a bit away from the walls
            float innerWidth  = Mathf.Max(0.1f, width  - 2f * _confinerMarginXY);
            float innerLength = Mathf.Max(0.1f, length - 2f * _confinerMarginXY);

            // Shrink at the top so the camera can't go right up to the ceiling
            float innerHeight = Mathf.Max(0.1f, height - _confinerTopMargin);

            // Interior vertical space runs from floor top (y = floorT)
            // up to floorT + innerHeight.
            float centerY = floorT + innerHeight * 0.5f;

            LearningSpaceBounds.center = new Vector3(0f, centerY, 0f);
            LearningSpaceBounds.size   = new Vector3(innerWidth, innerHeight, innerLength);

            Debug.Log($"[LearningSpace] Confiner bounds configured. Size={LearningSpaceBounds.size}, Center={LearningSpaceBounds.center}");
        }
    }
}