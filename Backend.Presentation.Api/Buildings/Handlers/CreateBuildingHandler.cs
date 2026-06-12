using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;

/// <summary>
/// Handler for creating buildings.
/// </summary>
public static class CreateBuildingsHandler
{
    /// <summary>
    /// Handles the creation of a building.
    /// </summary>
    /// <param name="buildingService"></param>
    /// <param name="request"></param>
    /// <returns>Response for creating a building.</returns>
    public static async Task<CreateBuildingResponse> HandleAsync(IBuildingService buildingService, CreateBuildingRequest request)
    {

        BuildingRenderInfoDto renderInfoDto = new BuildingRenderInfoDto(request.Color, request.Height, request.Width,
            request.Depth, request.X, request.Y, request.Z, request.Texture);
        BuildingDto buildingDto = new BuildingDto(request.OfficialID, request.Name, request.FloorCount, renderInfoDto);

        Building building = BuildingDtoMapper.toEntity(buildingDto);

        await buildingService.CreateBuildingAsync(building);
        return new CreateBuildingResponse(BuildingDtoMapper.ToDtoWithId(building));
    }
}
