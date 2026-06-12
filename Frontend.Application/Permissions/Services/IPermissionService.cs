using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Permissions.Services;

/// <summary>
/// Defines a contract for managing and retrieving permissions.
/// </summary>
/// <remarks>This service provides methods to retrieve permission data, which can be used to manage access control
/// within an application. Implementations of this interface should ensure thread safety for concurrent usage.</remarks>
public interface IPermissionService
{
    /// <summary>
    /// Asynchronously retrieves all available permissions.
    /// </summary>
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    /// <summary>
    /// Checks if the current user has the specified permission.
    /// </summary>
    /// <param name="permissionName">The name of the permission to check.</param>
    /// <returns>True if the user has the permission, false otherwise.</returns>
    Task<bool> HasPermissionAsync(string permissionName);

    /// <summary>
    /// Gets all permissions for the current user.
    /// </summary>
    /// <returns>A collection of permission names the current user has.</returns>
    Task<IEnumerable<string>> GetCurrentUserPermissionsAsync();

    /// <summary>
    /// Checks if the current user has any of the specified permissions.
    /// </summary>
    Task<bool> HasAnyPermissionAsync(params string[] permissionNames);

    /// <summary>
    /// Checks if the current user has all of the specified permissions.
    /// </summary>
    Task<bool> HasAllPermissionsAsync(params string[] permissionNames);
}
