using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Represents the response containing a collection of learning spaces filtered by building ID.
/// </summary>
/// <param name="LearningSpaces">Collection of learning space DTOs associated with the specified building.</param>
public record ListLearningSpacesByBuildingIdResponse(IEnumerable<LearningSpaceDto> LearningSpaces);
