using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Handlers;

/// <summary>
/// Handler for listing learning spaces by building ID.
/// </summary>
public static class ListLearningSpaceByBuildingIdHandler
{
    /// <summary>
    /// Handles the listing of learning spaces filtered by building ID.
    /// </summary>
    /// <param name="learningSpaceService">Implementation of learning space service interface.</param>
    /// <param name="buildingId">The ID of the building to filter learning spaces by.</param>
    /// <returns>ListLearningSpacesByBuildingIdResponse as an asynchronous operation.</returns>
    public static async Task<ListLearningSpacesByBuildingIdResponse> HandleAsync(
        ILearningSpaceService learningSpaceService,
        int buildingId)
    {
        var learningSpaces = await learningSpaceService.ListLearningSpacesByBuildingIdAsync(buildingId);

        return new ListLearningSpacesByBuildingIdResponse(
            learningSpaces.Select(LearningSpaceDtoMapper.ToDto));
    }
}