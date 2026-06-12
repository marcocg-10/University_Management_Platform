using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Central manager that coordinates player interactions with buildings.
/// Keeps track of the current building the player is near and delegates
/// interaction requests to it.
/// Also manages board interactions via BoardInteractionManager.
/// </summary>
public class PlayerInteractionManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the <see cref="PlayerInteractionManager"/>.
    /// Ensures there is only one manager active in the scene.
    /// </summary>
    public static PlayerInteractionManager Instance { get; private set; }

    /// <summary>
    /// Reference to the building the player is currently near and can interact with.
    /// </summary>
    private BuildingInteraction currentBuilding;

    /// <summary>
    /// The maximum distance, in units, for performing a raycast to detect board hover interactions.
    /// </summary>
    [Header("Board Raycast Settings")]
    [Tooltip("Max distance for board hover raycast (cursor-based).")]
    public float boardRaycastDistance = 8f;

    /// <summary>
    /// Gets or sets the <see cref="LayerMask"/> used to filter raycast hits on the board.
    /// </summary>
    [Tooltip("LayerMask used to filter board raycast hits (optional). Leave 0 for all layers.")]
    public LayerMask boardLayerMask;

    /// <summary>
    /// Represents the screen-space centered text used to display interaction prompts to the user.
    /// </summary>
    /// <remarks>This text is typically used to provide context-sensitive instructions or feedback to the
    /// user, such as "Press E to interact." Ensure the text is updated dynamically based on the current interaction
    /// state.</remarks>
    [Header("Screen Prompt")]
    [Tooltip("Screen-space centered Text that shows interaction prompts.")]
    public Text screenPromptText;

    /// <summary>
    /// The prompt message displayed when hovering over a board.
    /// </summary>
    [Tooltip("Prompt shown when hovering a board.")]
    public string hoverPrompt = "Press E to interact";

    /// <summary>
    /// The prompt message displayed to the user when focused on a board.
    /// </summary>
    [Tooltip("Prompt shown while focused on a board.")]
    public string exitPrompt = "Press E to exit";

    /// <summary>
    /// Represents the transform component of the camera.
    /// </summary>
    /// <remarks>This field is used to store a reference to the camera's transform, which can be used for
    /// operations  such as positioning, rotation, or scaling relative to the camera's perspective.</remarks>
    private Transform _cameraTransform;

    // Board interaction helper (initialized en InitializeBoardInteractions)
    private BoardInteractionManager _boardManager;
    private bool _initializedBoardManager = false;

    // Track picker visibility to detect transitions (open -> closed)
    private bool _wasPickerVisible = false;

    /// <summary>
    /// Unity lifecycle method called when the script instance is being loaded.
    /// Initializes the singleton instance and ensures only one manager exists.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    /// <summary>
    /// Starts the necessary processes to initialize board interactions and hide the screen prompt.
    /// </summary>
    /// <remarks>This method should be called to prepare the system for user interaction. It ensures that the
    /// board is ready for use and any initial prompts are hidden from view.</remarks>
    private void Start()
    {
        InitializeBoardInteractions();
        HideScreenPrompt();
    }

    /// <summary>
    /// Updates the current game state by refreshing the board and updating the screen prompt.
    /// </summary>
    /// <remarks>This method ensures that the game board is updated and the user interface reflects the latest
    /// state.</remarks>
    private void Update()
    {
        // Ensure board manager initialized
        if (!_initializedBoardManager)
        {
            InitializeBoardInteractions();
        }

        // Detect color picker visibility changes: if picker just closed, reinitialize board interactions
        bool pickerVisible = false;
        if (BoardColorPickerManager.Instance != null)
        {
            pickerVisible = BoardColorPickerManager.Instance.IsVisible;
        }

        if (_wasPickerVisible && !pickerVisible)
        {
            Debug.Log("[PlayerInteractionManager] Color picker closed -> reinitializing board interactions to restore focus state.");
            InitializeBoardInteractions();
        }
        _wasPickerVisible = pickerVisible;

        // Update board manager safely
        try
        {
            _boardManager?.Update(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerInteractionManager] Exception during _boardManager.Update: {e}");
        }

        UpdateScreenPrompt();
    }

    /// <summary>
    /// Registers a building as the current interactable when the player enters its trigger.
    /// </summary>
    /// <param name="building">The <see cref="BuildingInteraction"/> to register.</param>
    public void SetCurrentBuilding(BuildingInteraction building) => currentBuilding = building;

    /// <summary>
    /// Clears the current building reference when the player exits its trigger.
    /// </summary>
    /// <param name="building">The <see cref="BuildingInteraction"/> to clear.</param>
    public void ClearCurrentBuilding(BuildingInteraction building)
    {
        if (currentBuilding == building)
            currentBuilding = null;
    }

    /// <summary>
    /// Handles the interaction logic for the player, such as interacting with the game board or entering a building.
    /// </summary>
    /// <remarks>This method first attempts to interact with the game board. If the interaction is handled
    /// successfully, no further actions are taken. Otherwise, it attempts to enter the currently selected building, if
    /// one is available.</remarks>
    public void Interact()
    {
        try
        {
            if (_boardManager != null && _boardManager.HandleInteract(false))
                return;
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerInteractionManager] Exception while handling board interaction: {e}");
        }

        currentBuilding?.TryEnter();
    }

    /// <summary>
    /// Initializes the board interactions by setting up the camera transform and configuring the board manager.
    /// </summary>
    /// <remarks>This method retrieves the main camera's transform, if available, and uses it to initialize
    /// the board manager  with the specified raycast distance, layer mask, and interaction settings.</remarks>
    private void InitializeBoardInteractions()
    {
        _cameraTransform = Camera.main != null ? Camera.main.transform : null;

        if (_boardManager == null)
        {
            // BoardInteractionManager appears to be a plain helper class in this project;
            // create instance lazily to avoid initialization order issues.
            _boardManager = new BoardInteractionManager();
        }

        try
        {
            _boardManager.Initialize(_cameraTransform, boardRaycastDistance, boardLayerMask, false);
            _initializedBoardManager = true;
            Debug.Log("[PlayerInteractionManager] BoardInteractionManager initialized.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerInteractionManager] Failed to initialize BoardInteractionManager: {e}");
            _initializedBoardManager = false;
        }
    }

    /// <summary>
    /// Updates the screen prompt based on the current state of the board manager.
    /// </summary>
    /// <remarks>This method determines whether to show or hide a screen prompt depending on whether the board
    /// manager  is in a focused or hovering state. If the board manager is focused, the exit prompt is displayed.  If
    /// the board manager is hovering, the hover prompt is displayed. If neither condition is met,  the screen prompt is
    /// hidden.</remarks>
    private void UpdateScreenPrompt()
    {
        if (_boardManager == null)
        {
            HideScreenPrompt();
            return;
        }

        if (_boardManager.IsFocused)
        {
            ShowScreenPrompt(exitPrompt);
            return;
        }

        if (_boardManager.IsHovering)
        {
            ShowScreenPrompt(hoverPrompt);
            return;
        }
        HideScreenPrompt();
    }

    /// <summary>
    /// Displays a screen prompt with the specified text.
    /// </summary>
    /// <remarks>Ensures the screen prompt is visible if it is not already active. The method has no effect if
    /// the underlying  screen prompt object is not initialized.</remarks>
    /// <param name="text">The text to display in the screen prompt. If null or empty, the prompt will not be updated.</param>
    private void ShowScreenPrompt(string text)
    {
        if (screenPromptText == null) return;
        screenPromptText.text = text;
        if (!screenPromptText.gameObject.activeSelf)
            screenPromptText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the screen prompt by deactivating its associated GameObject and clearing its text.
    /// </summary>
    /// <remarks>This method ensures that the screen prompt is no longer visible by setting its GameObject to
    /// inactive  and resetting the text content to an empty string. If the screen prompt is already hidden, the method 
    /// performs no action.</remarks>
    private void HideScreenPrompt()
    {
        if (screenPromptText == null) return;
        if (screenPromptText.gameObject.activeSelf)
            screenPromptText.gameObject.SetActive(false);
        screenPromptText.text = string.Empty;
    }

    /// <summary>
    /// Gets a value indicating whether the board is currently focused.
    /// </summary>
    public bool IsBoardFocused => _boardManager != null && _boardManager.IsFocused;

    /// <summary>
    /// Permite que otros componentes (p. ej. BoardInteraction) notifiquen que han salido del foco.
    /// Delegará en el BoardInteractionManager para limpiar la referencia interna.
    /// </summary>
    public void ClearFocusedBoard(BoardInteraction board)
    {
        try
        {
            _boardManager?.ForceClearFocus(board);
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlayerInteractionManager] Error clearing focused board: {e}");
        }
    }
}