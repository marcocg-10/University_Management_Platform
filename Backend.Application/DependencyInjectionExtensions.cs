using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UCR.ECCI.PI.ThemePark.Backend.Application.Authentication.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services.Implementations;

namespace UCR.ECCI.PI.ThemePark.Backend.Application;

/// <summary>
/// Provides extension methods for registering application layer services.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers all the services that belong to the Application Layer.
    /// </summary>
    /// <param name="services">
    /// The IServiceCollection to which the application services will be added.
    /// </param>
    /// <returns>
    /// The same IServiceCollection, allowing for method chaining.
    /// </returns>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {
        // Here we can register all the services of the Application layer.
        services.AddScoped<IBuildingService, BuildingService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ILearningSpaceService, LearningSpaceService>();
        services.AddScoped<IInteractiveComponentService, InteractiveComponentService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IClaimsTransformation, PermissionInjectorService>();

        // Add FluentValidation validators from the current assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
