using Microsoft.AspNetCore.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Pages;

/// <summary>
/// Partial page component that manages interactive boards, including listing with pagination,
/// creation, edition and deletion. It also loads related buildings and learning spaces required
/// to associate boards.
/// </summary>
/// <remarks>
/// Responsibilities:
/// 1. Fetch paginated boards.
/// 2. Maintain UI state (modals, loading flags, pagination navigation).
/// 3. Execute CRUD operations with domain and validation error handling.
/// 4. Preload buildings and learning spaces used as associations.
/// </remarks>
public partial class InteractiveComponentsPage
{
    /// <summary>
    /// Indicates whether the Boards section is visible (controlled by a UI toggle).
    /// </summary>
    private bool showBoards = true;

    /// <summary>
    /// Indicates whether board data is currently being fetched from the backend.
    /// Used to show a loading indicator during service calls.
    /// </summary>
    private bool _isLoadingBoards = true;

    /// <summary>
    /// Indicates whether learning space data is currently being fetched from the backend.
    /// Used to show a loading indicator during service calls.
    /// </summary>
    private bool _isLoadingLearningSpaces = true;

    /// <summary>
    /// Indicates whether buildings data is currently being fetched from the backend.
    /// Used to show a loading indicator during service calls.
    /// </summary>
    private bool _isLoadingBuildings = true;

    /// <summary>
    /// Indicates whether buildings and learning space data is currently being fetched from the backend.
    /// Used to show a loading indicator during service calls.
    /// </summary>
    private bool _isLoadingBuildingsAndLearningSpaces = true;

    /// <summary>
    /// Contains the list of <see cref="Board"/> entities retrieved from the service.
    /// </summary>
    private IEnumerable<Board> _boards = [];

    /// <summary>
    /// List of available buildings for interactive component placement.
    /// </summary>
    private IEnumerable<Building> _availableBuildings = Enumerable.Empty<Building>();

    /// <summary>
    /// List of available learning spaces for interactive component placement.
    /// </summary>
    private IEnumerable<LearningSpace> _availableLearningSpaces = Enumerable.Empty<LearningSpace>();

    /// <summary>
    /// Determines whether the "Create Board" modal dialog is open.
    /// </summary>
    private bool showCreateModal = false;

    /// <summary>
    /// Backing model bound to the board creation form.
    /// Holds input values for all board properties.
    /// </summary>
    private BoardForm form = new();

    /// <summary>
    /// Stores error messages related to board data fetching or creation.
    /// Primarily used for internal debugging.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Boolean flag indicating whether the delete confirmation dialog is shown.
    /// </summary>
    private bool _showDelete;

    /// <summary>
    /// String storing the identifier of the board pending deletion.
    /// </summary>
    private string? _pendingDeleteId;

    /// <summary>
    /// Indicates whether the edit modal is currently visible.
    /// </summary>
    private bool _showEdit = false;

    /// <summary>
    /// Backing model for the edit form.
    /// </summary>
    private BoardForm _editForm = new();

    /// <summary>
    /// Current pagination metadata.
    /// </summary>
    private PaginationMetadata? _pagination;

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    private int CurrentPage => _pagination?.CurrentPage ?? 1;

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    private int TotalPages => _pagination?.TotalPages ?? 0;

    /// <summary>
    /// Indicates whether a previous page is available.
    /// </summary>
    private bool CanPrev => _pagination?.HasPrevious == true;

    /// <summary>
    /// Indicates whether a next page is available.
    /// </summary>
    private bool CanNext => _pagination?.HasNext == true;

    /// <summary>
    /// Current search term used for filtering boards.
    /// </summary>
    private string _searchTerm = string.Empty;

