using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
///  Defines an immutable Api response that wraps a collection of laboratories, and represents the 
///  output of the list laboratories endpoint.
/// </summary>
/// <param name="Laboratories"></param>
public record ListLaboratoriesResponse(IEnumerable<LaboratoryDto> Laboratories);
