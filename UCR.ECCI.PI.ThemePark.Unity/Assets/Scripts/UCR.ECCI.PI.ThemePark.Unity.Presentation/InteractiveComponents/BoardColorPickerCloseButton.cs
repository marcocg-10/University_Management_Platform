using UnityEngine;

/// <summary>
/// Handles the Close (X) button inside the board color picker UI.
/// When pressed, the current preview color is discarded and the
/// brush color is restored to the value it had before the picker
/// was opened.
/// </summary>
public class BoardColorPickerCloseButton : MonoBehaviour
{
    /// <summary>
    /// Called by the Unity UI Button component when the user clicks
    /// the Close (X) button. Delegates the cancellation behavior to
    /// the <see cref="BoardColorPickerManager"/> singleton, which
    /// restores the original brush color and hides the picker panel.
    /// </summary>
    public void ClosePicker()
    {
        if (BoardColorPickerManager.Instance != null)
        {
            BoardColorPickerManager.Instance.CancelAndClose();
        }
    }
}
