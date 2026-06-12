using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Handlers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions;

/// <summary>
/// Provides extension methods for mapping permission-related endpoints to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
/// <remarks>This class includes methods for registering endpoints that handle permission-related operations, such as
/// retrieving permission and creating new permission. The endpoints are mapped to their respective handlers and configured
/// with appropriate route names.</remarks>
internal static class PermissionEndpoints
{

    /// <summary>
    /// Maps permission-related endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This method registers the following endpoints: <list type="bullet"> <item>
    /// <description><c>POST /createPermission</c>: Creates new permissions based on the provided data.</description> </item>
    /// </list></remarks>
    /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to which the endpoints will be added.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> instance with the permission-related endpoints mapped.</returns>
    internal static IEndpointRouteBuilder MapPermissionEndpoints(this IEndpointRouteBuilder routes)
    {
        routes
            .MapPost("/permissions", CreatePermissionsHandler.HandleAsync)
            .WithName("CreatePermissions").RequireAuthorization("ManageRoles");

        routes
           .MapGet("/permissions", GetAllPermissionsHandler.HandleAsync)
           .WithName("GetPermissions").RequireAuthorization("ManageRoles");

        return routes;
    }
}
