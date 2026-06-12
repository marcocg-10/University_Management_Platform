using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Permissions.Repositories;

/// <summary>
/// Initializes a new instance of the <see cref="PermissionRepository"/> class with the specified database context.
/// </summary>
internal class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Attempts to create a permission and save it in the database
    /// </summary>
    public async Task<Permission> CreatePermissionAsync(Permission permission)
    {
        try
        {
             await _dbContext.Permissions.AddAsync(permission);
             await _dbContext.SaveChangesAsync();
        } 
        catch (DbUpdateException ex) // TODO: add IsUniqueConstraintViolation exception
        {
            throw new PermissionAlreadyExistsException(permission.Name);
        } 
        catch (Exception)
        {
            throw new PermissionInvalidDataException("Error trying to create a new permission");
        }
        return permission;
        
    }

    /// <summary>
    /// Asynchronously retrieves all permissions from the database.
    /// </summary>
    /// <remarks>This method queries the database to retrieve all permissions and returns them as a
    /// collection.  The result will be an empty collection if no permissions are found.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IEnumerable{T} of Permission
    /// objects representing all permissions.</returns>
    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _dbContext.Permissions
            .ToListAsync();
    }
}
