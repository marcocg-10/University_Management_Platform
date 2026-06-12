using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI back button visibility and behaviour while a board interaction is focused.
/// </summary>
public class BoardFocusUI : MonoBehaviour
{
    /// <summary>
    /// Reference to the Back button in the UI.
    /// </summary>
    [Tooltip("Referencia al botón Back en la UI.")]
    public Button backButton;

    /// <summary>
    /// Reference to the BoardInteractionManager that controls focus and interactions.
    /// </summary>
    [Tooltip("Referencia al BoardInteractionManager.")]
    public BoardInteractionManager interactionManager;

    /// <summary>
    /// Reference to the drawing/erasing tools panel that should contain the Back button when focused.
    /// </summary>
    [Tooltip("Referencia al panel de herramientas de dibujo/borrado.")]
    public GameObject toolsPanel;

    /// <summary>
    /// Initialize UI state and register the Back button callback.
    /// </summary>
    private void Awake()
    {
        backButton.gameObject.SetActive(false);
        backButton.onClick.AddListener(OnBackPressed);
    }

    /// <summary>
    /// Toggles the Back button visibility based on the interaction focus and ensures it is parented
    /// to the tools panel while the board is focused.
    /// </summary>
    /// <remarks>
    /// This runs every frame to reflect the current focus state from the interaction manager.
    /// </remarks>
    private void Update()
    {
        if (interactionManager != null && interactionManager.IsFocused)
        {
            backButton.gameObject.SetActive(true);
            if (toolsPanel != null && backButton.transform.parent != toolsPanel.transform)
            {
                backButton.transform.SetParent(toolsPanel.transform, false);
            }
        }
        else
        {
            backButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Called when the Back button is pressed. Requests the interaction manager to end focus.
    /// </summary>
    private void OnBackPressed()
    {
        if (interactionManager != null && interactionManager.IsFocused)
        {
            interactionManager.HandleInteract(false);
        }
    }
}