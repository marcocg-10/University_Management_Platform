using UnityEngine;

/// <summary>
/// Helper that manages camera framing and animated transitions when a <see cref="BoardInteraction"/> is focused.
/// Responsible for computing a target camera transform that frames the board, animating the camera to/from that transform,
/// and disabling/enabling external camera controller components while focused.
/// </summary>
public class BoardInteractionCamera
{
    private readonly Camera _cam;
    private readonly Transform _camTf;

    // Config
    private float _coverage = 0.6f;
    private float _verticalOffset = 0f;
    private Vector2 _distanceClamp = new Vector2(0.75f, 12f);
    private float _focusTravelTime = 0.35f;
    private float _returnTravelTime = 0.35f;
    private Behaviour[] _componentsToDisable;

    // Camera anim state
    private enum Anim { None, ToFocus, ToOriginal }
    private Anim _anim = Anim.None;
    private float _animElapsed;
    private Vector3 _startPos, _targetPos, _origPos;
    private Quaternion _startRot, _targetRot, _origRot;

    private bool _wasFocused;

    // Track whether the original camera transform has been captured for a focus session.
    private bool _originalCached = false;

    /// <summary>
    /// Creates a new instance bound to the provided camera.
    /// </summary>
    /// <param name="cam">Camera to control. May be null; methods gracefully no-op if camera is absent.</param>
    public BoardInteractionCamera(Camera cam)
    {
        _cam = cam;
        _camTf = cam != null ? cam.transform : null;
        // Do not prematurely mark original as cached — capture when a focus begins.
        _originalCached = false;
    }

    /// <summary>
    /// Configures framing parameters and which components will be disabled while a board is focused.
    /// </summary>
    /// <param name="focusScreenCoverage">Fraction of vertical screen height the board should occupy when focused (clamped internally).</param>
    /// <param name="verticalOffset">Additional world-up offset applied to the camera while focused.</param>
    /// <param name="distanceClamp">Min/max allowed camera distance from the board when framing.</param>
    /// <param name="focusTravelTime">Duration in seconds to move camera into focus framing.</param>
    /// <param name="returnTravelTime">Duration in seconds to return camera to its original transform.</param>
    /// <param name="componentsToDisable">Array of <see cref="Behaviour"/> components to disable while focused. May be null.</param>
    public void Configure(
        float focusScreenCoverage,
        float verticalOffset,
        Vector2 distanceClamp,
        float focusTravelTime,
        float returnTravelTime,
        Behaviour[] componentsToDisable)
    {
        _coverage = Mathf.Clamp(focusScreenCoverage, 0.3f, 0.95f);
        _verticalOffset = verticalOffset;
        _distanceClamp = new Vector2(Mathf.Max(0.01f, distanceClamp.x), Mathf.Max(distanceClamp.x, distanceClamp.y));
        _focusTravelTime = Mathf.Max(0.01f, focusTravelTime);
        _returnTravelTime = Mathf.Max(0.01f, returnTravelTime);
        _componentsToDisable = componentsToDisable;
    }

    /// <summary>
    /// Begins the focus transition toward the given board. Disables configured external controllers.
    /// </summary>
    /// <param name="board">Transform of the board to focus. If null or camera missing, the call is ignored.</param>
    public void StartFocus(Transform board)
    {
        if (_camTf == null || board == null) return;

        // Capture original camera transform only when starting focus so we can return to it later.
        CacheOriginalIfNeeded();

        // Disable external controllers while focused
        SetControllersEnabled(false);

        ComputeFocusTarget(board, out _targetPos, out _targetRot);

        _startPos = _camTf.position;
        _startRot = _camTf.rotation;
        _animElapsed = 0f;
        _anim = Anim.ToFocus;
    }

    /// <summary>
    /// Begins the animated return to the original camera transform captured before focus.
    /// </summary>
    public void StartReturn()
    {
        if (_camTf == null) return;

        // Do NOT overwrite the cached original here — we need the original captured at StartFocus.
        _startPos = _camTf.position;
        _startRot = _camTf.rotation;
        _animElapsed = 0f;
        _anim = Anim.ToOriginal;
    }

