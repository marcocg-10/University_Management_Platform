using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Repositories;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;


namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Roles.Services.Implementations;

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
    public async Task<string?> AssociatePermissionAsync(
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
    /// Retrieves all permissions associated with a specific <see cref="Role"/> entity asynchronously.
    /// </summary>
    /// <param name="role">The role entity whose permissions are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all permissions associated with the role (or null on error)
    /// and an optional error message.
    /// </returns>
    /// 
    public async Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetRolePermissionsAsync(Role role)
    {
        return await _roleRepository.GetRolePermissionsAsync(role);
    }

    /// TODO: Update this documentation
    /// <summary>
    /// Gets a list of all users in the database matching a given query
    /// </summary>
    /// <returns>A list of all users in the database matching a given query</returns>
    /// <param name="query">The queried name to match against</param>
    public async Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> SearchRolesAsync(string query, int pageNumber, int pageSize)
    {
        return await _roleRepository.SearchRolesAsync(query, pageNumber, pageSize);
    }

    /// <summary>
    /// Gets a paginated list of active users along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> ListRolesPagedAsync(int pageNumber, int pageSize)
    {

        return await _roleRepository.ListRolesPagedAsync(pageNumber, pageSize);
    }
}

