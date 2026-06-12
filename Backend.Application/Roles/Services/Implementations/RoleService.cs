using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services.Implementations;

/// <summary>
/// RoleService provides methods for managing role-related operations, including creating new roles.
/// </summary>
internal class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class with the specified role repository.
    /// </summary>
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Asynchronously creates a new role in the system.
    /// </summary>
    public async Task<Role> CreateRoleAsync(Role role)
    {
        return await _roleRepository.CreateRoleAsync(role);
    }

    /// <summary>
    /// Asynchronously associates a role with a permission in the system.
    /// </summary>
    public async Task<(Role, Permission)> AssociatePermissionAsync(
        Role role,
        Permission permission)
    {
        return await _roleRepository.AssociatePermissionAsync(role, permission);
    }

    /// <summary>
    /// Asynchronously retrieves all roles in the system.
    /// </summary>
    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        return await _roleRepository.GetRolesAsync();
    }

    /// <summary>
    /// Attempts to fetch a a pre-existing <see cref="Role"/> entity with a matching Id asynchronously
    /// </summary>
    /// <param name="id">The id to search for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found role (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<(Role? maybeRole, string? errorMessage)> GetRoleFromIdAsync(int id)
    {
        return await _roleRepository.GetRoleFromIdAsync(id);
    }

    /// <summary>
    /// Retrieves all <see cref="Permission"/> entities associated with a given id asynchronously.
    /// </summary>
    /// <param name="roleId">The id of the role whose permissions are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// permissions associated with the role (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetRolePermissionsAsync(int roleId)
    {
        return await _roleRepository.GetPermissionsFromRoleIdAsync(roleId);
    }
    /// <summary>
    /// Gets a role by its name asynchronously.
    /// </summary>
    /// <param name="roleName">Name of the role</param>
    /// <returns>A asynchorous Task representing the role</returns>
    /// <exception cref="RoleNotFoundException">When the role is not found</exception>
    public Task<Role> GetRoleByNameAsync(RoleName roleName)
    {
        return _roleRepository.GetRoleByNameAsync(roleName);
    }


    /// <summary>
    /// Gets a list of all users in the database matching a given query
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
    public Task<(IEnumerable<Role> Roles, int TotalCount)> SearchRolesAsync(string query, int pageNumber, int pageSize)
    {
        return _roleRepository.SearchRolesAsync(query, pageNumber, pageSize);
    }


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
    /// A tuple containing the paginated list of boards and the total count of boards:
    /// <list type="bullet">
    /// <item><description><see cref="IEnumerable{Board}"/>: The collection of boards for the specified page.</description></item>
    /// <item><description><see cref="int"/>: The total count of boards available.</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When either the page number or size are less than 0.
    /// </exception>
    public Task<(IEnumerable<Role> Roles, int TotalCount)> ListRolesPagedAsync(int pageNumber, int pageSize)
    {
        return _roleRepository.ListRolesPagedAsync(pageNumber, pageSize);
    }


}
