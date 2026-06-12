namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;

/// <summary>
/// Contains pagination metadata for paginated responses.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Gets the current page number (starting from 1).
    /// </summary>
    public int CurrentPage { get; init; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    public int TotalPages { get; init; }

    /// <summary>
    /// Gets whether there is a previous page.
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether there are additional pages available after the current page.
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;
}
