using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an API response that wraps a single laboratory,
/// and represents the output of the read laboratory endpoint.
/// </summary>
/// <param name="Laboratory">Immutable object that holds the retrieved laboratory.</param>
public record GetLaboratoryResponse(LaboratoryDto Laboratory);
