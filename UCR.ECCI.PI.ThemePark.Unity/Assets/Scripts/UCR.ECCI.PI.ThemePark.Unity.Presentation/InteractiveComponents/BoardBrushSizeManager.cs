using UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents;
using UnityEngine;
using UnityEngine.UI;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation
{
    /// <summary>
    /// Manages the UI panel that controls the brush size used by a <see cref="BoardDrawer"/>.
    /// Handles showing/hiding the panel, syncing the slider with the selected board,
    /// and applying brush size changes in real time.
    /// </summary>
    public class BoardBrushSizeManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance for global access from board interaction tools.
        /// </summary>
        public static BoardBrushSizeManager Instance { get; private set; }

        /// <summary>
        /// Components for the User Interface.
        /// </summary>
        [Header("UI Components")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Slider _sizeSlider;
        [SerializeField] private Text _pixelSize;

        /// <summary>
        /// Temporary variables.
        /// </summary>
        private int _tempBrushSize; 

        /// <summary>
        /// The active board currently controlled by the brush size slider.
        /// </summary>
        private BoardDrawer _boardDrawer;

        private void Awake()
        {
            // Ensure only a single manager exists.
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Hide panel at startup.
            _panel.SetActive(false);

            // Configure slider range.
            _sizeSlider.minValue = 1;
            _sizeSlider.maxValue = 50;
            _sizeSlider.wholeNumbers = true;
        }

        /// <summary>
        /// Opens or closes the panel and binds it to the provided <see cref="BoardDrawer"/>.
        /// Replaces old listeners to avoid stacking and initializes the UI with the drawer's current size.
        /// </summary>
        /// <param name="drawer">The board drawer whose brush size will be modified.</param>
        public void Toggle(BoardDrawer drawer)
        {
            if (drawer == null)
            {
                Debug.LogWarning("BoardBrushSizeManager.Toggle received NULL drawer.");
                return;
            }

            // Assign drawer before events so callbacks are valid.
            _boardDrawer = drawer;

            // Reset listeners to prevent duplicates from previous toggles.
            _sizeSlider.onValueChanged.RemoveAllListeners();
            _sizeSlider.onValueChanged.AddListener(OnBrushSizeChanged);

            // Sync slider and UI with current board brush size.
            _sizeSlider.value = _boardDrawer.brushSize;
            OnBrushSizeChanged(_sizeSlider.value);

            // Toggle visibility.
            _panel.SetActive(!_panel.activeSelf);
        }

        /// <summary>
        /// Applies the new brush size to the active board and updates the label.
        /// </summary>
        private void OnBrushSizeChanged(float value)
        {
            int intValue = Mathf.RoundToInt(value);
            _pixelSize.text = $"Size: {intValue} pixel{(intValue == 1 ? "" : "s")}";

            // Ignore early slider events when no board is bound.
            if (_boardDrawer == null)
                return;

            _tempBrushSize = intValue;
        }

        /// <summary>
        /// Accepts the new size for the brush and sets it using the board drawer.
        /// </summary>
        public void Accept()
        {
            if (_boardDrawer == null) return;

            _boardDrawer.SetBrushSize(_tempBrushSize);

            Hide();
        }

        /// <summary>
        /// Hides the panel for the board brush size slider.
        /// </summary>
        public void Hide()
        {
            _panel.SetActive(false);

            if (_boardDrawer != null)
            {
                _boardDrawer.SetDrawMode();

                var buttons = _boardDrawer.GetComponent<BoardButtonManager>();
                if (buttons != null)
                    buttons.SetPencilActive();
            }

            _boardDrawer = null;
        }

        /// <summary>
        /// Cancels the change of brush size.
        /// </summary>
        public void CancelAndClose()
        {
            if (_boardDrawer == null) return;

            Hide();
        }
    }
}
