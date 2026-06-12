using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services;


/// <summary>
/// Defines a contract for managing permissions within the system.
/// </summary>
/// <remarks>This service provides methods for creating, retrieving, updating, and deleting permissions.  It is
/// designed to be used in scenarios where fine-grained access control is required.</remarks>
public interface IPermissionService
{
    /// <summary>
    /// Creates a new permission in the system asynchronously.
    /// </summary>
    /// <param name="permission">The <see cref="Permission"/> object representing the permission to be created. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the created
    /// permission.</returns>
    Task<Permission> CreatePermissionAsync(Permission permission);

    /// <summary>
    /// Asynchronously retrieves all available permissions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of Permission
    /// objects representing all available permissions.</returns>
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
}

