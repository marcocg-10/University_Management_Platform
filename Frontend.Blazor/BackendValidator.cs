using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor;

/// <summary>
/// Validates the backend api during token validation.
/// </summary>
public class BackendValidator
{
    /// <summary>
    /// Validates the backend api during token validation.
    /// </summary>
    /// <param name="context">The token validated context.</param>
    /// <returns>A task representing the asynchronous operation.</returns> 
    public static async Task AuthenticateWithBackendAsync(TokenValidatedContext context)
    {
        try
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();

            var apiUrl = configuration["ApiBaseUrl"];
            var accessToken = context.TokenEndpointResponse?.AccessToken
                              ?? context.Properties?.GetTokenValue("access_token");

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(apiUrl))
            {
                using var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                await client.GetAsync($"{apiUrl}/validate"); 
            }
        }
        catch (Exception ex)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(ex, "Failed to authenticate with backend during token validation");
        }
    }
}