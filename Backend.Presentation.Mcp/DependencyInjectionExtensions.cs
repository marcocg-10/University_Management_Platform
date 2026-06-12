using Microsoft.Extensions.DependencyInjection;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp;

/// <summary>
/// Provides methods for registering Model Context Protocol (MCP) layer services.
/// </summary>
/// <remarks>
/// Configures MCP server and its dependencies, such as HTTP transport and tools from current
/// assembly.
/// </remarks>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers services needed for the MCP services.
    /// </summary>
    /// <param name="services"> The application's collection of services.</param>
    /// <returns>The application services instance.</returns>
    public static IServiceCollection AddMcpLayerServices(this IServiceCollection services)
    {
        // Configure MCP services: HTTP transport, tool discovery from current assembly.
        services
            .AddMcpServer()
            .WithHttpTransport()
            .WithToolsFromAssembly();

        return services;
    }
}