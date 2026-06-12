using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;
using Microsoft.Identity.Web;
namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Authentication.Services.Implementations;

/// <summary>
/// Service that injects user permissions into claims after authentication.
/// </summary>
internal class PermissionInjectorService : IClaimsTransformation
{
    private readonly IPermissionRepository _kiotaPermissionRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionInjectorService"/> class.
    /// </summary>
    /// <param name="kiotaPermissionRepository"></param>
    /// <remarks>Adapted from
    /// <see href="https://webtiger.co.uk/posts/2024-02-05/injecting-claims-into-the-net-authentication-pipeline/"/>
    /// </remarks>
    public PermissionInjectorService(IPermissionRepository kiotaPermissionRepository)
    {
        _kiotaPermissionRepository = kiotaPermissionRepository;
    }
    
    /// <summary>
    /// Transforms the given ClaimsPrincipal by adding permission claims.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity? identity = principal.Identity as ClaimsIdentity;
        if (identity == null)
        {
            return Task.FromResult(principal).Result;
        }
        
        var azureObjectIdentifier = 
            GetClaimValue(principal.Claims, "http://schemas.microsoft.com/identity/claims/objectidentifier");
        var userPermissions =  
            await _kiotaPermissionRepository.GetCurrentUserPermissionsAsync(azureObjectIdentifier);
        
        if (userPermissions.errorMessage != null || userPermissions.permissions == null)
        {
            return principal;
        }
        
        foreach (var permission in userPermissions.permissions)
        {
            identity.AddClaim(new Claim("extension_Permissions", permission.Name.Value));
        }
        
        return new ClaimsPrincipal(identity);
    }
    
    /// <summary>
    /// Gets the value of a claim by type.
    /// </summary>
    /// <param name="claims">The collection of claims.</param>
    /// <param name="claimType">The type of claim to retrieve.</param>
    /// <returns>The claim value or null if not found.</returns>
    private static string? GetClaimValue(IEnumerable<Claim> claims, string claimType)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
