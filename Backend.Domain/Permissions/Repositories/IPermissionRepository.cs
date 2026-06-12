using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Repositories;

/// <summary>
/// Defines a contract for managing permissions in the system.
/// </summary>
/// <remarks>This interface provides methods for creating, retrieving, updating, and deleting permissions.
/// Implementations of this interface should ensure thread safety and proper handling of data persistence.</remarks>
public interface IPermissionRepository
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
