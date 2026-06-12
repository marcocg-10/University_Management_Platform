using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services in the dependency injection container.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers infrastructure layer services into the dependency injection container.
    /// </summary>
    /// <param name="services">
    /// The IServiceCollection to which the application services will be added.
    /// </param>
    /// <param name="configuration"></param>
    /// <returns>
    /// The same IServiceCollection, allowing for method chaining.
    /// </returns>
    public static IServiceCollection AddInfrastructureLayerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IAccessTokenProvider, KiotaAccessTokenProvider>();
        services.AddScoped<IAuthenticationProvider, BaseBearerTokenAuthenticationProvider>();
        services.AddScoped<IRequestAdapter>((services) =>
        {
            var authProvider = services.GetRequiredService<IAuthenticationProvider>();
            var requestAdapter = new HttpClientRequestAdapter(authProvider);
            var usedUrl = configuration["ApiBaseUrlLocal"] ?? configuration["ApiBaseUrl"];
            requestAdapter.BaseUrl = usedUrl;
            
            return requestAdapter;
        });
        services.AddScoped<ApiClient>();

        // Example: services.AddSingleton<I______Service, ______Service>();

        // Register application layer services here
        services.AddScoped<IBuildingRepository, KiotaBuildingRepository>();

        services.AddScoped<ILearningSpaceRepository, KiotaLearningSpaceRepository>();

        services.AddScoped<IInteractiveComponentRepository, KiotaInteractiveComponentRepository>();

        services.AddScoped<IUserRepository, KiotaUserRepository>();

        services.AddScoped<IPermissionRepository, KiotaPermissionRepository>();

        services.AddScoped<IRoleRepository, KiotaRoleRepository>();

        return services;
    }
}