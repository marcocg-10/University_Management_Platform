using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handles the deletion of an existing classroom learning space.
/// </summary>
public static class DeleteClassroomHandler
{
    /// <summary>
    /// Handles the DELETE /classrooms/{classroomId} request.
    /// </summary>
    /// <param name="classroomId">The unique identifier of the classroom to delete.</param>
    /// <param name="learningSpaceService">The learning space service instance.</param>
    /// <returns>An <see cref="Results"/> indicating the outcome of the operation.</returns>
    public static async Task<Results<
        Ok<DeleteLearningSpaceResponse>,
        UnauthorizedHttpResult,
        NotFound<LearningSpaceNotFoundErrorResponse>,
        Conflict<LearningSpaceConflictErrorResponse>>> HandleAsync(
            [FromServices] ILearningSpaceService learningSpaceService,
            int classroomId)
    {
        try
        {
            await learningSpaceService.DeleteClassroomAsync(classroomId);
            return TypedResults.Ok(new DeleteLearningSpaceResponse("The learning space was deleted successfully"));
        }
        catch (LearningSpaceNotFoundException exception)
        {
            return TypedResults.NotFound(
                new LearningSpaceNotFoundErrorResponse(exception.Message));
        }
        catch (ForeignKeyException ex)
        {
            return TypedResults.Conflict(
                new LearningSpaceConflictErrorResponse(ex.Message));
        }
    }
}
