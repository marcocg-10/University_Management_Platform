using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp;

namespace UCR.ECCI.PI.ThemePark.Backend.DependencyInjection;

/// <summary>
/// Provides extension methods for registering the API and MCP endpoints.
/// </summary>
/// <remarks>
/// A single extension method is defined:
/// <see cref="MapCleanArchitectureEndpoints(IEndpointRouteBuilder"/>
/// </remarks>
public static class EndpointExtensions
{
    /// <summary>
    /// Maps the API and MCP endpoints on the specified application route.
    /// </summary>
    /// <param name="routes">The route builder used to register the API and Mcp routes</param>
    /// <returns>The route builder instance.</returns>
    public static IEndpointRouteBuilder MapCleanArchitectureEndpoints(this IEndpointRouteBuilder routes)
    {
        // Map the API and MCP endpoints.
        
        routes.MapApiEndpoints();
        routes.MapMcpEndpoints();

        return routes;
    }
}