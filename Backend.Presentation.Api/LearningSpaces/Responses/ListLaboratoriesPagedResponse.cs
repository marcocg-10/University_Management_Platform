using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Represents a paged response containing a collection of laboratories and associated pagination metadata.
/// </summary>
/// <param name="Laboratories">The collection of laboratories included in the current page of the response.</param>
/// <param name="Metadata">The pagination metadata providing details about the current page, total items, and other pagination-related
/// information.</param>
public record ListLaboratoriesPagedResponse(
    IEnumerable<LaboratoryDto> Laboratories,
    PaginationMetadata Metadata
);
