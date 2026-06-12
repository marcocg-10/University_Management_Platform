using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Pages;

/// <summary>
/// Partial definition of <see cref="InteractiveComponentsPage"/> dedicated to
/// managing <strong>Projector</strong>-related logic and UI state.
/// </summary>
public partial class InteractiveComponentsPage
{
    /// <summary>
    /// Determines whether the Projectors section is currently visible on the page.
    /// Bound to a checkbox toggle in the UI.
    /// </summary>
    private bool showProjectors = true;

    /// <summary>
    /// Indicates whether Projector data is currently being loaded from the backend.
    /// Used to control loading indicators in the UI.
    /// </summary>
    private bool _isLoadingProjectors = true;

    /// <summary>
    /// Holds the collection of <see cref="Projector"/> entities retrieved from the service.
    /// If the list is empty, a fallback message ("No projectors found.") is shown.
    /// </summary>
    private IEnumerable<Projector> _projectors = [];

    /// <summary>
    /// Determines whether the "Create Projector" modal dialog is currently open.
    /// </summary>
    private bool showCreateProjectorModal = false;

    /// <summary>
    /// Backing model bound to the projector creation form.
    /// Holds input values for all projector properties.
    /// </summary>
    private ProjectorForm projectorForm = new();

    /// <summary>
    /// Stores error messages related to Projector data fetching or processing.
    /// Primarily used for developer logging and diagnostic output.
    /// </summary>
    private string? _errorMessageProjector;

    /// <summary>
    /// Reference to the create modal component for error mapping.
    /// </summary>
    private ProjectorCreateModal? _createProjectorModalRef;

    /// <summary>
    /// Current pagination metadata for projectors.
    /// </summary>
    private PaginationMetadata? _paginationProjector;

    /// <summary>
    /// Current page number (1-based) for projectors.
    /// </summary>
    private int CurrentPageProjector => _paginationProjector?.CurrentPage ?? 1;

    /// <summary>
    /// Total number of pages available for projectors.
    /// </summary>
    private int TotalPagesProjector => _paginationProjector?.TotalPages ?? 0;

    /// <summary>
    /// Indicates whether a previous page is available for projectors.
    /// </summary>
    private bool CanPrevProjector => _paginationProjector?.HasPrevious == true;

    /// <summary>
    /// Indicates whether a next page is available for projectors.
    /// </summary>
    private bool CanNextProjector => _paginationProjector?.HasNext == true;

    /// <summary>
    /// Current search term used for filtering projectors.
    /// </summary>
    private string _searchTermProjector = string.Empty;

    /// <summary>
    /// Sequence of page numbers for projector pagination controls, using a windowed approach.
    /// Shows first, last, and a window around the current page.
    /// </summary>
    private IEnumerable<int> PageNumbersProjector
    {
        get
        {
            // Window size: show current page ±2, plus first and last page
            const int window = 2;
            int totalPages = Math.Max(TotalPagesProjector, 1);
            int current = CurrentPageProjector;
            var pages = new List<int>();
            if (totalPages <= 7)
            {
                // If few pages, show all
                pages.AddRange(Enumerable.Range(1, totalPages));
            }
            else
            {
                // Always show first page
                pages.Add(1);
                // Determine window start/end
                int start = Math.Max(2, current - window);
                int end = Math.Min(totalPages - 1, current + window);
                // Add windowed pages
                for (int i = start; i <= end; i++)
                {
                    pages.Add(i);
                }
                // Always show last page
                pages.Add(totalPages);
            }
            return pages;
        }
    }

    private int _pageSizeProjector = 10;

    private string _selectedPageSizeOptionProjectorBacking = "10";
    private int _customPageSizeProjectorBacking = 10;

    private string _selectedPageSizeOptionProjector
    {
        get => _selectedPageSizeOptionProjectorBacking;
        set
        {
            if (_selectedPageSizeOptionProjectorBacking == value) return;
            _selectedPageSizeOptionProjectorBacking = value;

            UpdateEffectivePageSizeProjector();
            _ = LoadProjectorsAsync();
        }
    }

    private int _customPageSizeProjector
    {
        get => _customPageSizeProjectorBacking;
        set
        {
            _customPageSizeProjectorBacking = value;
        }
    }

    private void ApplyCustomPageSizeProjector()
    {
        if (_selectedPageSizeOptionProjectorBacking == "custom")
        {
            if (_customPageSizeProjectorBacking < 1)
            {
                _customPageSizeProjectorBacking = 1;
                ToastService.ShowWarning("Page size must be at least 1. \nIt has been adjusted to 1.");
            }
            else if (_customPageSizeProjectorBacking > 100)
            {
                _customPageSizeProjectorBacking = 100;
                ToastService.ShowWarning("Page size cannot exceed 100. \nIt has been adjusted to 100.");
            }

            UpdateEffectivePageSizeProjector();
            _ = LoadProjectorsAsync();
        }
    }

