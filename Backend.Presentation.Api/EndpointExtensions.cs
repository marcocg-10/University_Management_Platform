using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api;

/// <summary>
/// Provides extension methods for mapping API endpoints to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
/// <remarks>
/// A single extension method is defined:
/// <see cref="MapApiEndpoints(IEndpointRouteBuilder"/>
/// </remarks>
public static class EndpointExtensions
{
    /// <summary>
    /// Maps the API endpoints for the application to the specified route builder.
    /// </summary>
    /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to which the API endpoints will be mapped.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> instance with the mapped API endpoints.</returns>
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        // Map endpoints (routes.Map_____Endpoints();)
        routes.MapInteractiveComponentEndpoints();

        routes.MapBuildingEndpoints();
      
        routes.MapLearningSpaceEndpoints();
      
        routes.MapUserEndpoints();

        routes.MapPermissionEndpoints();

        routes.MapRoleEndpoints();
        
        routes.Map("/validate", () => "API has token")
            .WithName("ValidateApi")
            .RequireAuthorization();
        return routes;
    }
}