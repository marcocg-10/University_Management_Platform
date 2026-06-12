using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Frontend.Application;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure;

namespace UCR.ECCI.PI.ThemePark.Frontend.DependencyInjection;

/// <summary>
/// Extension methods for registering services in the dependency injection container.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers all layers of the Clean Architecture into the dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The IServiceCollection to which the application services will be added.
    /// </param>
    /// <param name="configuration">
    /// </param>
    /// <returns>
    /// The same IServiceCollection, allowing for method chaining.
    /// </returns>
    public static IServiceCollection AddCleanArchitecture(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationLayerServices()
            .AddInfrastructureLayerServices(configuration);

      return services;
    }
}