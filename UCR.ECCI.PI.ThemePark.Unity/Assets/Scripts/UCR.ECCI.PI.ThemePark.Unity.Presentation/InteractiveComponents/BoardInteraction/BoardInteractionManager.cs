using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles cursor-based hover detection and delegates focus/exit requests to a single focus method on boards.
/// </summary>
public class BoardInteractionManager
{
    private Transform _cameraTransform;
    private float _raycastDistance;
    private LayerMask _layerMask;

    private BoardInteraction _hoveredBoard;
    private BoardInteraction _focusedBoard;

    public void Initialize(Transform cameraTransform, float raycastDistance, LayerMask layerMask, bool debug)
    {
        _cameraTransform = cameraTransform;
        _raycastDistance = raycastDistance;
        _layerMask = layerMask;
    }

    /// <summary>
    /// Performs a cursor raycast each frame (when not focused) to detect hover state.
    /// </summary>
    public void Update(bool debug)
    {
        if (_focusedBoard != null) return;

        if (_cameraTransform == null || Mouse.current == null)
        {
            ClearHovered();
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            ClearHovered();
            return;
        }

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            _raycastDistance,
            _layerMask.value == 0 ? Physics.DefaultRaycastLayers : _layerMask,
            QueryTriggerInteraction.Ignore
        );

        if (hits.Length == 0)
        {
            ClearHovered();
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        BoardInteraction foundBoard = null;
        foreach (var hit in hits)
        {
            var bi = hit.collider.GetComponent<BoardInteraction>();
            if (bi != null)
            {
                foundBoard = bi;
                break;
            }
        }

        if (foundBoard == null)
        {
            ClearHovered();
            return;
        }

        if (_hoveredBoard != foundBoard)
        {
            ClearHovered();
            _hoveredBoard = foundBoard;
            _hoveredBoard.SetHover(true);
        }
    }

    /// <summary>
    /// Handles the interact key:
    /// - If a board is focused: exits focus.
    /// - If hovering: enters focus (centered in front of camera).
    /// Returns true if the action was consumed.
    /// </summary>
    public bool HandleInteract(bool debug)
    {
        if (_focusedBoard != null && _focusedBoard.IsFocused)
        {
            _focusedBoard.ExitFocus();
            _focusedBoard = null;
            return true;
        }

        if (_hoveredBoard != null && _hoveredBoard.IsHover)
        {
            _hoveredBoard.EnterFocus(_cameraTransform);
            _focusedBoard = _hoveredBoard;
            return true;
        }

        return false;
    }

    private void ClearHovered()
    {
        if (_hoveredBoard != null)
        {
            _hoveredBoard.SetHover(false);
            _hoveredBoard = null;
        }
    }

    /// <summary>
    /// Force clears the current focused board if it matches the provided instance.
    /// Used when focus is exited by an external UI button so the manager resumes hover detection.
    /// </summary>
    public void ForceClearFocus(BoardInteraction board)
    {
        if (_focusedBoard == board)
        {
            _focusedBoard = null;
        }
    }

    public bool IsFocused => _focusedBoard != null && _focusedBoard.IsFocused;
    public bool IsHovering => _hoveredBoard != null && _hoveredBoard.IsHover;
    public BoardInteraction HoveredBoard => _hoveredBoard;
    public BoardInteraction FocusedBoard => _focusedBoard;
}