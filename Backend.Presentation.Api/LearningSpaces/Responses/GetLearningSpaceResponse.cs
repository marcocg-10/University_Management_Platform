using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an API response that wraps a single learning space,
/// and represents the output of the read learning space endpoint.
/// </summary>
/// <param name="LearningSpace">Immutable object that holds the retrieved learning space.</param>
public record GetLearningSpaceResponse(LearningSpaceDto LearningSpace);