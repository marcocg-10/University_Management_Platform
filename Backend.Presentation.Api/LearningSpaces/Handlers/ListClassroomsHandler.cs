using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for listing classrooms.
/// </summary>
public static class ListClassroomsHandler
{
    /// <summary>
    /// Handles the listing of classrooms.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of learning space service interface.</param>
    /// <returns>ListClassroomsResponse as an asynchronous operation.</returns>
    public static async Task<ListClassroomsResponse> HandleAsync(
        ILearningSpaceService learningSpaceService)
    {
    var classrooms = await learningSpaceService.ListClassroomsAsync();

    return new ListClassroomsResponse(
        classrooms.Select(LearningSpaceDtoMapper.ToDto));
    }
}
