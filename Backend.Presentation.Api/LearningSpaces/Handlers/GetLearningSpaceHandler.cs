using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for reading a learning space by its ID.
/// This handler retrieves any type of learning space (Laboratory, Classroom, etc.).
/// </summary>
public static class GetLearningSpaceHandler
{
    /// <summary>
    /// Handles the retrieval of a learning space by its ID.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="learningSpaceId">The ID of the learning space to retrieve.</param>
    /// <returns>GetLearningSpaceResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<GetLearningSpaceResponse>,
        NotFound<LearningSpaceNotFoundErrorResponse>>> HandleAsync(
        ILearningSpaceService learningSpaceService,
        int learningSpaceId)
    {
        LearningSpace? learningSpace;

        try
        {
            learningSpace = await learningSpaceService.ReadLearningSpaceByIdAsync(learningSpaceId);
        }
        catch (LearningSpaceNotFoundException exception)
        {
            return TypedResults.NotFound(
                new LearningSpaceNotFoundErrorResponse(exception.Message));
        }

        var learningSpaceDto = LearningSpaceDtoMapper.ToDto(learningSpace!);
        var response = new GetLearningSpaceResponse(learningSpaceDto);

        return TypedResults.Ok(response);
    }
}
