using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles;

internal static class RoleEndpoints
{
    internal static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder routes)
    {
        // Endpoint for create roles
        routes
            .MapPost("/roles", Handlers.CreateRoleHandler.HandleAsync)
            .WithName("CreateRole")
            .RequireAuthorization("ManageRoles");

        routes
            .MapPut("/assignPermissionToRole/{roleId}", Handlers.AssignPermissionHandler.HandleAsync)
            .WithName("AssignRole")
            .RequireAuthorization("ManageRoles")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces<ConflictErrorResponse>(StatusCodes.Status409Conflict)
            .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ExceptionResult>(StatusCodes.Status500InternalServerError);

        // Endpoint for getting roles
        routes
            .MapGet("/roles", Handlers.ListRolesHandler.HandleAsync)
            .WithName("GetRoles")
            .RequireAuthorization("ManageRoles");

        // Endpoint for getting role permissions
        routes
            .MapGet("/roles/{roleId}/permissions", Handlers.ListRolePermissionsHandler.HandleAsync)
            .WithName("GetRolePermissions")
            .Produces<ListRolesPermissionsResponse>(StatusCodes.Status200OK)
            .Produces<ListRolesPermissionsErrorResponse>(StatusCodes.Status500InternalServerError).RequireAuthorization("ManageRoles");

        routes
          .MapGet("/searchRoles", SearchRolesHandler.HandleAsync)
          .WithName("SearchRoles")
          .RequireAuthorization("ManageRoles");

        routes
            .MapGet("/roles/paginated", ListRolesPagedHandler.HandleAsync)
            .WithName("GetRolesPaged")
            .RequireAuthorization("ManageRoles");

        return routes;
    }
   
}
