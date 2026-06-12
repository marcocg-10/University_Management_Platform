using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.InteractiveComponents.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Permissions.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Permissions.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Roles.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services.Implementations;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application;

/// <summary>
/// Extension methods for registering application layer services.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers application layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The IServiceCollection to which the application services will be added.
    /// </param>
    /// <returns>
    /// The same IServiceCollection, allowing for method chaining.
    /// </returns>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        // Example: services.AddSingleton<I______Service, ______Service>();

        // Register application layer services here
        services.AddScoped<IBuildingService, BuildingService>();

        services.AddScoped<ILearningSpaceService, LearningSpaceService>();

        services.AddScoped<IInteractiveComponentService, InteractiveComponentService>();

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();

        services.AddScoped<IClaimsTransformation, PermissionInjectorService>();

        return services;
    }
}