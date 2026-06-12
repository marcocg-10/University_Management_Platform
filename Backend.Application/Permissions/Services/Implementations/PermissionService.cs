using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services.Implementations;

/// <summary>
/// Provides functionality for managing permissions within the system.
/// </summary>
/// <remarks>This service acts as an intermediary between the application and the underlying data repository for
/// permissions. It provides methods to create, retrieve, and manage permissions asynchronously. </remarks>
internal class PermissionService : IPermissionService
{

    /// <summary>
    /// Represents the repository used to manage and retrieve permissions.
    /// </summary>
    /// <remarks>This field is read-only and is intended to store a dependency injected implementation of 
    /// <see cref="IPermissionRepository"/>. It is used internally to perform operations related to
    /// permissions.</remarks>
    private readonly IPermissionRepository _permissionRepository;

    /// <summary>
    /// Gets and sets an abstract version of the entity as a parameter
    /// </summary>
    /// <param name="permissionRepository"></param>
    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    /// <summary>
    /// Asynchronously creates a new permission in the system.
    /// </summary>
    /// <param name="permission">The <see cref="Permission"/> object representing the permission to be created. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the newly
    /// created permission.</returns>
    public Task<Permission> CreatePermissionAsync(Permission permission)
    {
        return _permissionRepository.CreatePermissionAsync(permission);
    }

    /// <summary>
    /// Asynchronously retrieves all available permissions.
    /// </summary>
    /// <remarks>This method retrieves all permissions from the underlying data source. The returned
    /// collection  may be empty if no permissions are available.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of Permission
    /// objects representing all available permissions.</returns>
    public Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return _permissionRepository.GetAllPermissionsAsync();
    }
}
