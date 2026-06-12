using UCR.ECCI.PI.ThemePark.Unity.Presentation.InteractiveComponents;
using UnityEngine;

/// <summary>
/// Central controller for the board color picker UI.
/// Handles showing/hiding the picker, applying preview colors,
/// confirming selections, or canceling changes.
/// </summary>
public class BoardColorPickerManager : MonoBehaviour
{
    /// <summary>Singleton instance of the color picker manager.</summary>
    public static BoardColorPickerManager Instance { get; private set; }

    [Header("References")]
    public FlexibleColorPicker fcp;
    public GameObject pickerPanel;

    private BoardDrawer _currentBoard; // Board currently being edited

    /// <summary>Whether the picker is currently visible.</summary>
    public bool IsVisible => pickerPanel.activeSelf;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        pickerPanel.SetActive(false);
    }

    /// <summary>
    /// Opens the picker for the given board, or closes it if already open
    /// for the same board.
    /// </summary>
    public void Toggle(BoardDrawer board)
    {
        if (_currentBoard == board && pickerPanel.activeSelf)
        {
            CancelAndClose();
            return;
        }

        _currentBoard = board;
        _currentBoard.originalBrushColor = _currentBoard.brushColor;
        fcp.color = _currentBoard.brushColor;
        board.SetIdleMode();
        pickerPanel.SetActive(true);
    }

    private void Update()
    {
        if (!pickerPanel.activeSelf || _currentBoard == null)
            return;

        _currentBoard.SetBrushPreviewColor(fcp.color);
    }

    /// <summary>
    /// Hides the picker and returns the board to drawing mode.
    /// Also updates button highlights (pencil active).
    /// </summary>
    public void Hide()
    {
        pickerPanel.SetActive(false);

        if (_currentBoard != null)
        {
            _currentBoard.SetDrawMode();

            var buttons = _currentBoard.GetComponent<BoardButtonManager>();
            if (buttons != null)
                buttons.SetPencilActive();
        }

        _currentBoard = null;
    }

    /// <summary>
    /// Cancels the color change by restoring the original brush color,
    /// then closes the picker.
    /// </summary>
    public void CancelAndClose()
    {
        if (_currentBoard != null)
        {
            _currentBoard.ApplyBrushColor(_currentBoard.originalBrushColor);
        }

        Hide();
    }

    /// <summary>
    /// Accepts the currently selected color and applies it permanently.
    /// </summary>
    public void Accept()
    {
        if (_currentBoard == null) return;

        _currentBoard.ApplyBrushColor(_currentBoard.tempBrushColor);

        Hide();
    }

    /// <summary>
    /// Resets the color picker and brush color to the board's default color.
    /// </summary>
    public void ResetToDefault()
    {
        if (_currentBoard == null) return;

        _currentBoard.SetBrushColor(_currentBoard.brushDefaultColor);

        fcp.color = _currentBoard.brushDefaultColor;
    }
}
