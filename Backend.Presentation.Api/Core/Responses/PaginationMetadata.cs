namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;

/// <summary>
/// Contains metadata about paginated results.
/// </summary>
/// <param name="CurrentPage">The current page number</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="TotalCount">The total number of items across all pages.</param>
/// <param name="TotalPages">The total number of pages.</param>
public record PaginationMetadata(
    int CurrentPage,
    int PageSize,
    int TotalCount,
    int TotalPages
);
