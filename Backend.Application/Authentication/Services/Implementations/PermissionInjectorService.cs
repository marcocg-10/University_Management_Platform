using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Authentication.Services.Implementations;

/// <summary>
/// Service that injects user permissions into claims after authentication.
/// </summary>
internal class PermissionInjectorService : IClaimsTransformation
{
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionInjectorService"/> class.
    /// </summary>
    /// <param name="permissionRepository"></param>
    /// <remarks>Adapted from
    /// <see href="https://webtiger.co.uk/posts/2024-02-05/injecting-claims-into-the-net-authentication-pipeline/"/>
    /// </remarks>
    public PermissionInjectorService(IUserRepository permissionRepository)
    {
        _userRepository = permissionRepository;
    }
    
    /// <summary>
    /// Transforms the given ClaimsPrincipal by adding permission claims.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        try
        {
            ClaimsIdentity? identity = principal.Identity as ClaimsIdentity;
            if (identity is null || !identity.IsAuthenticated)
            {
                return principal;
            }
        
            var azureObjectIdentifier = 
                GetClaimValue(principal.Claims, "http://schemas.microsoft.com/identity/claims/objectidentifier");
            var resultUser = await _userRepository.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier!);
            if (resultUser is null)
            {
                return principal;
            }
            var resultPermissions =  
                await _userRepository.GetCurrentUserPermissionsAsync(resultUser.IdKey);
        
            if (resultPermissions is null )
            {
                return principal;
            }
        
            foreach (var permission in resultPermissions)
            {
                identity.AddClaim(new Claim("extension_Permissions", permission.Name.Value));
            }
        
            return new ClaimsPrincipal(identity); 
        } catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while transforming claims: {e.Message}", e);
        }
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
