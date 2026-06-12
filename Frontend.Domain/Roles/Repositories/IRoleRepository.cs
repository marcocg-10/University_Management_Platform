using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Role"/> entities.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Persists a new <see cref="Role"/> entity asynchronously.
    /// </summary>
    /// <param name="role">The role entity to create.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the created <see cref="Role"/>
    /// </returns>
    Task<Role> CreateRoleAsync(Role role);

    /// <summary>
    /// Associates a pre-existing <see cref="Role"/> entity
    /// with a pre-existing <see cref="Permission"> asynchronously.
    /// </summary>
    /// <param name="role">The role entity to be associated with the permisssion.</param>
    /// <param name="permission">The permission entity to be associated with the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an optional message describing the failure.
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
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all permissions associated with the role (an empty enumerable if none are found, or null on error)
    /// and an optional error message.
    /// </returns>
    Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetRolePermissionsAsync(Role role);

    /// TODO: Update this documentation
    /// <summary>
    /// Asynchronously retrieves the permissions associated with the specified user.
    /// </summary>
    /// <remarks>If the operation fails, the <c>permissions</c> value will be <see langword="null"/> and
    /// <c>errorMessage</c> will contain a description of the error.</remarks>
    /// <param name="user">The user for whom to retrieve permissions. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The result is a tuple containing: <list type="bullet"> <item>
    /// <description><see cref="IEnumerable{Role}"/> permissions: The collection of permissions associated with the user, or <see
    /// langword="null"/> if an error occurs.</description> </item> <item> <description><see cref="string"/>
    /// errorMessage: A message describing the error, or <see langword="null"/> if the operation is
    /// successful.</description> </item> </list></returns>
    Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> SearchRolesAsync(string query, int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paginated list of users along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the users for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// boards.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of users to include in each page. Must be greater than 0.</param>
    /// <returns>A tuple containing the users in the requested page and pagination metadata.</returns>
    Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> ListRolesPagedAsync(int pageNumber, int pageSize);
}