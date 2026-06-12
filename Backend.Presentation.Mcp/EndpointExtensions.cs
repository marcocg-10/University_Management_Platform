using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp;

/// <summary>
/// Provides extension methods for registering Model Context Protocol (MCP) endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Maps the MCP endpoint on the specified application route.
    /// </summary>
    /// <param name="routes">The route builder used to register the Mcp routes.</param>
    /// <returns>The route builder instance.</returns>
    public static IEndpointRouteBuilder MapMcpEndpoints(this IEndpointRouteBuilder routes)
    {
        // Route that agents will use to communicate.
        routes.MapMcp("/mcp");

        return routes;
    }
}