using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for listing laboratories.
/// </summary>
public static class ListLaboratoriesHandler
{
    /// <summary>
    /// Handles the listing of laboratories.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of learning space service interface.</param>
    /// <returns>ListLaboratoriesResponse as an asynchronous operation.</returns>
    public static async Task<ListLaboratoriesResponse> HandleAsync(
        ILearningSpaceService learningSpaceService)
    {
        var laboratories = await learningSpaceService.ListLaboratoriesAsync();

        return new ListLaboratoriesResponse(
            laboratories.Select(LearningSpaceDtoMapper.ToDto));
    }
}