using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure;

/// <summary>
/// Provides extension methods for registering application layer services.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services in the dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The service collection where the Infrastructure services will be registered.
    /// </param>
    /// <param name="configuration">
    /// The application configuration, used to get the database connection string.
    /// </param>
    /// <returns>
    /// /// The same service collection, allowing other registration calls to be chained.
    /// </returns>
    /// <remarks>
    /// We set the connection string as 'DefaultConnection' even though the database has not been created yet.
    /// </remarks> 
    public static IServiceCollection AddInfrastructureLayerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the database context using the connection string from configuration.

        // We set the AppDbContext to use SQL Server.
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories from the Infrastructure layer here.
        // Example:
        // services.AddScoped<IRepository, Repository>();
      
        services.AddScoped<IBuildingRepository, BuildingRepository>();
      
        services.AddScoped<ILearningSpaceRepository, LearningSpaceRepository>();
        
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.AddScoped<IInteractiveComponentRepository, InteractiveComponentRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}