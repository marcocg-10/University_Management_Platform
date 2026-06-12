using UnityEngine;
using UnityEngine.InputSystem;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents
{
    /// <summary>
    /// Handles cursor appearance when hovering over a board surface.
    /// Shows a pencil cursor in draw mode, an eraser cursor in erase mode,
    /// and resets when the cursor leaves the board.
    /// </summary>
    public class BoardCursorManager : MonoBehaviour
    {
        [Header("Cursor Textures")]
        public Texture2D pencilCursor;
        public Texture2D eraserCursor;

        // Hotspots (center of the cursor)
        public Vector2 pencilHotspot = new Vector2(0, 0);
        public Vector2 eraserHotspot = new Vector2(0, 0);

        private BoardDrawer _drawer;
        private bool _isHoveringBoard;

        private void Start()
        {
            _drawer = GetComponent<BoardDrawer>();
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;

            // Disable board cursor entirely when color picker is open
            if (BoardColorPickerManager.Instance != null &&
                BoardColorPickerManager.Instance.IsVisible)
            {
                if (_isHoveringBoard)
                {
                    _isHoveringBoard = false;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
                return;
            }

            Camera cam = Camera.main;
            if (cam == null)
                return;

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            // Raycast to detect board surface
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!_isHoveringBoard)
                    {
                        _isHoveringBoard = true;
                        UpdateCursor();
                    }
                    return;
                }
            }

            // Not hovering anymore, reset cursor
            if (_isHoveringBoard)
            {
                _isHoveringBoard = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        /// <summary>
        /// Called externally when draw/erase mode changes.
        /// Updates the cursor if hovering.
        /// </summary>
        public void UpdateCursor()
        {
            if (!_isHoveringBoard)
                return;

            if (_drawer != null)
            {
                if (_drawer.IsErasing)
                    Cursor.SetCursor(eraserCursor, eraserHotspot, CursorMode.Auto);
                else
                    Cursor.SetCursor(pencilCursor, pencilHotspot, CursorMode.Auto);
            }
        }

        /// <summary>
        /// Ensures the system cursor is restored when the component is disabled
        /// or the board is destroyed while the cursor is hovering over it.
        /// </summary>
        private void OnDisable()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
