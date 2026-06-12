using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

/// <summary>
///  Defines an immutable Api response that wraps a collection of classrooms, and represents the 
///  output of the list classrooms endpoint.
/// </summary>
/// <param name="Classrooms"></param>
public record ListClassroomsResponse(IEnumerable<ClassroomDto> Classrooms);
