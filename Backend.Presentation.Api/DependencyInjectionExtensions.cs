using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Middlewares;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Middlewares;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.InteractiveComponents.Middleware;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.LearningSpaces.Middlewares;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Middlewares;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Middlewares;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;
#if SWAGGER
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Swagger;
#endif

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api;

/// <summary>
/// Provides extension methods for dependency injection setup,
/// following the Clean Architecture pattern.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers all the services that belong to the Presentation API Layer.
    /// </summary>
    /// <param name="services">
    /// The IServiceCollection to which the application services will be added.
    /// </param>
    /// <returns>
    /// The same IServiceCollection, allowing for method chaining.
    /// </returns>
    public static IServiceCollection AddPresentationLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the global exception handling middleware.
        services.AddTransient<ExceptionHandlerMiddleware>();

        // Register specific exception handlers for different domains.
        services.AddTransient<IExceptionHandler, BuildingExceptionHandler>();
        services.AddTransient<IExceptionHandler, InteractiveComponentExceptionHandler>();
        services.AddTransient<IExceptionHandler, LearningSpaceExceptionHandler>();
        services.AddTransient<IExceptionHandler, RoleExceptionHandler>();
        services.AddTransient<IExceptionHandler, PermissionExceptionHandler>();

        // Additional domain-specific exception handlers can be registered here.
        // Each handler should implement the IExceptionHandler interface.
        // Add OpenAPI
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        // Add authentication
        var azureEntraIdConfiguration = configuration.GetSection("AzureAd");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddMicrosoftIdentityWebApi(azureEntraIdConfiguration);

        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            // Hooking into the token validation event preserving the existing handler(s) if any
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                    var roleService = context.HttpContext.RequestServices.GetRequiredService<IRoleService>();
                    await RegisterUserFromClaimsHandler.HandleAsync(userService, roleService, context.Principal!);
                    var claimTransformer =
                        context.HttpContext.RequestServices.GetRequiredService<IClaimsTransformation>();
                    var transformed = await claimTransformer.TransformAsync(context.Principal!);
                    context.Principal = transformed;

                    if (context.Properties is not null)
                    {
                        context.Properties.Items[".AuthScheme"] =
                            context.Scheme?.Name
                            ?? (context.Properties.Items.TryGetValue(".AuthScheme", out var existing) ? existing : null);
                    }
                }
            };

        });

        // Add authorization handler
        services.AddAuthorization(
            options =>
            {
                // List the permissions; at the moment only CRUDS
                var permissionList = new[]
                {
                   "ListUsers",
                   "CreateUsers",
                   "AssignRole",
                   "ManageRoles",
                   "ManageBuildings",
                   "ManageInterComponents",
                   "ManageLearningSpaces",

                   "ListBuildings",
                   "ListInterComponents",
                   "ListLearningSpaces"
                };

                foreach (var permission in permissionList)
                {
                    options.AddPolicy(permission, policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("extension_Permissions", permission);
                    });
                }
            });
        // Add Swagger configuration
#if SWAGGER
        services.AddSwaggerConfiguration(configuration);
#endif

        return services;
    }
}
