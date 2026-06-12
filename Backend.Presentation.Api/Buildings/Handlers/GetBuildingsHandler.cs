using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;

/// <summary>
/// Handler for retrieving buildings.
/// </summary>
public static class GetBuildingsHandler
{
    /// <summary>
    /// Handles the retrieval of buildings.
    /// </summary>
    /// <param name="buildingService"></param>
    /// <returns>Response for retrieving buildings.</returns>
    public static async Task<GetBuildingsResponse> HandleAsync(IBuildingService buildingService)
    {
        var buildings = await buildingService.GetBuildingsAsync();
        return new GetBuildingsResponse(buildings.Select(BuildingDtoMapper.ToDtoWithId));
    }
}
