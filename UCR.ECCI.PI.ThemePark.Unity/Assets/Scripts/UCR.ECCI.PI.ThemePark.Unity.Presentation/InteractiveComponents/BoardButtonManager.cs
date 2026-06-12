using UnityEngine;
using UnityEngine.InputSystem;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents
{
    /// <summary>
    /// Manages the interactive 3D buttons (pencil, eraser, and color) attached to a board surface.
    /// Responsible for creating, displaying, and updating the buttons that allow the user to
    /// switch between drawing, erasing, and selecting brush colors.
    /// The buttons remain gray by default, and the active button turns white for clarity.
    /// </summary>
    [RequireComponent(typeof(BoardDrawer))]
    [RequireComponent(typeof(Renderer))]
    public class BoardButtonManager : MonoBehaviour
    {
        /// <summary>
        /// Icon for the pencil button (PNG or Texture2D).
        /// </summary>
        [Header("Button Icon Settings")]
        [Tooltip("Icon for the pencil button (PNG or Texture2D).")]
        public Texture2D pencilIcon;

        /// <summary>
        /// Icon for the eraser button (PNG or Texture2D).
        /// </summary>
        [Tooltip("Icon for the eraser button (PNG or Texture2D).")]
        public Texture2D eraserIcon;

        /// <summary>
        /// Icon for the color picker button (PNG or Texture2D).
        /// </summary>
        [Tooltip("Icon for the color picker button (PNG or Texture2D).")]
        public Texture2D colorIcon;

        /// <summary>
        /// Icon for the brush size button (PNG or Texture2D).
        /// </summary>
        [Tooltip("Icon for the brush size button (PNG or Texture2D).")]
        public Texture2D brushSizeIcon;

        private GameObject _pencilButton;       // Reference to the pencil button GameObject.
        private GameObject _eraserButton;       // Reference to the eraser button GameObject.
        private GameObject _colorButton;        // Reference to the color picker button GameObject.
        private GameObject _brushSizeButton;    // Reference to the brush size picker button GameObject.


        private Renderer _pencilRenderer;       // Renderer for the pencil button.
        private Renderer _eraserRenderer;       // Renderer for the eraser button.
        private Renderer _colorRenderer;        // Renderer for the color button.
        private Renderer _brushSizeRenderer;    // Renderer for the brush size button.

        private Renderer _pencilIconRenderer;   // Renderer for the pencil button's icon.
        private Renderer _eraserIconRenderer;   // Renderer for the eraser button's icon.
        private Renderer _colorIconRenderer;    // Renderer for the color button's icon.
        private Renderer _brushSizeIconRenderer;// Renderer for the brush size button's icon.

        private BoardDrawer _drawer;            // Reference to the board's drawing component.
        private Renderer _renderer;             // Renderer of the board to calculate placement.

        /// <summary>
        /// Ensures clicked object belongs to this board.
        /// Prevents cross-board interaction.
        /// </summary>
        private bool IsFromThisBoard(Transform t) => t.IsChildOf(transform);

        /// <summary>
        /// Initializes the button system and sets the initial active mode to draw.
        /// </summary>
        private void Start()
        {
            _drawer = GetComponent<BoardDrawer>();
            _renderer = GetComponent<Renderer>();
            Create3DButtons();
            SetActiveButton(_pencilButton);
        }

        /// <summary>
        /// Detects mouse input and handles button click interactions.
        /// </summary>
        private void Update()
        {
            if (Mouse.current?.leftButton.wasPressedThisFrame == true)
                TryClickButton();
        }

        /// <summary>
        /// Destroys dynamically created materials to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            if (_pencilRenderer != null && _pencilRenderer.material != null)
                Destroy(_pencilRenderer.material);

            if (_eraserRenderer != null && _eraserRenderer.material != null)
                Destroy(_eraserRenderer.material);

            if (_colorRenderer != null && _colorRenderer.material != null)
                Destroy(_colorRenderer.material);

            if (_brushSizeRenderer != null && _brushSizeRenderer.material != null)
                Destroy(_brushSizeRenderer.material);

            if (_pencilIconRenderer != null && _pencilIconRenderer.material != null)
                Destroy(_pencilIconRenderer.material);

            if (_eraserIconRenderer != null && _eraserIconRenderer.material != null)
                Destroy(_eraserIconRenderer.material);

            if (_colorIconRenderer != null && _colorIconRenderer.material != null)
                Destroy(_colorIconRenderer.material);

            if (_brushSizeIconRenderer != null && _brushSizeIconRenderer.material != null)
                Destroy(_brushSizeIconRenderer.material);
        }

        /// <summary>
        /// Creates the 3D buttons (pencil, eraser, and color) and positions them
        /// slightly above the board surface for visibility.
        /// </summary>
        private void Create3DButtons()
        {
            Bounds bounds = _renderer.localBounds;

            float buttonSize = Mathf.Min(bounds.size.x, bounds.size.y) * 0.08f;
            float buttonDepth = bounds.size.z + 0.0005f;

            float topY = bounds.max.y - buttonSize;
            float rightX = bounds.max.x - (buttonSize / 2);
            float zOffset = 0f;

            // Pencil button on the left
            _pencilButton = CreateButton("PencilButton", new Vector3(
                rightX - (buttonSize * 0.6f), topY, zOffset), buttonSize, buttonDepth);

            // Eraser button in the middle
            _eraserButton = CreateButton("EraserButton", new Vector3(
                rightX, topY, zOffset), buttonSize, buttonDepth);

            // Color button on the right
            _colorButton = CreateButton("ColorButton", new Vector3(
                rightX - (buttonSize * 1.2f), topY, zOffset), buttonSize, buttonDepth);

            // Brush size button on the right
            _brushSizeButton = CreateButton("BrushSizeButton", new Vector3(
                rightX - (buttonSize * 1.8f), topY, zOffset), buttonSize, buttonDepth);

            _pencilRenderer = _pencilButton.GetComponent<Renderer>();
            _eraserRenderer = _eraserButton.GetComponent<Renderer>();
            _colorRenderer = _colorButton.GetComponent<Renderer>();
            _brushSizeRenderer = _brushSizeButton.GetComponent<Renderer>();

            // All buttons start gray
            _pencilRenderer.material.color = Color.gray;
            _eraserRenderer.material.color = Color.gray;
            _colorRenderer.material.color = Color.gray;
            _brushSizeRenderer.material.color = Color.gray;

            // Add icons
            _pencilIconRenderer = AddIconPlane(_pencilButton, pencilIcon);
            _eraserIconRenderer = AddIconPlane(_eraserButton, eraserIcon);
            _colorIconRenderer = AddIconPlane(_colorButton, colorIcon);
            _brushSizeIconRenderer = AddIconPlane(_brushSizeButton, brushSizeIcon);
        }

        /// <summary>
        /// Creates a cube-based 3D button with an unlit material.
        /// </summary>
        /// <param name="name">The name of the button GameObject.</param>
        /// <param name="position">Local position relative to the board.</param>
        /// <param name="size">Width and height of the button.</param>
        /// <param name="depth">Depth (thickness) of the button.</param>
        /// <returns>The created button GameObject.</returns>
        private GameObject CreateButton(string name, Vector3 position, float size, float depth)
        {
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            button.name = name;
            button.transform.SetParent(transform, false);
            button.transform.localScale = new Vector3(size / 2, size, depth);
            button.transform.localPosition = position;

            // Remove the automatically added collider (we use icon plane colliders instead)
            Collider autoCollider = button.GetComponent<Collider>();
            if (autoCollider != null)
                Destroy(autoCollider);

            // Assign tags
            if (name.Contains("Pencil"))
                button.tag = "PencilButton";
            else if (name.Contains("Eraser"))
                button.tag = "EraserButton";
            else if (name.Contains("Color"))
                button.tag = "ColorButton";
            else if (name.Contains("BrushSize"))
                button.tag = "BrushSizeButton";

            // Use unlit constant material
            Renderer renderer = button.GetComponent<Renderer>();
            Shader unlitColorShader = Shader.Find("Unlit/Color");
            if (unlitColorShader != null)
                renderer.material = new Material(unlitColorShader) { color = Color.gray };
            else
                UnityEngine.Debug.LogError("Shader 'Unlit/Color' not found. Button material not assigned.");

            return button;
        }

        /// <summary>
        /// Adds a transparent PNG icon plane on top of the given button.
        /// </summary>
        /// <param name="button">The parent button GameObject.</param>
        /// <param name="icon">The icon texture to apply.</param>
        /// <returns>The renderer of the created icon plane.</returns>
        private Renderer AddIconPlane(GameObject button, Texture2D icon)
        {
            if (icon == null)
                return null;

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.name = button.name + "_Icon";
            plane.transform.SetParent(button.transform, false);

            Renderer buttonRenderer = button.GetComponent<Renderer>();
            var faceSize = buttonRenderer.bounds.size;

            // Slight offset to ensure visibility
            plane.transform.localPosition = new Vector3(
                0,
                0,
                button.transform.localScale.z / 2f);

            plane.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            plane.tag = button.tag;

            // Transparent material for PNG icons
            Shader transparentShader = Shader.Find("Unlit/Transparent");
            if (transparentShader == null)
            {
                UnityEngine.Debug.LogError("Shader 'Unlit/Transparent' not found. Cannot create transparent material for button icon.");
                Destroy(plane);
                return null;
            }

            Material mat = new Material(transparentShader)
            {
                mainTexture = icon
            };

            Renderer renderer = plane.GetComponent<Renderer>();
            renderer.material = mat;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            return renderer;
        }

        /// <summary>
        /// Detects user clicks on the buttons via raycast and switches between draw, erase,
        /// and color selection modes.
        /// </summary>
        private void TryClickButton()
        {
            Camera cam = Camera.main;
            if (cam == null)
                return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (!Physics.Raycast(cam.ScreenPointToRay(mousePos), out RaycastHit hit))
                return;

            if (hit.collider == null)
                return;

            GameObject clicked = hit.collider.gameObject;

            // Pencil button or its icon
            if (clicked.CompareTag("PencilButton") && IsFromThisBoard(clicked.transform))
            {
                _drawer.SetDrawMode();
                SetActiveButton(_pencilButton);
                return;
            }

            // Eraser button or its icon
            if (clicked.CompareTag("EraserButton") && IsFromThisBoard(clicked.transform))
            {
                _drawer.SetEraseMode();
                SetActiveButton(_eraserButton);
                return;
            }

            // Color button or its icon
            if (clicked.CompareTag("ColorButton") && IsFromThisBoard(clicked.transform))
            {
                _drawer.SetIdleMode();
                BoardColorPickerManager.Instance.Toggle(_drawer);
                SetActiveButton(_colorButton);
                return;
            }

            if (clicked.CompareTag("BrushSizeButton") && IsFromThisBoard(clicked.transform))
            {
                _drawer.SetIdleMode();
                BoardBrushSizeManager.Instance.Toggle(_drawer);
                SetActiveButton(_brushSizeButton);
                return;
            }
        }

        /// <summary>
        /// Highlights the active button (white) and resets inactive buttons to gray.
        /// </summary>
        /// <param name="activeButton">The button GameObject that is currently active.</param>
        private void SetActiveButton(GameObject activeButton)
        {
            if (_pencilRenderer == null || _eraserRenderer == null || _colorRenderer == null || _brushSizeRenderer == null)
                return;

            _pencilRenderer.material.color = Color.gray;
            _eraserRenderer.material.color = Color.gray;
            _colorRenderer.material.color = Color.gray;
            _brushSizeRenderer.material.color = Color.gray;

            // Active button turns white
            if (activeButton == _pencilButton)
                _pencilRenderer.material.color = Color.white;
            else if (activeButton == _eraserButton)
                _eraserRenderer.material.color = Color.white;
            else if (activeButton == _colorButton)
                _colorRenderer.material.color = Color.white;
            else if (activeButton == _brushSizeButton)
                _brushSizeRenderer.material.color = Color.white;
        }

        public void SetPencilActive()
        {
            _drawer.SetDrawMode();
            SetActiveButton(_pencilButton);
        }
    }
}
