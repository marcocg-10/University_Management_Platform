using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
/// Response returned by get classroom by id endpoint.
/// </summary>
/// <param name="Classroom">The classroom DTO.</param>
public record GetClassroomResponse(ClassroomDto Classroom);
