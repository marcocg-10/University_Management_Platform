using UnityEngine;

/// <summary>
/// Handles the Accept button inside the board color picker UI.
/// When the user clicks this button, it confirms the currently
/// selected brush color and applies it permanently to the board.
/// </summary>
public class BoardColorPickerAcceptButton : MonoBehaviour
{
    /// <summary>
    /// Called by the Unity UI Button component when the user clicks
    /// the Accept button. Delegates the confirmation action to the
    /// <see cref="BoardColorPickerManager"/> singleton.
    /// </summary>
    public void Accept()
    {
        if (BoardColorPickerManager.Instance != null)
        {
            BoardColorPickerManager.Instance.Accept();
        }
    }
}
