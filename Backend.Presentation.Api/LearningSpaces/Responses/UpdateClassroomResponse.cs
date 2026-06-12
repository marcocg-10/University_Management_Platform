using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an immutable Api response that wraps a single classroom,
/// and represents the output of the update classroom endpoint.
/// </summary>
/// <param name="Classroom">Immutable object that holds the updated classroom.</param>
public record UpdateClassroomResponse(ClassroomDto Classroom);