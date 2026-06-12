using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

/// <summary>
/// Handles the filtering of projectors based on a search term and pagination parameters.
/// </summary>
/// <remarks>This method retrieves a paginated list of projectors that match the specified search term. The results
/// include metadata about the pagination, such as the total number of pages and items.</remarks>
public static class FilterProjectorHandler
{
    private const int MaxPageSize = 100;
    /// <summary>
    /// Handles the filtering of projectors based on the specified search term, page number, and page size.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. If null or whitespace, no filtering is applied.</param>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of items per page. Must be between 1 and the maximum allowed page size.</param>
    /// <param name="interactiveComponentService">The service used to perform the projector filtering operation. Cannot be null.</param>
    /// <returns>A <see cref="FilterProjectorResponse"/> containing the filtered projectors and pagination metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentService"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1, or if <paramref name="pageSize"/> is less than 1 or
    /// greater than the maximum allowed page size.</exception>
    public static async Task<FilterProjectorResponse> HandleAsync(
        string searchTerm,
        int pageNumber,
        int pageSize,
        IInteractiveComponentService interactiveComponentService)
    {
        if (interactiveComponentService is null)
            throw new ArgumentNullException(nameof(interactiveComponentService));
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
        if (pageSize < 1 || pageSize > MaxPageSize)
            throw new ArgumentOutOfRangeException(nameof(pageSize), $"Page size must be between 1 and {MaxPageSize}.");
        var normalizedTerm = string.IsNullOrWhiteSpace(searchTerm) ? string.Empty : searchTerm.Trim();
        var (projectors, totalCount) = await interactiveComponentService
            .FilterProjectorsAsync(normalizedTerm, pageNumber, pageSize)
            .ConfigureAwait(false);
        var projectorDtos = (projectors ?? Enumerable.Empty<Domain.InteractiveComponents.Entities.Projector>())
            .Select(ProjectorDtoMapper.ToDto)
            .ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var paginationMetadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );
        return new FilterProjectorResponse(projectorDtos, paginationMetadata);
    }
}