    private void UpdateEffectivePageSizeProjector()
    {
        if (_selectedPageSizeOptionProjectorBacking == "custom")
        {
            _pageSizeProjector = _customPageSizeProjectorBacking;
        }
        else
        {
            if (!int.TryParse(_selectedPageSizeOptionProjectorBacking, out _pageSizeProjector))
            {
                _pageSizeProjector = 10;
            }
        }
    }

    /// <summary>
    /// Asynchronously retrieves all Projectors from the backend service
    /// and updates the local state accordingly.
    /// </summary>
    private async Task LoadProjectorsAsync()
    {
        _isLoadingProjectors = true;
        _errorMessageProjector = null;
        StateHasChanged();

        try
        {
            await LoadProjectorsPageAsync(1);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the projectors.", ex);
        }
        finally
        {
            _isLoadingProjectors = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Loads a specific page of projectors applying pagination.
    /// </summary>
    /// <param name="page">Page number to load (1-based).</param>
    private async Task LoadProjectorsPageAsync(int page)
    {
        if (page < 1) page = 1;
        _isLoadingProjectors = true;
        _errorMessageProjector = null;
        StateHasChanged();

        try
        {
            (IEnumerable<Projector> Projectors, PaginationMetadata Metadata) result;

            result = !string.IsNullOrWhiteSpace(_searchTermProjector)
                ? await InteractiveComponentService.FilterProjectorsAsync(_searchTermProjector, page, _pageSizeProjector)
                : await InteractiveComponentService.FilterProjectorsAsync(string.Empty, page, _pageSizeProjector);

            _projectors = result.Projectors ?? Enumerable.Empty<Projector>();
            _paginationProjector = result.Metadata ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the projectors page.", ex);
        }
        finally
        {
            _isLoadingProjectors = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Advances to the next page if available.
    /// </summary>
    private Task NextPageProjectorAsync() => CanNextProjector ? LoadProjectorsPageAsync(CurrentPageProjector + 1) : Task.CompletedTask;

    /// <summary>
    /// Goes back to the previous page if available.
    /// </summary>
    private Task PrevPageProjectorAsync() => CanPrevProjector ? LoadProjectorsPageAsync(CurrentPageProjector - 1) : Task.CompletedTask;

    /// <summary>
    /// Navigates to a specific page via button.
    /// </summary>
    private Task GoToPageProjector(int page)
    {
        var max = Math.Max(TotalPagesProjector, 1);
        if (page < 1) page = 1;
        if (page > max) page = max;

        return LoadProjectorsPageAsync(page);
    }

    ///<summary>
    /// Opens the "Create Projector" modal dialog.
    /// </summary>
    private async Task OpenCreateProjector()
    {
        showCreateProjectorModal = true;
    }

    /// <summary>
    /// Closes the "Create Projector" modal dialog.
    /// </summary>
    private void CloseCreateProjector()
        => showCreateProjectorModal = false;

    /// <summary>
    /// Handles the creation of a new Projector by sending form data
    /// to the backend service via <c>InteractiveComponentService</c>.
    /// </summary>
    /// <remarks>
    /// Validates data on the backend through value object exceptions such as:
    /// <see cref="InvalidResolutionException"/>, <see cref="InvalidDimensionsException"/>,
    /// <see cref="InvalidCoordinatesException"/>, and <see cref="InvalidPlateIdException"/>.
    /// If creation succeeds, the projector list is refreshed and a success toast is shown.
    /// </remarks>
    private async Task CreateProjector()
    {
        bool success = false;
        try
        {
            await InteractiveComponentService.CreateProjectorAsync(
                colorValue: projectorForm.Color,
                texture: projectorForm.Texture,
                brightness: (int)projectorForm.Brightness,
                plateId: projectorForm.PlateId,
                resWidth: (int)projectorForm.ResWidth,
                resHeight: (int)projectorForm.ResHeight,
                x: (double)projectorForm.X, y: (double)projectorForm.Y, z: (double)projectorForm.Z,
                width: (double)projectorForm.Width, height: (double)projectorForm.Height, depth: (double)projectorForm.Depth,
                XAxisRotation: (double)projectorForm.XAxisRotation, YAxisRotation: (double)projectorForm.YAxisRotation, ZAxisRotation: (double)projectorForm.ZAxisRotation,
                learningSpaceId: (int)projectorForm.RoomId
            );

            await LoadProjectorsPageAsync(CurrentPageProjector);
            ToastService.ShowSuccess("Projector created successfully!");
            success = true;
        }
        catch (ValidationException ex)
        {
            MapBackendError(ex.Message);
        }
        catch (ArgumentException ex)
        {
            MapBackendError(ex.Message);
        }
        catch (DomainException ex)
        {
            MapBackendError(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while creating the projector.", ex);
        }
        finally
        {
            if (success)
            {
                // Reset form and close modal on success
                projectorForm = new();
                showCreateProjectorModal = false;
            }
            StateHasChanged();
        }
    }

    /// <summary>
    /// Reacts to typing in the search bar and reloads page 1 for projectors.
    /// Clearing the search term reverts to non-filtered pagination.
    /// </summary>
    private async Task OnSearchProjectorChanged()
    {
        await LoadProjectorsPageAsync(1);
    }
}
