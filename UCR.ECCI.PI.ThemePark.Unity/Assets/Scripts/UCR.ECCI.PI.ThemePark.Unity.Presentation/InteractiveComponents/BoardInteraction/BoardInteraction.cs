using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BoardInteraction : MonoBehaviour
{
    private enum State { Idle, Hover, Focus }

    [Header("Camera Lock")]
    [Tooltip("Locks the Main Camera to frame this board while focused, restoring it on exit.")]
    public bool lockCameraOnFocus = true;
    [Tooltip("Fraction of vertical screen height the board should occupy when focused.")]
    [Range(0.3f, 0.95f)]
    public float camFocusScreenCoverage = 0.6f;
    [Tooltip("Additional world-up offset applied to the camera while focused.")]
    public float camVerticalOffset = 0f;
    [Tooltip("Clamp range for camera distance to the board while focused.")]
    public Vector2 camDistanceClamp = new Vector2(0.75f, 12f);
    [Tooltip("Duration (seconds) to move camera into focus framing.")]
    public float camFocusTravelTime = 0.35f;
    [Tooltip("Duration (seconds) to return camera to its original transform.")]
    public float camReturnTravelTime = 0.35f;
    [Tooltip("Camera components to disable during focus (e.g., CinemachineBrain or your custom camera controller).")]
    public Behaviour[] cameraComponentsToDisableOnFocus;

    [Header("Hover/Focus Prompt (optional)")]
    public Canvas popupCanvas;
    public Text popupText;
    public string hoverPrompt = "Press E to interact";
    public string exitPrompt = "Press E to exit";
    public Vector3 hoverPopupOffset = new Vector3(0f, 0.6f, 0.05f);
    public Vector3 focusPopupOffset = new Vector3(0f, 0.7f, 0.08f);
    public bool billboardPopup = true;
    public bool billboardOnlyYaw = true;

    [Header("Focus UI")]
    public Button backButton;

    private State _state = State.Idle;

    private Camera _mainCam;
    private BoardInteractionCamera _camHelper;

    private void Awake()
    {
        _mainCam = Camera.main;

        if (backButton != null)
        {
            backButton.gameObject.SetActive(false);
            backButton.onClick.AddListener(OnBackButtonPressed);
        }

        if (popupCanvas != null) popupCanvas.enabled = false;

        // Initialize camera helper with whatever was injected/assigned
        _camHelper = new BoardInteractionCamera(_mainCam);
        _camHelper.Configure(
            camFocusScreenCoverage,
            camVerticalOffset,
            camDistanceClamp,
            camFocusTravelTime,
            camReturnTravelTime,
            cameraComponentsToDisableOnFocus
        );
    }

    /// <summary>
    /// Injects camera controller components from a scene or manager.
    /// Reconfigures the internal camera helper accordingly.
    /// </summary>
    /// <param name="controllers">Array of <see cref="Behaviour"/> components to disable while focused. Null will be treated as an empty array.</param>
    public void InjectCameraControllers(Behaviour[] controllers)
    {
        cameraComponentsToDisableOnFocus = controllers ?? System.Array.Empty<Behaviour>();
        if (_mainCam == null) _mainCam = Camera.main;
        if (_camHelper == null) _camHelper = new BoardInteractionCamera(_mainCam);

        _camHelper.Configure(
            camFocusScreenCoverage,
            camVerticalOffset,
            camDistanceClamp,
            camFocusTravelTime,
            camReturnTravelTime,
            cameraComponentsToDisableOnFocus
        );
    }

    private void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonPressed);

        _camHelper?.ForceRelease();
    }

    private void Update()
    {
        HandlePopupBillboard();
    }

    private void LateUpdate()
    {
        _camHelper?.Tick(transform, _state == State.Focus);
    }

    /// <summary>
    /// Sets or clears the hover state. If currently focused, the call is ignored.
    /// Shows or hides the hover popup accordingly.
    /// </summary>
    /// <param name="isHovering">True to set hover state; false to clear it.</param>
    public void SetHover(bool isHovering)
    {
        if (_state == State.Focus) return;

        _state = isHovering ? State.Hover : State.Idle;

        if (_state == State.Hover) ShowPopup(hoverPrompt, hoverPopupOffset);
        else HidePopup();
    }

    /// <summary>
    /// Attempts to enter focus state. Only valid when currently hovered.
    /// The 'cam' parameter is kept for backwards compatibility but is not used for camera locking.
    /// If <see cref="lockCameraOnFocus"/> is false, this method does nothing.
    /// </summary>
    /// <param name="cam">Optional transform of the requesting camera (unused).</param>
    public void EnterFocus(Transform cam)
    {
        if (_state != State.Hover) return;
        if (!lockCameraOnFocus) return;

        _state = State.Focus;

        if (backButton != null) backButton.gameObject.SetActive(true);
        ShowPopup(exitPrompt, focusPopupOffset);

        _camHelper?.StartFocus(transform);
    }

    /// <summary>
    /// Exits focus state, restores camera behavior and hides focus UI.
    /// </summary>
    public void ExitFocus()
    {
        if (_state != State.Focus) return;

        _state = State.Idle;

        if (backButton != null) backButton.gameObject.SetActive(false);
        HidePopup();

        _camHelper?.StartReturn();

        if (PlayerInteractionManager.Instance != null)
        {
            PlayerInteractionManager.Instance.ClearFocusedBoard(this);
        }
    }

    private void OnBackButtonPressed() => ExitFocus();

    private void ShowPopup(string message, Vector3 localOffset)
    {
        if (popupCanvas == null || popupText == null) return;

        popupText.text = message;

        if (!popupCanvas.enabled) popupCanvas.enabled = true;
        if (!popupText.gameObject.activeSelf) popupText.gameObject.SetActive(true);

        popupCanvas.transform.localPosition = localOffset;
    }

    private void HidePopup()
    {
        if (popupText != null && popupText.gameObject.activeSelf)
        {
            popupText.gameObject.SetActive(false);
            popupText.text = string.Empty;
        }
        if (popupCanvas != null && popupCanvas.enabled)
            popupCanvas.enabled = false;
    }

    private void HandlePopupBillboard()
    {
        if (!billboardPopup || popupCanvas == null || !popupCanvas.enabled || _mainCam == null)
            return;

        var camTf = _mainCam.transform;
        if (billboardOnlyYaw)
        {
            Vector3 toCam = camTf.position - popupCanvas.transform.position;
            toCam.y = 0f;
            if (toCam.sqrMagnitude > 0.0001f)
                popupCanvas.transform.rotation = Quaternion.LookRotation(toCam.normalized, Vector3.up);
        }
        else
        {
            popupCanvas.transform.rotation = Quaternion.LookRotation(camTf.forward, Vector3.up);
        }
    }

    /// <summary>
    /// True when this board is currently focused.
    /// </summary>
    public bool IsFocused => _state == State.Focus;

    /// <summary>
    /// True when this board is currently hovered.
    /// </summary>
    public bool IsHover => _state == State.Hover;
}