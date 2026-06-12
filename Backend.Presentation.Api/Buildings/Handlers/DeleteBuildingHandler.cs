using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;

/// <summary>
/// Handles the deletion of a building entity.
/// </summary>
public static class DeleteBuildingHandler
{
    /// <summary>
    /// Handles the delete building request asynchronously.
    /// </summary>
    /// <param name="buildingService">The building service used to delete the building.</param>
    /// <param name="officialId">The official ID of the building to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task<IResult> HandleAsync(IBuildingService buildingService, string officialId)
    {
        try
        {
            await buildingService.DeleteBuildingAsync(officialId);

            var response = new DeleteBuildingResponse($"The building with ID: {officialId} was deleted successfully.");

            return TypedResults.Ok(response);
        }
        catch (BuildingDataException ex)
        {
            return TypedResults.BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}