using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;

/// <summary>
/// Defines a contract for managing permissions in the system.
/// </summary>
/// <remarks>This interface provides methods for creating and retrieving permissions. Implementations of this
/// interface are expected to handle the underlying data storage and retrieval mechanisms for permissions.</remarks>
public interface IPermissionRepository
{
    /// ToDo: add Create method

    /// <summary>
    /// Asynchronously retrieves all available permissions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of Permission
    /// objects representing all available permissions.</returns>
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    /// <summary>
    /// Gets all permissions for a user by their Idl.
    /// </summary>
    /// <param name="azureObjectIdentifier">The User ID.</param>
    /// <returns>A collection of permission names the user has.</returns>
    Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetCurrentUserPermissionsAsync(string azureObjectIdentifier);
}
