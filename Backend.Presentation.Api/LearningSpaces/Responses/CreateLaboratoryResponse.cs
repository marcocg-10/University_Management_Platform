using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an immutable Api response that wraps a single laboratory,
/// and represents the output of the create laboratory endpoint.
/// </summary>
/// <param name="Laboratory"> Immutable object that holds the created laboratory. </param>
public record CreateLaboratoryResponse(LaboratoryDto Laboratory);
