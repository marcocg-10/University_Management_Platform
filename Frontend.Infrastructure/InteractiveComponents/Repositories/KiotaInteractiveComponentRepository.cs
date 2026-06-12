using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="InteractiveComponent"/> entities.
/// </summary>
/// <remarks>
/// This repository handles CRUD operations.
/// </remarks>
internal class KiotaInteractiveComponentRepository : IInteractiveComponentRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of <see cref="ApiClient"/>.
    /// </summary>
    /// <param name="apiClient">The Api client to make requests using Kiota.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiClient"/> is null.</exception>
    public KiotaInteractiveComponentRepository(ApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    /// <summary>
    /// Adds a new <see cref="Board"/> to the database asynchronously.
    /// </summary>
    /// <param name="board">The board to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddBoardAsync(Board board)
    {
        CreateBoardResponse? response = null;

        try
        {
            await _apiClient.InteractiveComponents.Board.PostAsync(
                new CreateBoardRequest
                {
                    Board = board.ToDto()
                });
        }
        catch (InteractiveComponentValidationErorrResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while adding the board.");
        }
        catch (InteractiveComponentConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while adding the board.");
        }
    }

    /// <summary>
    /// Retrieves all <see cref="Board"/> entities.
    /// </summary>
    /// <returns>A collection of all boards.</returns>
    public async Task<IEnumerable<Board>> ListAllBoardsAsync()
    {
        var response = await _apiClient.InteractiveComponents.Board.GetAsync().ConfigureAwait(false);

        var boards = response?.Boards?.Select(
            UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers.BoardDtoMapper.ToEntity
        ) ?? Enumerable.Empty<Board>();

        return boards;
    }

    /// <summary>
    /// Adds a new <see cref="Projector"/> to the database asynchronously.
    /// </summary>
    /// <param name="projector">The projector to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="projector"/> is null. </exception>
    public async Task AddProjectorAsync(Projector projector)
    {
        CreateProjectorResponse? response = null;

        try
        {
            await _apiClient.InteractiveComponents.Projector.PostAsync(
                new CreateProjectorRequest
                {
                    Projector = projector.ToDto()
                });
        }
        catch (InteractiveComponentValidationErorrResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while adding the projector.");
        }
        catch (InteractiveComponentConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while adding the projector.");
        }
    }

    /// <summary>
    /// Retrieves all <see cref="Projector"/> entities.
    /// </summary>
    /// <returns>
    /// A collection of all projectors.
    /// </returns>
    public async Task<IEnumerable<Projector>> ListAllProjectorsAsync()
    {
        var response = await _apiClient.InteractiveComponents.Projector.GetAsync().ConfigureAwait(false);

        var projectors = response?.Projectors?.Select(
            UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Mappers.ProjectorDtoMapper.ToEntity
        ) ?? Enumerable.Empty<Projector>();

        return projectors;
    }

    /// <summary>
    /// Deletes a <see cref="Board"/> from the database by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if the board does not exist in the database.</exception>
    /// <exception cref="Exception">Thrown if a database error occurs during deletion.</exception>
    public async Task DeleteBoardAsync(string plateId)
    {
        await _apiClient.InteractiveComponents.Board[plateId].DeleteAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the specified board asynchronously.
    /// </summary>
    /// <remarks>This method sends an update request for the specified board to the API. If the operation
    /// fails due to validation  or conflict errors, a <see cref="DomainException"/> is thrown with the relevant error
    /// message.</remarks>
    /// <param name="board">The board to update. The board must have a valid <see cref="Board.PlateId"/> value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="DomainException">Thrown if a validation error occurs or if a conflict is detected while updating the board.</exception>
    public async Task UpdateBoardAsync(Board board)
    {
        CreateBoardResponse? response = null;

        try
        {
            await _apiClient.InteractiveComponents.Board[board.PlateId.Value].PutAsync(
                new UpdateBoardRequest
                {
                    Board = board.ToDto()
                });
        }
        catch (InteractiveComponentValidationErorrResponse validationError)
        {
            throw new DomainException(validationError.ErrorMessage ?? "Validation error occurred while updating the board.");
        }
        catch (InteractiveComponentConflictErrorResponse conflictError)
        {
            throw new DomainException(conflictError.ErrorMessage ?? "A conflict occurred while updating the board.");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of boards along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the boards for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// boards.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than 0.</param>
    /// <returns>A tuple containing the boards in the requested page and pagination metadata.</returns>
    public async Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> ListBoardsPagedAsync(int pageNumber, int pageSize)
    {
        var response = await _apiClient.InteractiveComponents.Boards.GetAsync(c =>
        {
            c.QueryParameters.PageNumber = pageNumber;
            c.QueryParameters.PageSize = pageSize;
        }).ConfigureAwait(false);

        var boards = response?.Boards?.Select(BoardDtoMapper.ToEntity) ?? Enumerable.Empty<Board>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (boards, metadata);
    }

    public async Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> FilterBoardsAsync(
        string searchTerm,
        int pageNumber, 
        int pageSize)
    {
        var response = await _apiClient.InteractiveComponents.Boards.Filter.GetAsync(c =>
        {
            c.QueryParameters.PageNumber = pageNumber;
            c.QueryParameters.PageSize = pageSize;
            c.QueryParameters.SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
        }).ConfigureAwait(false);

        var boards = response?.Boards?.Select(BoardDtoMapper.ToEntity) ?? Enumerable.Empty<Board>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (boards, metadata);
    }

    public async Task<(IEnumerable<Projector> Projectors, PaginationMetadata Metadata)> FilterProjectorsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize)
    {
        var response = await _apiClient.InteractiveComponents.Projectors.Filter.GetAsync(c =>
        {
            c.QueryParameters.PageNumber = pageNumber;
            c.QueryParameters.PageSize = pageSize;
            c.QueryParameters.SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm;
        }).ConfigureAwait(false);

        var projectors = response?.Projectors?.Select(ProjectorDtoMapper.ToEntity) ?? Enumerable.Empty<Projector>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (projectors, metadata);
    }
}