    /// Sequence of page numbers for pagination controls, using a windowed approach.
    /// Shows first, last, and a window around the current page.
    /// </summary>
    private IEnumerable<int> PageNumbers
    {
        get
        {
            // Window size: show current page ±2, plus first and last page
            const int window = 2;
            int totalPages = Math.Max(TotalPages, 1);
            int current = CurrentPage;
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

    /// <summary>
    /// Reference to the create modal component for error mapping.
    /// </summary>
    private BoardCreateModal? _createBoardModalRef;

    /// <summary>
    /// Reference to the update modal component for error mapping.
    /// </summary>
    private BoardUpdateModal? _updateBoardModalRef;

    /// <summary>
    /// Asynchronously retrieves all boards from the backend service
    /// and updates the local state accordingly.
    /// </summary>
    private async Task LoadBoardsAsync()
    {
        _isLoadingBoards = true;
        _errorMessage = null;
        StateHasChanged();

        try
        {
            await LoadBoardsPageAsync(1);
            if (!_availableBuildings.Any()) await LoadBuildingsAsync();
            if (!_availableLearningSpaces.Any()) await LoadLearningSpacesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the boards.", ex);
        }
        finally
        {
            _isLoadingBoards = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Loads a specific page of boards applying pagination.
    /// </summary>
    /// <param name="page">Page number to load (1-based).</param>
    private async Task LoadBoardsPageAsync(int page)
    {
        if (page < 1) page = 1;
        _isLoadingBoards = true;
        _errorMessage = null;
        StateHasChanged();

        try
        {
            (IEnumerable<Board> Boards, PaginationMetadata Metadata) result;

            result = !string.IsNullOrWhiteSpace(_searchTerm)
                ? await InteractiveComponentService.FilterBoardsAsync(_searchTerm, page, _pageSize)
                : await InteractiveComponentService.ListBoardsPagedAsync(page, _pageSize);

            _boards = result.Boards ?? Enumerable.Empty<Board>();
            _pagination = result.Metadata ?? null;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the boards page.", ex);
        }
        finally
        {
            _isLoadingBoards = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Advances to the next page if available.
    /// </summary>
    private Task NextPageAsync() => CanNext ? LoadBoardsPageAsync(CurrentPage + 1) : Task.CompletedTask;

    /// <summary>
    /// Goes back to the previous page if available.
    /// </summary>
    private Task PrevPageAsync() => CanPrev ? LoadBoardsPageAsync(CurrentPage - 1) : Task.CompletedTask;

    /// <summary>
    /// Navigates to a specific page via button.
    /// </summary>
    private Task GoToPage(int page)
    {
        var max = Math.Max(TotalPages, 1);
        if (page < 1) page = 1;
        if (page > max) page = max;

        return LoadBoardsPageAsync(page);
    }

    /// <summary>
    /// Opens the create board modal.
    /// </summary>
    private Task OpenCreate()
    {
        showCreateModal = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Closes the create board modal.
    /// </summary>
    private void CloseCreate() => showCreateModal = false;

    /// <summary>
    /// Handles the creation of a new board by sending form data
    /// to the backend service via <c>IInteractiveComponentService</c>.
    /// </summary>
    private async Task CreateBoard()
    {
        bool success = false;

        try
        {
            await InteractiveComponentService.CreateBoardAsync(
                colorValue: form.Color,
                markerColorValue: form.MarkerColor,
                texture: form.Texture,
                plateIdValue: form.PlateId,
                x: (double)form.X, y: (double)form.Y, z: (double)form.Z,
                width: (double)form.Width, height: (double)form.Height, depth: (double)form.Depth,
                XAxisRotation: (double)form.XAxisRotation, YAxisRotation: (double)form.YAxisRotation, ZAxisRotation: (double)form.ZAxisRotation,
                learningSpaceId: (int)form.RoomId
            );

            await LoadBoardsPageAsync(CurrentPage);
            ToastService.ShowSuccess("Board created successfully!");
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
            throw new Exception("An unexpected error occurred while creating the board.", ex);
        }
        finally
        {
            if (success)
            {
                form = new();
                showCreateModal = false;
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// Shows the delete confirmation for a given board.
    /// </summary>
    /// <param name="id">Plate identifier of the board to delete.</param>
    private void AskDeleteBoard(string id)
    {
        _pendingDeleteId = id;
        _showDelete = true;
    }

    /// <summary>
    /// Confirms and performs deletion of the pending board.
    /// </summary>
    private async Task ConfirmDeleteBoard()
    {
        try
        {
            await InteractiveComponentService.DeleteBoardAsync(_pendingDeleteId);
            await LoadBoardsPageAsync(CurrentPage);
            // After reloading, check if the current page is empty and not the first page
            if (!_boards.Any()  && CurrentPage > 1)
            {
                await LoadBoardsPageAsync(CurrentPage - 1);
            }
            _pendingDeleteId = null;
            _showDelete = false;
            ToastService.ShowSuccess("Board deleted successfully.");
            StateHasChanged();
        }
        catch (DomainException ex)
        {
            ToastService.ShowError(ex.Message);
        }
        catch (ArgumentException ex)
        {
            ToastService.ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the board.", ex);
        }
    }

    /// <summary>
    /// Cancels the pending deletion.
    /// </summary>
    private Task CancelDeleteBoard()
    {
        _pendingDeleteId = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Opens the edit modal and populates the edit form with a board's current data.
    /// </summary>
    /// <param name="plateId">Plate identifier of the board to edit.</param>
    private Task OpenEditBoard(string plateId)
    {
        var board = _boards.FirstOrDefault(b => b.PlateId.Value == plateId);
        if (board is null)
        {
            ToastService.ShowError($"Board '{plateId}' not found.");
            return Task.CompletedTask;
        }

        _editForm = new BoardForm
        {
            PlateId = board.PlateId.Value,
            RoomId = board.LearningSpaceId,
            Color = board.Color.Value,
            Texture = board.Texture,
            MarkerColor = board.MarkerColor.Value,
            Width = board.Dimensions.Width,
            Height = board.Dimensions.Height,
            Depth = board.Dimensions.Depth,
            X = board.Coordinates.X,
            Y = board.Coordinates.Y,
            Z = board.Coordinates.Z,
            XAxisRotation = board.Rotations.XAxisRotation,
            YAxisRotation = board.Rotations.YAxisRotation,
            ZAxisRotation = board.Rotations.ZAxisRotation
        };

        _showEdit = true;
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Closes the edit modal.
    /// </summary>
    private Task CloseEditBoard()
    {
        _showEdit = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Persists board changes and refreshes the current page.
    /// </summary>
    private async Task ConfirmEditBoard()
    {
        bool success = false;

        try
        {
            var updatedBoard = new Board(
                new Color(_editForm.Color),
                new Color(_editForm.MarkerColor),
                _editForm.Texture,
                new PlateId(_editForm.PlateId),
                new Coordinates((double)_editForm.X, (double)_editForm.Y, (double)_editForm.Z),
                new Dimensions((double)_editForm.Width, (double)_editForm.Height, (double)_editForm.Depth),
                new Rotations((double)_editForm.XAxisRotation, (double)_editForm.YAxisRotation, (double)_editForm.ZAxisRotation),
                (int)_editForm.RoomId
            );

            await InteractiveComponentService.UpdateBoardAsync(updatedBoard);
            await LoadBoardsPageAsync(CurrentPage);
            ToastService.ShowSuccess("Board updated successfully!");
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
            throw new Exception("An unexpected error occurred while updating the board.", ex);
        }
        finally
        {
            if (success)
            {
                _showEdit = false;
            }

            StateHasChanged();
        }
    }

    /// <summary>
    /// Service used to retrieve learning spaces.
    /// </summary>
    [Inject] ILearningSpaceService LearningSpaceService { get; set; } = default!;

    /// <summary>
    /// Loads available learning spaces (laboratories and classrooms) concurrently.
    /// </summary>
    private async Task LoadLearningSpacesAsync()
    {
        _isLoadingLearningSpaces = true;
        _errorMessage = null;
        StateHasChanged();

        try
        {
            var laboratoriesTask = LearningSpaceService.ListLaboratoriesAsync();
            var classroomsTask = LearningSpaceService.ListClassroomsAsync();
            await Task.WhenAll(laboratoriesTask, classroomsTask);

            _availableLearningSpaces = laboratoriesTask.Result.Cast<LearningSpace>()
                .Concat(classroomsTask.Result.Cast<LearningSpace>());
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the learning spaces.", ex);
        }
        finally
        {
            _isLoadingLearningSpaces = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Service used to retrieve buildings.
    /// </summary>
    [Inject] IBuildingService BuildingService { get; set; } = default!;

    /// <summary>
    /// Loads available buildings.
    /// </summary>
    private async Task LoadBuildingsAsync()
    {
        _isLoadingBuildings = true;
        _errorMessage = null;
        StateHasChanged();

        try
        {
            var result = await BuildingService.GetBuildingsAsync();
            _availableBuildings = result ?? Enumerable.Empty<Building>();
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while loading the buildings.", ex);
        }
        finally
        {
            _isLoadingBuildings = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Translates backend error messages to specific form field errors
    /// </summary>
    /// <param name="errorMessage"> The error message received from the backend. </param>
    private void MapBackendError(string errorMessage)
    {
        if (_createBoardModalRef is null)
        {
            ToastService.ShowError(errorMessage);
            return;
        }

        if (_updateBoardModalRef is null)
        {
            ToastService.ShowError(errorMessage);
            return;
        }

        if (_createProjectorModalRef is null)
        {
            ToastService.ShowError(errorMessage);
            return;
        }

        if (errorMessage.Contains("Plate", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.PlateId), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.PlateId), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.PlateId), errorMessage);
            return;
        }

        if (errorMessage.Contains("Resolution width", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.ResWidth), errorMessage);
            return;
        }

        if (errorMessage.Contains("Resolution Height", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.ResHeight), errorMessage);
            return;
        }

        if (errorMessage.Contains("Width", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.Width), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.Width), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Width), errorMessage);
            return;
        }

        if (errorMessage.Contains("Height", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.Height), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.Height), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Height), errorMessage);
            return;
        }

        if (errorMessage.Contains("Depth", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.Depth), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.Depth), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Depth), errorMessage);
            return;
        }

        if (errorMessage.Contains("X coordinate", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.X), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.X), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.X), errorMessage);
            return;
        }

        if (errorMessage.Contains("Y coordinate", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.Y), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.Y), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Y), errorMessage);
            return;
        }

        if (errorMessage.Contains("Z coordinate", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.Z), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.Z), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Z), errorMessage);
            return;
        }

        if (errorMessage.Contains("X Axis", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.XAxisRotation), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.XAxisRotation), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.XAxisRotation), errorMessage);
            return;
        }

        if (errorMessage.Contains("Y Axis", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.YAxisRotation), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.YAxisRotation), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.YAxisRotation), errorMessage);
            return;
        }

        if (errorMessage.Contains("Z Axis", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateModal)
                _createBoardModalRef.SetFieldError(nameof(BoardForm.ZAxisRotation), errorMessage);
            else if (_showEdit)
                _updateBoardModalRef.SetFieldError(nameof(BoardForm.ZAxisRotation), errorMessage);
            else if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.ZAxisRotation), errorMessage);
            return;
        }

        if (errorMessage.Contains("Brightness", StringComparison.OrdinalIgnoreCase))
        {
            if (showCreateProjectorModal)
                _createProjectorModalRef.SetFieldError(nameof(ProjectorForm.Brightness), errorMessage);
            return;
        }

        ToastService.ShowError(errorMessage);
    }

    /// <summary>
    /// Reacts to typing in the search bar and reloads page 1.
    /// Clearing the search term reverts to non-filtered pagination.
    /// </summary>
    private async Task OnSearchChanged()
    {
        await LoadBoardsPageAsync(1);
    }
}
