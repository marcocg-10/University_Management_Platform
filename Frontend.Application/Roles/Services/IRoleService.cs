using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;


namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Roles.Services;

/// <summary>
/// Defines the contract for role operations in the application layer.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Asynchronously creates a new role entity.
    /// </summary>
    /// <param name="role">The <see cref="Role"/> object containing the details of the role to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the created <see cref="Role"/>
    /// </returns>
    Task<Role> CreateRoleAsync(Role role);

    /// <summary>
    /// Associates a pre-existing <see cref="Role"/> entity
    /// with a pre-existing <see cref="Permission"> asynchronously.
    /// </summary>
    /// <param name="role">The role entity to be associated with the permission.</param>
    /// <param name="permission">The permission entity to be associated with the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an optional error message describing the failure,
    /// or <c>null</c> if the association was successful.
    /// </returns>
    Task<string?> AssociatePermissionAsync(Role role, Permission permission);

    /// <summary>
    /// Retrieves all <see cref="Role"/> entities asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Role"/> entities
    /// </returns>
    Task<IEnumerable<Role>> GetRolesAsync();

    /// <summary>
    /// Retrieves all permissions associated with a specific <see cref="Role"/> entity asynchronously.
    /// </summary>
    /// <param name="role">The role entity whose permissions are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with an enumerable of all permissions associated with the role (an empty enumerable if no permissions exist, or null only on exception)
    /// and an optional error message.
    /// </returns>
    Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetRolePermissionsAsync(Role role);

    /// <summary>
    /// Gets a list of all users in the database matching a given query
    /// </summary>
    /// <returns>A list of all users in the database matching a given query</returns>
    /// <param name="query">The queried name to match against</param>
    Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> SearchRolesAsync(string query, int pageNumber, int pageSize);

    /// <summary>
    /// Gets a paginated list of active users along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> ListRolesPagedAsync(int pageNumber, int pageSize);
}

