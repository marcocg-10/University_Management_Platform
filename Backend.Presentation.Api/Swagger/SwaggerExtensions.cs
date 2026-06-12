#if SWAGGER
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Swagger;
/// <summary>
/// Class for adding the Swagger related configurations to the build.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger with Azure AD authentication for the API.
    /// </summary>
    /// <param name="services">The service collection to which Swagger and authentication services will be added.</param>
    /// <param name="configuration">The application configuration containing Azure AD settings.</param>
    /// <returns>The updated service collection with Swagger and authentication configured.</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var azureEntraIdConfiguration = configuration.GetSection("AzureAd");
        // Configure Microsoft Identity platform (Azure AD v2.0) authentication for Web API
        if (azureEntraIdConfiguration.Exists() && !string.IsNullOrEmpty(azureEntraIdConfiguration["Instance"]))
        {
            services.AddSwaggerGen(x =>
                {
                var apiScopeUrl = $"https://{azureEntraIdConfiguration["Domain"]}/{azureEntraIdConfiguration["ClientId"]}";
                var scopes = azureEntraIdConfiguration["Scopes"]?
                    .Split(',')
                    .Select(scope => $"{apiScopeUrl}/{scope}")
                    .ToList() ?? new List<string>();
                scopes.AddRange(["openid", "offline_access"]);

                var baseUrl = $"{azureEntraIdConfiguration["Instance"]}{azureEntraIdConfiguration["Domain"]}/{azureEntraIdConfiguration["SignUpSignInPolicyId"]}";
                x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "OAuth2.0 Auth Code with PKCE",
                        Name = "oauth2",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{baseUrl}/oauth2/v2.0/authorize"),
                                TokenUrl = new Uri($"{baseUrl}/oauth2/v2.0/token"),//token end point
                                Scopes = scopes.ToDictionary(scope => scope, scope => scope),
                            }
                        }
                    });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                                    Scheme = "oauth2",
                                    Name = "oauth2",
                                    In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                     });
            
            });
        }

        return services;
    }
}
#endif