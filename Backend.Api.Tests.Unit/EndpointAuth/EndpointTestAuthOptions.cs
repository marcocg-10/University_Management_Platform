using System.Security.Claims;

namespace UCR.ECCI.PI.ThemePark.Backend.Api.Tests.Unit.ApiAuth;

/// <summary>
/// Configuration options for the test authentication handler.
/// </summary>
public class EndpointTestAuthOptions
{
    public bool IsAuthenticated { get; set; }
    public bool IsExpired { get; set; }
    public List<Claim> AdditionalClaims { get; set; } = new();
}