    /// <summary>
    /// Ticks the internal animation and keeps the camera locked while focused.
    /// Must be called regularly (for example from LateUpdate of the owning <see cref="BoardInteraction"/>).
    /// </summary>
    /// <param name="board">Transform of the board being focused. Used to recompute lock target when not animating.</param>
    /// <param name="isFocused">Whether the board is currently focused.</param>
    public void Tick(Transform board, bool isFocused)
    {
        if (_camTf == null) return;

        if (isFocused)
        {
            if (!_wasFocused && _anim == Anim.None)
            {
                // Entering focus but no anim set yet (safety)
                StartFocus(board);
            }

            if (_anim == Anim.ToFocus)
            {
                _animElapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_animElapsed / _focusTravelTime));

                _camTf.position = Vector3.Lerp(_startPos, _targetPos, t);
                _camTf.rotation = Quaternion.Slerp(_startRot, _targetRot, t);

                if (t >= 1f) _anim = Anim.None;
            }
            else
            {
                // Lock camera onto board every frame (if board or player moves)
                ComputeFocusTarget(board, out _targetPos, out _targetRot);
                _camTf.position = _targetPos;
                _camTf.rotation = _targetRot;
            }
        }
        else
        {
            if (_wasFocused && _anim == Anim.None)
            {
                StartReturn();
            }

            if (_anim == Anim.ToOriginal)
            {
                _animElapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_animElapsed / _returnTravelTime));

                _camTf.position = Vector3.Lerp(_startPos, _origPos, t);
                _camTf.rotation = Quaternion.Slerp(_startRot, _origRot, t);

                if (t >= 1f)
                {
                    _anim = Anim.None;
                    // Re-enable controllers after returning
                    SetControllersEnabled(true);
                    // Allow next focus to capture a fresh original
                    _originalCached = false;
                }
            }
        }

        _wasFocused = isFocused;
    }

    /// <summary>
    /// Forces release of any temporary state and re-enables controllers.
    /// Use when the owner is destroyed while focus was active.
    /// </summary>
    public void ForceRelease()
    {
        // Re-enable controllers if something destroys the owner while focused
        SetControllersEnabled(true);
        // Clear any cached original so future sessions will recapture
        _originalCached = false;
        _anim = Anim.None;
        _wasFocused = false;
    }

    /// <summary>
    /// Computes a camera pose that frames the provided board according to current configuration.
    /// Uses renderer bounds when available, otherwise falls back to collider bounds or the board transform.
    /// </summary>
    /// <param name="board">Board transform to frame.</param>
    /// <param name="pos">Computed target camera position.</param>
    /// <param name="rot">Computed target camera rotation.</param>
    private void ComputeFocusTarget(Transform board, out Vector3 pos, out Quaternion rot)
    {
        Renderer r = board.GetComponent<Renderer>();
        Collider c = (r == null) ? board.GetComponent<Collider>() : null;

        Vector3 center = r != null ? r.bounds.center :
                         c != null ? c.bounds.center : board.position;

        float height = r != null ? r.bounds.size.y :
                       c != null ? c.bounds.size.y : board.lossyScale.y;

        float fovRad = _cam.fieldOfView * Mathf.Deg2Rad;
        float denom = 2f * Mathf.Tan(fovRad * 0.5f) * _coverage;
        float d = denom > 0.0001f ? (height / denom) : 2f;
        d = Mathf.Clamp(d, _distanceClamp.x, _distanceClamp.y);

        Vector3 forward = board.forward;

        pos = center + forward * d + Vector3.up * _verticalOffset;
        rot = Quaternion.LookRotation(center - pos, Vector3.up);
    }

    /// <summary>
    /// Ensures the original camera transform is recorded for subsequent return animations.
    /// </summary>
    private void CacheOriginalIfNeeded()
    {
        if (_camTf == null) return;
        if (_originalCached) return;

        _origPos = _camTf.position;
        _origRot = _camTf.rotation;
        _originalCached = true;
    }

    /// <summary>
    /// Enables or disables the configured external controller components.
    /// Null-safe.
    /// </summary>
    /// <param name="enabled">True to enable, false to disable.</param>
    private void SetControllersEnabled(bool enabled)
    {
        if (_componentsToDisable == null) return;
        for (int i = 0; i < _componentsToDisable.Length; i++)
        {
            var comp = _componentsToDisable[i];
            if (comp == null) continue;
            comp.enabled = enabled;
        }
    }
}