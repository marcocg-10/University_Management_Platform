using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation
{
    /// <summary>
    /// Class for the accept button on the board brush size.
    /// </summary>
    public class BoardBrushSizeAcceptButton : MonoBehaviour
    {
        /// <summary>
        /// Accepts the new brush size.
        /// </summary>
        public void Accept()
        {
            if (BoardBrushSizeManager.Instance != null)
            {
                BoardBrushSizeManager.Instance.Accept();
            }
        }
    }
}
