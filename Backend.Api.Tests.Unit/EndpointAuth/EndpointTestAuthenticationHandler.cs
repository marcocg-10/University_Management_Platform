using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace UCR.ECCI.PI.ThemePark.Backend.Api.Tests.Unit.ApiAuth;

/// <summary>
/// Custom authentication handler for testing that simulates various authentication states.
/// </summary>
public class EndpointTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly EndpointTestAuthOptions _authOptions;

    public EndpointTestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        EndpointTestAuthOptions authOptions) : base(options, logger, encoder)
    {
        _authOptions = authOptions;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Handle unauthenticated state
        if (!_authOptions.IsAuthenticated)
        {
            return Task.FromResult(AuthenticateResult.Fail("Not authenticated for test"));
        }

        // Handle expired token state
        if (_authOptions.IsExpired)
        {
            return Task.FromResult(AuthenticateResult.Fail("Token expired for test"));
        }

        // Create successful authentication result
        var claims = CreateTestClaims();
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private List<Claim> CreateTestClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "TestUser"),
            new(ClaimTypes.NameIdentifier, "123"),
            new(ClaimTypes.Email, "testuser@test.com"),
            new("scope", "App.Read")
        };

        // Add any additional claims provided by the test
        claims.AddRange(_authOptions.AdditionalClaims);

        return claims;
    }
}