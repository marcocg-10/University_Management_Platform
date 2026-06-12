using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents a response containing a filtered collection of boards and associated pagination metadata.
/// </summary>
/// <param name="Boards">The collection of boards that fit the search term</param>
/// <param name="Metadata">The pagination metadata providing details about the current page, total items, and other pagination-related
/// information.</param>
public record FilterBoardsResponse (
    IEnumerable<BoardDto> Boards,
    PaginationMetadata Metadata
);
