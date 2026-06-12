using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UCR.ECCI.PI.ThemePark.Unity.Services;
using UCR.ECCI.PI.ThemePark.Unity.Presentation.Core;
using Zenject;

/// <summary>
/// Component responsible for handling player interactions with a building.
/// - Detects when the player enters or exits the building's trigger collider.
/// - Displays a popup UI when the player is nearby.
/// - Allows the player to enter the building when the interact action is performed.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class BuildingInteraction : MonoBehaviour
{
    [InjectOptional(Id = "SceneID")]
    public string SceneIdDebug;

    [Inject]
    private ISceneTransitionService _sceneTransitionService;

    [Header("UI Popup")]
    [Tooltip("Canvas that appears when the player is close to the building.")]
    /// <summary>
    /// Reference to the popup canvas shown when the player is within interaction range.
    /// This canvas should be set in the prefab via the Inspector.
    /// </summary>
    public Canvas popupCanvas;

    [Header("Interaction Options")]
    [Tooltip("Scene index to load when entering the building.")]
    /// <summary>
    /// Scene index to load when the player enters the building.
    /// If left as default, scene index 1 will be loaded.
    /// </summary>
    public int BuildingSelectionScene = 1;

    /// <summary>
    /// Text element used to display the option to enter.
    /// </summary>
    public Text BuildingText;

    /// <summary>
    /// Optional UI text element used to display the building ID.
    /// </summary>
    public Text BuildingId;

    public Text PromptText;

    /// <summary>
    /// Tracks whether the player is currently inside the building's trigger collider.
    /// </summary>
    private bool _isPlayerNearby = false;

    private bool _isReady = false;

    /// <summary>
    /// Domain entity representing the building associated with this interaction.
    /// </summary>
    public Building _building;

    private BoxCollider _collider;

    /// <summary>
    /// Unity lifecycle method called on the first frame.
    /// Ensures the collider is configured as a trigger and hides the popup canvas initially.
    /// </summary>
    private void Awake()
    {
        // Ensure the collider is set as a trigger
        var collider = GetComponent<BoxCollider>();
        // collider.isTrigger = true;

        // Hide popup at the start
        if (popupCanvas != null)
            popupCanvas.enabled = false;
    }

    private void Start()
    {
        Debug.Log($"[BuildingInteraction] Start: _sceneTransitionService is " +
                  (_sceneTransitionService == null ? "NULL" : "NOT null") +
                  $" on {gameObject.name}");

        Debug.Log("[BuildingInteraction] Debug SceneID = " + SceneIdDebug);
    }

    /// <summary>
    /// Assigns the domain building entity to this interaction component.
    /// Called by the BuildingManager during prefab instantiation.
    /// </summary>
    /// <param name="building">The domain building entity to associate with this interaction.</param>
    public void SetBuilding(Building building)
    {
        _building = building;
        _isReady = (_building != null);

        if (BuildingId != null && _building != null)
            BuildingId.text = _building.Id.ToString();

        if (_collider != null && _building != null)
        {
            // Adjust trigger
            float w = (float)_building.RenderInfo.Width.Value;
            float h = (float)_building.RenderInfo.Heigth.Value;
            float d = (float)_building.RenderInfo.Depth.Value;

            _collider.center = new Vector3(0f, h * 0.5f, 0f);
            _collider.size = new Vector3(w, h, d);

            _collider.enabled = true;
        }

        Debug.Log($"[BuildingInteraction] SetBuilding OK -> id={_building?.Id}");
    }


    /// <summary>
    /// Unity event called when another collider enters this trigger.
    /// If the collider belongs to the player:
    /// - Marks the player as nearby.
    /// - Enables the popup canvas.
    /// - Registers this building as the current interactable in the PlayerInteractionManager.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_isReady || !other.CompareTag("Player")) return;

        _isPlayerNearby = true;

         Debug.Log($"Popup ref? {popupCanvas != null}");
        if (popupCanvas != null)
        {
            popupCanvas.enabled = true;
            Debug.Log($"Popup enabled: {popupCanvas.enabled}, renderMode={popupCanvas.renderMode}, layer={popupCanvas.gameObject.layer}");
        }

        // Register this building as the current interactable
        PlayerInteractionManager.Instance.SetCurrentBuilding(this);

        Debug.Log($"Player entered {_building.Name.Value}, _building = {_building.Id}");
    }

    /// <summary>
    /// Unity event called when another collider exits this trigger.
    /// If the collider belongs to the player:
    /// - Marks the player as no longer nearby.
    /// - Disables the popup canvas.
    /// - Clears this building reference from the PlayerInteractionManager.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (!_isReady || !other.CompareTag("Player")) return;

        _isPlayerNearby = false;
        if (popupCanvas != null) 
                popupCanvas.enabled = false;

            // Clear reference to this building
        PlayerInteractionManager.Instance.ClearCurrentBuilding(this);

        Debug.Log($"Player exited {_building.Name.Value}, _building = {_building.Id}");
    }

    /// <summary>
    /// Attempts to enter the building when the player presses the interact action.
    /// This method is called externally by the PlayerInteractionManager.
    /// If the player is nearby:
    /// - Logs the building entry.
    /// - Stores the building ID in the BuildingSession singleton.
    /// - Loads the configured interior scene.
    /// </summary>
    public void TryEnter()
    {
        if (!_isPlayerNearby) return;

        if (!_isReady || _building == null)
        {
            Debug.LogWarning("[BuildingInteraction] Not ready: _building is null (espera a SetBuilding).");
            return;
        }

        if (BuildingSession.Instance == null)
        {
            Debug.LogError("[BuildingInteraction] BuildingSession.Instance es null.");
            return;
        }

        if (_sceneTransitionService == null)
        {
            var sceneContext = FindObjectOfType<SceneContext>();
            if (sceneContext != null)
            {
                Debug.Log("[BuildingInteraction] Resolving ISceneTransitionService manually from SceneContext.");
                _sceneTransitionService = sceneContext.Container.Resolve<ISceneTransitionService>();
            }
            else
            {
                Debug.LogError("[BuildingInteraction] No SceneContext found to manually resolve ISceneTransitionService.");
            }
        }

        Debug.Log($"[BuildingInteraction] Entering building id={_building.Id}");
        BuildingSession.Instance.BuildingId = _building.Id;
        //SceneManager.LoadScene(1);
        if (_sceneTransitionService is null)
        {
            Debug.Log($"[BuildingInteraction] _sceneTransitionService is null");
        } else {
            _sceneTransitionService.TransitionTo(BuildingSelectionScene);
        }
    }
}
