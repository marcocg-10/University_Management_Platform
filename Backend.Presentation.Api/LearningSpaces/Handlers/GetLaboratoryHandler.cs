using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for reading a laboratory by its ID.
/// </summary>
public static class GetLaboratoryHandler
{
    /// <summary>
    /// Handles the retrieval of a laboratory by its ID.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of a learning space service interface.</param>
    /// <param name="laboratoryId">The ID of the laboratory to retrieve.</param>
    /// <returns>ReadLaboratoryResponse as an asynchronous operation.</returns>
    public static async Task<Results<
        Ok<GetLaboratoryResponse>,
        NotFound<LearningSpaceNotFoundErrorResponse>>> HandleAsync(
        ILearningSpaceService learningSpaceService,
        int laboratoryId)
    {
        Laboratory? laboratory;

        try
        {
            laboratory = await learningSpaceService.ReadLaboratoryByIdAsync(laboratoryId);
        }
        catch (LearningSpaceNotFoundException exception)
        {
            return TypedResults.NotFound(
                new LearningSpaceNotFoundErrorResponse(exception.Message));
        }

        var laboratoryDto = LearningSpaceDtoMapper.ToDto(laboratory!);
        var response = new GetLaboratoryResponse(laboratoryDto);

        return TypedResults.Ok(response);
    }
}
