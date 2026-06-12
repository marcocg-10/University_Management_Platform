using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Responses;

/// <summary>
/// Represents the response returned by a filter operation on projectors,  containing the filtered list of projectors
/// and associated pagination metadata.
/// </summary>
/// <param name="Projectors">The collection of <see cref="ProjectorDto"/> objects that match the filter criteria.</param>
/// <param name="Metadata">The pagination metadata associated with the filtered results, such as total count and page information.</param>
public record FilterProjectorResponse(
    IEnumerable<ProjectorDto> Projectors,
    PaginationMetadata Metadata
);
