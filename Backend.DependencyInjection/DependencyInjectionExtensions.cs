using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain;
using UCR.ECCI.PI.ThemePark.Backend.Application;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mcp;

namespace UCR.ECCI.PI.ThemePark.Backend.DependencyInjection;

/// <summary>
/// Provides a single entry point to register all services required by the solution,
/// following the Clean Architecture approach.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers the Application, Infrastructure, and MCP layer services
    /// in the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection where services are registered.</param>
    /// <param name="configuration">The application configuration, used by Infrastructure.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddCleanArchitecture(
        this IServiceCollection services,
        IConfiguration configuration)

    {
        // Register Domain layer services
        services.AddDomainLayerServices();

        // Register Application layer services
        services.AddApplicationLayerServices();

        // Register Infrastructure layer services (needs configuration for DbContext)
        services.AddInfrastructureLayerServices(configuration);

        // Register Mcp services
        services.AddMcpLayerServices();

        // Register Presentation API services
        services.AddPresentationLayerServices(configuration);

    // Return the updated service collection to support chaining
    return services;
    }
}
