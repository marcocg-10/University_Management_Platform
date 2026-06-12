using UnityEngine;

/// <summary>
/// Handles the Reset button inside the board color picker UI.
/// Restores the brush color to the board's default value.
/// </summary>
public class BoardColorPickerResetButton : MonoBehaviour
{
    /// <summary>
    /// Called by the Unity UI Button component when the user clicks Reset.
    /// Delegates the action to <see cref="BoardColorPickerManager"/>,
    /// which applies the board's stored default brush color.
    /// </summary>
    public void ResetColor()
    {
        if (BoardColorPickerManager.Instance != null)
        {
            BoardColorPickerManager.Instance.ResetToDefault();
        }
    }
}
