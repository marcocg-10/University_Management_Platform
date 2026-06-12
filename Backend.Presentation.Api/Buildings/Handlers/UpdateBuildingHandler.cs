using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Requests;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;

/// <summary>
/// Handles the update of a building entity.
/// </summary>
public static class UpdateBuildingHandler
{
    /// <summary>
    /// Handles the update building request asynchronously.
    /// </summary>
    /// <param name="buildingService">The building service used to update the building.</param>
    /// <param name="request">The update building request containing the building data.</param>
    /// <returns>This returns an <see cref="IResult"/> representing the outcome of the update operation.</returns>
    public static async Task<IResult> HandleAsync(IBuildingService buildingService, 
        UpdateBuildingRequest request)
    {
            BuildingRenderInfoDto renderInfoDto = new BuildingRenderInfoDto(
                request.Color, request.Height, request.Width, request.Depth, request.X, request.Y, request.Z, request.Texture);
            BuildingDto buildingDto = new BuildingDto(request.OfficialID, request.Name, request.FloorCount, renderInfoDto);
            var building = BuildingDtoMapper.toEntity(buildingDto);
            await buildingService.UpdateBuildingAsync(building);
            var response = new UpdateBuildingResponse($"The building with ID: {request.OfficialID} was updated successfully.");
            return TypedResults.Ok(response);
    }
}
