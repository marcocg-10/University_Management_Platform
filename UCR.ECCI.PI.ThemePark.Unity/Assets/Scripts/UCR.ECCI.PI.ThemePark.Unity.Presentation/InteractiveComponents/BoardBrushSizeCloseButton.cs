using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation
{
    /// <summary>
    /// Class for the close button on the board brush size manager.
    /// </summary>
    public class BoardBrushSizeCloseButton : MonoBehaviour
    {
        /// <summary>
        /// Closes the panel with the board brush size slider.
        /// </summary>
        public void Close()
        {
            if (BoardBrushSizeManager.Instance != null)
            {
                BoardBrushSizeManager.Instance.CancelAndClose();
            }
        }
    }
}
