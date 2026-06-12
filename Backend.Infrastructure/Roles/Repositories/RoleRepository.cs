using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Roles.Repositories;

/// <summary>
/// Repository for managing Role entities in the database.
/// </summary>
internal class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Attempts to create a role and save it in the database
    /// </summary>
    /// <param name="role">The role to create.</param>
    /// <returns>The created role.</returns>
    public async Task<Role> CreateRoleAsync(Role role)
    {
        await _dbContext.Roles.AddAsync(role);

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(
            () => _dbContext.SaveChangesAsync());

        return role;
    }

    /// <summary>
    /// Attempts to associate a pre-existing <see cref="Role"/> entity
    /// with a pre-existing <see cref="Permission"> asynchronously. 
    /// </summary>
    /// <param name="role">The role entity to be associated with the permisssion.</param>
    /// <param name="permission">The permission entity to be associated with the role.</param>
    /// <returns>A tuple containing the role and permission</returns>
    public async Task<(Role, Permission)> AssociatePermissionAsync(Role role, Permission permission)
    {
        Role? maybeRole = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            maybeRole = await _dbContext.Roles
                .Include(r => r.Permissions)
                .SingleOrDefaultAsync(r => r.Name == role.Name);
            });
        if (maybeRole is null)
            throw new RoleNotFoundException(role.Name);

        Permission? maybePermission = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
                maybePermission = await _dbContext.Permissions
                    .SingleOrDefaultAsync(p => p.Name == permission.Name);
            });      

        if (maybePermission is null)
            throw new AssignablePermissionNotFoundException(permission.Name);

        if (maybeRole.Permissions.Any(p => p.Name == maybePermission.Name))
        {
            throw new PermissionAlreadyAssignedException(maybePermission.Name, maybeRole.Name);
        }

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            maybeRole.Permissions.Add(maybePermission);
            await _dbContext.SaveChangesAsync();
            });

        return (role, permission);
    }

    /// <summary>
    /// Retrieves all <see cref="Role"/> entities asynchronously.
    /// </summary>
    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        var roles = new List<Role>();

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
        {
            roles = await _dbContext.Roles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Include(r => r.Permissions)
                .ToListAsync();
        });
        return roles;
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
        Role? maybeRole;
        try
        {
            maybeRole = await _dbContext.Roles
                .SingleOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception ex)
        {
            return (null, $"Error searching for role with id {id}");
        }
        if (maybeRole is null)
            throw new RoleNotFoundException(id);

        return (maybeRole, null);
    }

    /// <summary>
    /// Attempts to fetch a the set of <see cref="Permission"/> entities assigned to a matching role Id asynchronously
    /// </summary>
    /// <param name="roleID">The role id from which the permissions are retrieved</param>
    /// <returns>A task containing a tuple with a list of permissions (or null on error) and an optional error message.</returns>
    public async Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetPermissionsFromRoleIdAsync(int roleID)
    {
        // Validate that role exists
        var (maybeRole, roleError) = await GetRoleFromIdAsync(roleID);
        if (maybeRole is null)
            return (null, roleError);
        try
        {
            var permissions = await _dbContext.Roles
                .AsNoTracking()
                .Include(r => r.Permissions)
                .Where(r => r.Id == roleID)
                .SelectMany(r => r.Permissions)
                .ToListAsync();
            
            return (permissions, null);
        }
        catch (Exception ex)
        {
            return (null, $"Error retrieving permissions for role with id {roleID}");
        }
    }
    
    /// <summary>
    /// Gets a role by its name asynchronously.
    /// </summary>
    /// <param name="roleName">Name of the role</param>
    /// <returns>A asynchorous Task representing the role</returns>
    /// <exception cref="RoleNotFoundException">When the role is not found</exception>
    public async Task<Role> GetRoleByNameAsync(RoleName roleName)
    {
        Role? maybeRole = null;
        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
        {
            maybeRole = await _dbContext.Roles
                .SingleOrDefaultAsync(r => r.Name.Equals(roleName));
        });

        return maybeRole ?? throw new RoleNotFoundException(roleName);
    }

    public async Task<(IEnumerable<Role> Roles, int TotalCount)> SearchRolesAsync(string query, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var roles = _dbContext.Roles;

        // Get total count before applying pagination
        var totalCount = await roles.CountAsync();

        var result = await roles
            .OrderBy(role => role.Name)
            .ThenBy(role => role.Id)
            .Where(role => ((string)role.Name).Contains(query))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (result, totalCount);
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
    public async Task<(IEnumerable<Role> Roles, int TotalCount)> ListRolesPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.Roles;

        // Get total count before applying pagination
        var totalCount = await query.CountAsync();

        // Retrieve paged results
        var roles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (roles, totalCount);
    }

}
