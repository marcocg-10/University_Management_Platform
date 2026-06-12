using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles the retrieval of a paginated list of boards.
/// </summary>
/// <remarks>This method retrieves a specific page of boards from the data source, along with metadata about the
/// pagination state. The boards are mapped to DTOs before being returned in the response.</remarks>
public static class ListBoardsPagedHandler
{
    /// <summary>
    /// Retrieves a paginated list of boards and associated pagination metadata.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be 1 or greater.</param>
    /// <param name="pageSize">The number of items per page. Must be between 1 and 100, inclusive.</param>
    /// <param name="interactiveComponentService">The service used to retrieve the boards. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="ListBoardsPagedResponse"/> containing the list of boards for the specified page  and the associated
    /// pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentService"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1, or if <paramref name="pageSize"/> is not between 1 and
    /// 100.</exception>
    public static async Task<ListBoardsPagedResponse> HandleAsync(
        int pageNumber,
        int pageSize,
        IInteractiveComponentService interactiveComponentService)
    {
        if (interactiveComponentService is null)
        {
            throw new ArgumentNullException(nameof(interactiveComponentService));
        }

        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
        }

        if (pageSize < 1 || pageSize > 100) // Max 100 items per page
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
        }

        var (boards, totalCount) = await interactiveComponentService
            .ListBoardsPagedAsync(pageNumber, pageSize)
            .ConfigureAwait(false);

        var boardDtos = boards
            .Select(BoardDtoMapper.ToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return new ListBoardsPagedResponse(boardDtos, paginationMetadata);
    }
}
