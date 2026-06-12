using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services.Implementations;

/// <summary>
/// Provides authentication-related operations including user registration.
/// </summary>
internal class CustomAuthenticationService : ICustomAuthenticationService
{
    private readonly ApiClient _apiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly HashSet<string> _registeredUsers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomAuthenticationService"/> class.
    /// </summary>
    /// <param name="apiClient">The API client for backend communication.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor for accessing user information.</param>
    public CustomAuthenticationService(
        ApiClient apiClient, 
        IHttpContextAccessor httpContextAccessor)
    {
        _apiClient = apiClient;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Triggers user registration in the backend system.
    /// </summary>
    /// <param name="principal">The claims principal of the authenticated user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task TriggerUserRegistrationAsync(ClaimsPrincipal principal)
    {
        try
        {
            // Check if user is authenticated before making the call
            if (principal.Identity.IsAuthenticated != true)
            {
                return;
            }

            // Get Azure B2C specific claims - these match what the backend expects
            var objectId = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            var email = principal.FindFirst("emails")?.Value;
            var fullName = principal.FindFirst("extension_FullName")?.Value;

            var userId = objectId ?? "Unknown";

            // Prevent multiple registration calls for the same user
            if (_registeredUsers.Contains(userId))
            {
                return;
            }
            _registeredUsers.Add(userId);

            // Call the register endpoint
            await _apiClient.Register.PutAsync();
        }
        catch (Microsoft.Kiota.Abstractions.ApiException)
        {
            // Remove from registered users cache if registration failed, so we can try again later
            var httpContext = _httpContextAccessor.HttpContext;
            var objectId = httpContext?.User?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")
                ?.Value;
            _registeredUsers.Remove(objectId);
        }
        catch (HttpRequestException)
        {
        }
        catch (Exception)
        {
        }
    }
}
