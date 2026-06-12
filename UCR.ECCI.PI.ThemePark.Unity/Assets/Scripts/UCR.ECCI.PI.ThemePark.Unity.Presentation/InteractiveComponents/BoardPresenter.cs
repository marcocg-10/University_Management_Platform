using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents
{
    /// <summary>
    /// Responsible for rendering and updating the visual representation of a <see cref="Board"/> entity in the scene.
    /// Adjusts its position, scale, color, and orientation to match the underlying data.
    /// </summary>
    public class BoardPresenter : MonoBehaviour
    {
        private Board _boardData;
        private GameObject _labelObj;

        /// <summary>
        /// Initializes the presenter with the specified board data and renders it in the scene.
        /// </summary>
        public void Initialize(Board board)
        {
            _boardData = board;
            RenderBoard();
        }

        /// <summary>
        /// Ensures that the board label (if present) always faces the main camera.
        /// </summary>
        private void Update()
        {
            if (_labelObj != null && Camera.main != null)
            {
                _labelObj.transform.rotation = Quaternion.LookRotation(
                    _labelObj.transform.position - Camera.main.transform.position
                );
            }
        }

        /// <summary>
        /// Applies the board's data to its visual representation,
        /// including position, scale, rotation, and color.
        /// </summary>
        private void RenderBoard()
        {
            if (_boardData == null)
            {
                Debug.LogWarning("[BoardPresenter] No board data to render.");
                return;
            }

            transform.localPosition = new Vector3(
                (float)_boardData.Coordinates.X,
                (float)_boardData.Coordinates.Y,
                (float)_boardData.Coordinates.Z
            );

            transform.localScale = new Vector3(
                (float)_boardData.Dimensions.Width,
                (float)_boardData.Dimensions.Height,
                (float)_boardData.Dimensions.Depth
            );

            transform.rotation = Quaternion.Euler(
                (float)_boardData.Rotations.XAxisRotation,
                (float)_boardData.Rotations.YAxisRotation,
                (float)_boardData.Rotations.ZAxisRotation
            );

            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                if (ColorUtility.TryParseHtmlString(_boardData.Color.Value, out Color color))
                    renderer.material.color = color;
            }

            var drawer = GetComponent<BoardDrawer>();
            if (drawer != null)
            {
                var markerColor = ParseColor(_boardData.MarkerColor.Value);
                var boardColor = ParseColor(_boardData.Color.Value);

                drawer.SetBrushColor(markerColor, true);

                // Only repaint the board if its background color has actually changed
                if (drawer.boardColor != boardColor)
                    drawer.SetBoardColor(boardColor);

                var texture = _boardData.Texture;
                if (texture != null && !string.IsNullOrWhiteSpace(texture.Value))
                {
                    // Apply the texture with the board's tint color
                    drawer.ApplyBoardTexture(texture.Value, boardColor);
                }
            }
        }

        /// <summary>
        /// Parses a color value in HTML format (e.g., "#FF0000") into a <see cref="Color"/> object.
        /// Returns black if the parsing fails.
        /// </summary>
        /// <param name="colorValue">The color value as a string.</param>
        /// <returns>The parsed color or black as a fallback.</returns>
        private Color ParseColor(string colorValue)
        {
            return ColorUtility.TryParseHtmlString(colorValue, out Color c) ? c : Color.black;
        }
    }
}
