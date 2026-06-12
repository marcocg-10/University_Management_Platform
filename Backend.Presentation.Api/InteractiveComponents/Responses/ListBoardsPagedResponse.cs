
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents a paged response containing a collection of boards and associated pagination metadata.
/// </summary>
/// <param name="Boards">The collection of boards included in the current page of the response.</param>
/// <param name="Metadata">The pagination metadata providing details about the current page, total items, and other pagination-related
/// information.</param>
public record ListBoardsPagedResponse(
    IEnumerable<BoardDto> Boards,
    PaginationMetadata Metadata
);