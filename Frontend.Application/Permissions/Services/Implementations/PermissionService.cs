using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Permissions.Services.Implementations;

/// <summary>
/// Service for managing user permissions and authorization (Clean Architecture - Application Layer).
/// Communicates with backend ONLY via API calls through repositories.
/// </summary>
internal class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IUserService _userService;
    
    // Cache permissions for the current request to avoid multiple API calls
    private IEnumerable<string>? _cachedPermissions;
    private string? _cachedUserOid;

    public PermissionService(
        IPermissionRepository permissionRepository,
        AuthenticationStateProvider authenticationStateProvider,
        IUserService userService)
    {
        _permissionRepository = permissionRepository;
        _authenticationStateProvider = authenticationStateProvider;
        _userService = userService;
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _permissionRepository.GetAllPermissionsAsync();
    }

    public async Task<bool> HasPermissionAsync(string permissionName)
    {
        var permissions = await GetCurrentUserPermissionsAsync();
        return permissions.Contains(permissionName);
    }

    public async Task<IEnumerable<string>> GetCurrentUserPermissionsAsync()
    {
        // Get the current user's Azure Object Identifier
        var azureOid = await GetCurrentUserAzureOidAsync();
        if (string.IsNullOrEmpty(azureOid))
        {
            return Enumerable.Empty<string>();
        }

        // Check cache to avoid repeated API calls for the same user in the same request
        if (_cachedUserOid == azureOid && _cachedPermissions != null)
        {
            return _cachedPermissions;
        }

        try
        {
            // Call repository with Azure Object Identifier
            var (permissions, errorMessage) = await _permissionRepository.GetCurrentUserPermissionsAsync(azureOid);
            
            if (permissions == null)
            {
                // Log error if needed
                return Enumerable.Empty<string>();
            }

            // Convert Permission entities to permission name strings
            var permissionNames = permissions.Select(p => p.Name.Value).ToList();
            
            // Cache the result
            _cachedUserOid = azureOid;
            _cachedPermissions = permissionNames;
            
            return permissionNames;
        }
        catch (Exception)
        {
            // Log exception if needed
            return Enumerable.Empty<string>();
        }
    }

    public async Task<bool> HasAnyPermissionAsync(params string[] permissionNames)
    {
        if (permissionNames == null || permissionNames.Length == 0)
            return false;

        var userPermissions = await GetCurrentUserPermissionsAsync();
        return permissionNames.Any(p => userPermissions.Contains(p));
    }

    public async Task<bool> HasAllPermissionsAsync(params string[] permissionNames)
    {
        if (permissionNames == null || permissionNames.Length == 0)
            return true;

        var userPermissions = await GetCurrentUserPermissionsAsync();
        return permissionNames.All(p => userPermissions.Contains(p));
    }

    private async Task<string?> GetCurrentUserAzureOidAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User?.Identity?.IsAuthenticated != true)
            return null;

        return GetAzureObjectIdentifier(authState.User);
    }

    private static string? GetAzureObjectIdentifier(ClaimsPrincipal user)
    {
        var claimTypes = new[]
        {
            "http://schemas.microsoft.com/identity/claims/objectidentifier",
            ClaimTypes.NameIdentifier,
            "oid",
            "sub"
        };

        foreach (var claimType in claimTypes)
        {
            var value = user.FindFirst(claimType)?.Value;
            if (!string.IsNullOrEmpty(value))
                return value;
        }

        return null;
    }
}
