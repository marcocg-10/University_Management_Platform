using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings;

/// <summary>
/// Extension methods for mapping building-related endpoints.
/// </summary>
internal static class BuildingEndpoints
{
    /// <summary>
    /// Maps building-related endpoints to the provided route builder.
    /// </summary>
    /// <param name="routes">The route builder to map endpoints to.</param>
    /// <returns>The updated route builder.</returns>
    internal static IEndpointRouteBuilder MapBuildingEndpoints(this IEndpointRouteBuilder routes)
    {
        var buildingsGroup = routes.MapGroup("/buildings");

        buildingsGroup.MapGet("/", GetBuildingsHandler.HandleAsync)
            .Produces<ErrorResponse>((int) HttpStatusCode.InternalServerError)
            .WithName("GetBuildings").RequireAuthorization("ListBuildings");

        buildingsGroup.MapPost("/", CreateBuildingsHandler.HandleAsync)
            .Produces<ConflictErrorResponse>((int) HttpStatusCode.Conflict)
            .Produces<ValidationErrorResponse>((int) HttpStatusCode.BadRequest)
            .Produces<ErrorResponse>((int) HttpStatusCode.InternalServerError)
            .WithName("CreateBuilding").RequireAuthorization("ManageBuildings");

        routes.MapDelete("/buildings/{officialId}", DeleteBuildingHandler.HandleAsync)
            .Produces<ConflictErrorResponse>((int) HttpStatusCode.Conflict)
            .Produces<ValidationErrorResponse>((int) HttpStatusCode.BadRequest)
            .Produces<ErrorResponse>((int) HttpStatusCode.InternalServerError)
            .WithName("DeleteBuilding").RequireAuthorization("ManageBuildings");

        routes.MapPut("/buildings/{officialId}", UpdateBuildingHandler.HandleAsync)
            .Produces<ConflictErrorResponse>((int) HttpStatusCode.Conflict)
            .Produces<ValidationErrorResponse>((int) HttpStatusCode.BadRequest)
            .Produces<ErrorResponse>((int) HttpStatusCode.InternalServerError)
            .WithName("UpdateBuilding").RequireAuthorization("ManageBuildings");
        return routes;
    }
}
