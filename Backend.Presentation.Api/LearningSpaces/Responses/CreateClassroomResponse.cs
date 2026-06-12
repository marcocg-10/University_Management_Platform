using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Defines an immutable Api response that wraps a single classroom,
/// and represents the output of the create classroom endpoint.
/// </summary>
/// <param name="Classroom"> Immutable object that holds the created classroom. </param>
public record class CreateClassroomResponse(ClassroomDto Classroom);
