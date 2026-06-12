using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;

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
    /// A task that represents the asynchronous operation. The task result contains the created <see cref="Role"/> entity.
    /// </returns>
    Task<Role> CreateRoleAsync(Role role);

    /// <summary>
    /// Associates a pre-existing <see cref="Role"/> entity
    /// with a pre-existing <see cref="Permission"> asynchronously.
    /// </summary>
    /// <param name="role">The role entity to be associated with the permisssion.</param>
    /// <param name="permission">The permission entity to be associated with the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result containsa tuple Role and Permission entities
    /// </returns>
    Task<(Role, Permission)> AssociatePermissionAsync(Role role, Permission permission);

    /// <summary>
    /// Retrieves all <see cref="Role"/> entities asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Role"/> entities
    /// </returns>
    Task<IEnumerable<Role>> GetRolesAsync();

    /// <summary>
    /// Attempts to fetch a a pre-existing <see cref="Role"/> entity with a matching Id asynchronously
    /// </summary>
    /// <param name="id">The id to search for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found role (null on error) and an optional message describing the failure.
    /// </returns>
    public Task<(Role? maybeRole, string? errorMessage)> GetRoleFromIdAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Permission"/> entities associated with a given <see cref="Role"/> Id asynchronously.
    /// </summary>
    /// <param name="roleId">The id of the role to retrieve permissions for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Permission"/> entities associated with the role (or null on error)
    /// and an optional error message.
    /// </returns>
    public Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetPermissionsFromRoleIdAsync(int roleId);
    
    /// <summary>
    /// Gets a role by its name asynchronously.
    /// </summary>
    /// <param name="roleName">Name of the role</param>
    /// <returns>A asynchorous Task representing the role</returns>
    /// <exception cref="RoleNotFoundException">When the role is not found</exception>
    public Task<Role> GetRoleByNameAsync(RoleName roleName);


    /// <summary>
    /// Gets a list of all roles in the database matching a given query
    /// </summary>
    /// <param name="query">The queried name to match against</param>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than zero.</param>
    /// <returns>
    /// A tuple containing the paginated list of users and the total count of users matching the query:
    /// <list type="bullet">
    /// <item><description><see cref="IEnumerable{Board}"/>: The collection of users for the specified page.</description></item>
    /// <item><description><see cref="int"/>: The total count of users available.</description></item>
    /// </list>
    /// </returns>
    /// /// <exception cref="ArgumentOutOfRangeException">
    /// When either the page number or size are less than 0.
    /// </exception>
    Task<(IEnumerable<Role> Roles, int TotalCount)> SearchRolesAsync(string query, int pageNumber, int pageSize);


    /// <summary>
    /// Retrieves a paginated list of users along with the total count of available users.
    /// </summary>
    /// <remarks>
    /// This method queries the database to retrieve the specified page of users. The total count is
    /// calculated before applying pagination, allowing the caller to determine the total number of pages
    /// available.
    /// </remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than zero.</param>
    /// <returns>
    /// A tuple containing the paginated list of users and the total count of users:
    /// <list type="bullet">
    /// <item><description><see cref="IEnumerable{Board}"/>: The collection of users for the specified page.</description></item>
    /// <item><description><see cref="int"/>: The total count of users available.</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When either the page number or size are less than 0.
    /// </exception>
    Task<(IEnumerable<Role> Roles, int TotalCount)> ListRolesPagedAsync(int pageNumber, int pageSize);
}