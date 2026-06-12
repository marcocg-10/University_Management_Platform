using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for getting a classroom by id.
/// </summary>
public static class GetClassroomHandler
{
    /// <summary>
    /// Handles the retrieval of a classroom by its ID.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="classroomId">The ID of the classroom to retrieve.</param>
    /// <returns>ReadClassroomResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<GetClassroomResponse>,
        NotFound<LearningSpaceNotFoundErrorResponse>>> HandleAsync(
        ILearningSpaceService learningSpaceService, 
        int classroomId)
    {
        Classroom? classroom;

        try
        {
            classroom = await learningSpaceService.ReadClassroomByIdAsync(classroomId);
        }
        catch (LearningSpaceNotFoundException exception)
        {
            return TypedResults.NotFound(
                new LearningSpaceNotFoundErrorResponse(exception.Message));
        }

        var classroomDto = LearningSpaceDtoMapper.ToDto(classroom!);
        var response = new GetClassroomResponse(classroomDto);

        return TypedResults.Ok(response);
    }
}
