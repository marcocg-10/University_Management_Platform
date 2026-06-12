using System.Security.Claims;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services;

/// <summary>
/// Interface for handling authentication-related operations.
/// </summary>
public interface ICustomAuthenticationService
{
    /// <summary>
    /// Triggers user registration in the backend system.
    /// </summary>
    /// <param name="principal">The claims principal of the authenticated user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task TriggerUserRegistrationAsync(ClaimsPrincipal principal);
}
