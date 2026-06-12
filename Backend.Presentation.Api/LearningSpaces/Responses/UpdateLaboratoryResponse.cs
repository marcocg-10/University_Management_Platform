using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an immutable Api response that wraps a single laboratory,
/// and represents the output of the update laboratory endpoint.
/// </summary>
/// <param name="Laboratory">Immutable object that holds the updated laboratory.</param>
public record UpdateLaboratoryResponse(LaboratoryDto Laboratory);