using Microsoft.AspNetCore.Components;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Pages;

public partial class InteractiveComponentsPage : ComponentBase
{
    protected override async Task OnInitializedAsync()
    {
        UpdateEffectivePageSize();
        await Task.WhenAll(LoadBoardsAsync(), LoadProjectorsAsync());
    }

    private enum InteractiveComponentsTab { Boards, Projectors }
    private InteractiveComponentsTab SelectedTab { get; set; } = InteractiveComponentsTab.Boards;
    private void SelectTab(InteractiveComponentsTab tab) => SelectedTab = tab;


    private int _pageSize = 10;

    private string _selectedPageSizeOptionBacking = "10";
    private int _customPageSizeBacking = 10;

    private string _selectedPageSizeOption
    {
        get => _selectedPageSizeOptionBacking;
        set
        {
            if (_selectedPageSizeOptionBacking == value) return;
            _selectedPageSizeOptionBacking = value;

            UpdateEffectivePageSize();
            _ = LoadBoardsAsync();
        }
    }

    private int _customPageSize
    {
        get => _customPageSizeBacking;
        set
        {
            _customPageSizeBacking = value;
        }
    }

    private void ApplyCustomPageSize()
    {
        if (_selectedPageSizeOptionBacking == "custom")
        {
            if (_customPageSizeBacking < 1)
            {
                _customPageSizeBacking = 1;
                ToastService.ShowWarning("Page size must be at least 1. \nIt has been adjusted to 1.");
            }
            else if (_customPageSizeBacking > 100)
            {
                _customPageSizeBacking = 100;
                ToastService.ShowWarning("Page size cannot exceed 100. \nIt has been adjusted to 100.");
            }

            UpdateEffectivePageSize();
            _ = LoadBoardsAsync();
        }
    }

    private void UpdateEffectivePageSize()
    {
        if (_selectedPageSizeOptionBacking == "custom")
        {
            _pageSize = _customPageSizeBacking;
        }
        else
        {
            if (!int.TryParse(_selectedPageSizeOptionBacking, out _pageSize))
            {
                _pageSize = 10;
            }
        }
    }
}
