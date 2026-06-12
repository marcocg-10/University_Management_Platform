using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure;

internal class KiotaAccessTokenProvider : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccesor;

    public KiotaAccessTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccesor = httpContextAccessor;
    }
    public AllowedHostsValidator AllowedHostsValidator => new();

    public async Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (_httpContextAccesor.HttpContext is null)
        {
            // Return empty string instead of throwing exception for missing HttpContext
            return string.Empty;
        }

        // Check if the user is authenticated
        if (!_httpContextAccesor.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            // Return empty string for unauthenticated users
            return string.Empty;
        }

        try
        {
            var accessToken = await _httpContextAccesor.HttpContext.GetTokenAsync(
                OpenIdConnectDefaults.AuthenticationScheme,
                "access_token");

            // Return empty string instead of throwing exception for missing token
            return accessToken ?? string.Empty;
        }
        catch
        {
            // Return empty string on any token retrieval error
            return string.Empty;
        }
    }
}
