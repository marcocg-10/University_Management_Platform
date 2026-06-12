using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services.Implementations;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain;

/// <summary>
/// Extension methods for configuring dependency injection for domain services.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds domain services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDomainLayerServices(this IServiceCollection services)
    {
        // Register Building Collision services.
        services.AddScoped<IBuildingCollisionService, BuildingCollisionService>();
        services.AddScoped<IBuildingCollisionDetector, BuildingCollisionDetector>();

        // Register Interactive Component Collision services.
        services.AddScoped<IInteractiveComponentCollisionService, InteractiveComponentCollisionService>();
        services.AddScoped<IInteractiveComponentCollisionDetector, InteractiveComponentCollisionDetector>();

        // Register Interactive Component Containment services.
        services.AddScoped<IInteractiveComponentContainmentService, InteractiveComponentContainmentService>();
        services.AddScoped<IInteractiveComponentContainmentDetector, InteractiveComponentContainmentDetector>();

        return services;
    }
}