using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents;

/// <summary>
/// Provides extension methods to map HTTP endpoints for managing interactive components,
/// specifically boards and projectors, in the theme park API.
/// </summary>
/// <remarks>
/// Each endpoint is mapped with its HTTP method, route, response types, and status codes.
/// This class centralizes the endpoint definitions for interactive components.
/// </remarks>
internal static class InteractiveComponentEndpoints
{
    /// <summary>
    /// Maps all interactive component-related endpoints to the provided <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="routes">The endpoint route builder used to register routes.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> instance for chaining.</returns>
    internal static IEndpointRouteBuilder MapInteractiveComponentEndpoints(this IEndpointRouteBuilder routes)
    {
        var interactiveComponentsGroup = routes.MapGroup("/interactive-components");

        // Endpoint for creating a new board
        interactiveComponentsGroup.MapPost("/board", CreateBoardHandler.HandleAsync)
            .WithName("CreateBoard").RequireAuthorization("ManageInterComponents");

        // Endpoint for reading a board by its PlateId
        interactiveComponentsGroup.MapGet("/board/{plateId}", ReadBoardByPlateIdHandler.HandleAsync)
            .WithName("ReadBoardByPlateId").RequireAuthorization("ManageInterComponents");

        // Endpoint for updating an existing board
        interactiveComponentsGroup.MapPut("/board/{plateId}", UpdateBoardHandler.HandleAsync)
            .WithName("UpdateBoard").RequireAuthorization("ManageInterComponents");

        // Endpoint for deleting a board by its PlateId
        interactiveComponentsGroup.MapDelete("/board/{plateId}", DeleteBoardHandler.HandleAsync)
            .WithName("DeleteBoard").RequireAuthorization("ManageInterComponents");

        // Endpoint for listing all boards
        interactiveComponentsGroup.MapGet("/board", ListAllBoardsHandler.HandleAsync)
            .WithName("ListAllBoards").RequireAuthorization("ListInterComponents");

        // Endpoint for creating a new projector
        interactiveComponentsGroup.MapPost("/projector", CreateProjectorHandler.HandleAsync)
            .WithName("CreateProjector").RequireAuthorization("ManageInterComponents");

        // Endpoint for listing all Projectors
        interactiveComponentsGroup.MapGet("/projector", ListAllProjectorsHandler.HandleAsync)
            .WithName("ListAllProjectors").RequireAuthorization("ListInterComponents");

        // Endpoint for listing boards with pagination
        interactiveComponentsGroup.MapGet("/boards", ListBoardsPagedHandler.HandleAsync)
            .WithName("ListBoardsPaginated").RequireAuthorization("ListInterComponents");

        // Endpoint for filtering boards
        interactiveComponentsGroup.MapGet("/boards/filter", FilterBoardsHandler.HandleAsync)
            .WithName("FilterBoards").RequireAuthorization("ListInterComponents");

        // Endpoint for filtering projectors
        interactiveComponentsGroup.MapGet("/projectors/filter", FilterProjectorHandler.HandleAsync)
            .WithName("FilterProjectors").RequireAuthorization("ListInterComponents");
        

        return routes;
    }
}
