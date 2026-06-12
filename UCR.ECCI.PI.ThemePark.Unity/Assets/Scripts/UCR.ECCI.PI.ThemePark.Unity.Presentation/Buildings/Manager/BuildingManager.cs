using System.Collections.Generic;
using UnityEngine;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Buildings
{
    /// <summary>
    /// Responsible for managing building instances in the Unity presentation layer.
    /// - Fetches building data from the application service.
    /// - Instantiates building prefabs in the scene.
    /// - Initializes both presenter and interaction components for each building.
    /// </summary>
    public class BuildingManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab that contains both the BuildingPresenter and BuildingInteraction scripts.
        /// This prefab is instantiated for each building retrieved from the service.
        /// </summary>
        public GameObject buildingPrefab;

        /// <summary>
        /// Service used to fetch building data from the application layer.
        /// Injected via Zenject dependency injection.
        /// </summary>
        [Inject]
        private IBuildingService _buildingService;

        [Inject] private IAuthReady _authReady;

        //[Inject] private IOAuth2Service _oauth;

        [Inject] private LoadingScreenView _loadingView;

        [Inject] private DiContainer _container;

        /// <summary>
        /// List of domain building entities retrieved from the service.
        /// </summary>
        private List<Building> _buildings = new();

        /// <summary>
        /// List of instantiated building GameObjects currently active in the scene.
        /// Used to clear and refresh buildings when new data is loaded.
        /// </summary>
        private List<GameObject> _buildingInstances = new();

        /// <summary>
        /// Unity lifecycle method called on the first frame.
        /// Starts the process of fetching building data and instantiating prefabs.
        /// </summary>
        private async void Start()
        {
            _loadingView.ShowImmediate();
            // If we’ve never completed an interactive login on this device, force it once.

            /* var token = await _oauth.GetValidAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                var ok = await _oauth.SignInAsync();
                if (!ok)
                {
                    Debug.LogError("Sign-in failed; aborting data load.");
                    return;
                }

                token = await _oauth.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    Debug.LogError("[Login] No valid access token after sign-in. " +
                                "Check redirect_uri, scopes, and token exchange.");
                    return;
                }
            } */

            await _authReady.Ready;

            // Proceed — you are signed in and have a valid token.
            await GetData();

            StartCoroutine(_loadingView.FadeOut());
        }

        /// <summary>
        /// Unity lifecycle method called once per frame.
        /// Currently unused but available for future per-frame updates.
        /// </summary>
        private void Update()
        {
            // Reserved for per-frame update logic if needed
        }

        /// <summary>
        /// Asynchronously fetches building data from the service and instantiates prefabs.
        /// - Clears any previously instantiated buildings.
        /// - Creates new prefab instances for each building.
        /// - Initializes presenter and interaction components with building data.
        /// </summary>
        private async Task GetData()
        {
            if (_buildingService == null)
            {
                Debug.LogError("_buildingService was not injected.");
                return;
            }

            // Fetch buildings from the service
            _buildings = (await _buildingService.GetBuildingsAsync()).ToList();

            // Clear previously instantiated buildings
            foreach (var go in _buildingInstances)
                Destroy(go);
            _buildingInstances.Clear();

            // Instantiate new buildings
            foreach (var building in _buildings)
            {
                // Create a new prefab instance under this manager
                //var instance = Instantiate(buildingPrefab, transform);
                var instance = _container.InstantiatePrefab(buildingPrefab, transform);

                _container.InjectGameObject(instance);

                // Position the instance using domain coordinates
                instance.transform.position = new Vector3(
                    (float)building.RenderInfo.XCoodinate.XValue,
                    (float)building.RenderInfo.YCoodinate.YValue,
                    (float)building.RenderInfo.ZCoodinate.ZValue
                );

                // Initialize the presenter with building data
                var presenter = instance.GetComponent<BuildingPresenter>();
                presenter?.SetData(building);

                // Initialize the interaction component (located in the Cube child)
                var interaction = instance.GetComponentInChildren<BuildingInteraction>();
                if (interaction != null)
                {
                    interaction.SetBuilding(building);
                }
                else
                {
                    Debug.LogError($"{instance.name} does not contain a BuildingInteraction component in its children.");
                }

                // Track the instance for later cleanup
                _buildingInstances.Add(instance);
            }
        }
    }
}
