using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The database context used to interact with the application's data store. Cannot be null.</param>
    public UserRepository(AppDbContext dbContext) {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets a list of all active users in the database
    /// </summary>
    /// <returns>A list of all active users in the database</returns>
    public async Task <IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _dbContext.Users
            .Where(user => user.IsActive)
            .OrderBy(user => user.Name)
            .ThenBy(user => user.Id)
            .ToListAsync();
    }

    /// <summary>
    /// Attempts to create a user and save it in the database
    /// </summary>
    /// <remarks>
    /// This method respects the table's constraints, so it will filter
    /// against duplicate email addresses, duplicate IDs, and duplicate Azure Object Identifiers.
    /// </remarks>
    /// <returns>The created user</returns>
    public async Task<User> CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(
            () => _dbContext.SaveChangesAsync());

        return user;

    }

    /// <summary>
    /// Attempts to associate a pre-existing <see cref="User"/> entity
    /// with a pre-existing <see cref="Role"/> asynchronously.
    /// </summary>
    /// <remarks>
    /// Uses a fetch-and-update pattern to associate the fetched user with the fetched role.
    /// </remarks>
    /// <param name="user">The user entity to be associated with the role.</param>
    /// <param name="role">The role entity to be associated with the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// amount of changes saved to the database (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<(User, Role)> AssociateRoleAsync(User user, Role role)
    {
        User? maybeUser = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            maybeUser = await _dbContext.Users
                .Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Id == user.Id);
            });

        if (maybeUser is null)
            throw new UserNotFoundException(user.Id);

        Role? maybeRole = null;
        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            maybeRole = await _dbContext.Roles
                .Include(r => r.Permissions)
                .SingleOrDefaultAsync(r => r.Name == role.Name);
            });
       

        if (maybeRole is null)
            throw new AssignableRoleNotFoundException(role.Name);

        if (maybeUser.Roles.Any(p => p.Name == maybeRole.Name))
        {
            throw new RoleAlreadyAssignedException(maybeUser.Id, maybeRole.Name);
        }

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            maybeUser.Roles.Add(maybeRole);
            await _dbContext.SaveChangesAsync();
            });

        return (user, role);
    }

    /// <summary>
    /// Attempts to fetch a a pre-existing <see cref="User"/> entity with a matching IdKey asynchronously
    /// </summary>
    /// <param name="idKey">The id to search for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<User> GetUserFromIdKeyAsync(int idKey)
    {
        User? user = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdKey == idKey);
            });

        if (user is null)
            throw new UserNotFoundException(idKey);

        return user;
    }

    /// <summary>
    /// Gets a user by their Azure Object Identifier.
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier to search for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<User> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier)
    {
        if (azureObjectIdentifier is null)
        {
            return null;
        }
        
        User? user = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
            {
            user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.AzureObjectIdentifier == azureObjectIdentifier);
            });

        return user;
   
    }

    /// <summary>
    /// Gets the roles associated with a user by their user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve roles for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found roles (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
        try 
        {
            return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .Where(x => x.IdKey == userId)
            .SelectMany(u => u.Roles)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new UserException($"An error occurred while retrieving user roles: {ex.Message}", ex);
        }
      
    }

    /// <summary>
    /// Gets the permissions associated with a user by their user ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve permissions for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found roles (null on error) and an optional message describing the failure.
    /// </returns>
    public async Task<IEnumerable<Permission>> GetCurrentUserPermissionsAsync(int userId)
    {
        // Get all permissions for the user through their roles
        try
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(u => u.IdKey == userId)
                .SelectMany(u => u.Roles)                    // Get all roles for the user
                .SelectMany(r => r.Permissions)              // Get all permissions from those roles
                .Distinct()                                  // Remove duplicate permissions
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new UserException($"An error occurred while retrieving user permissions: {ex.Message}", ex);
        }

    }

    Task IUserRepository.AssociateRoleAsync(User user, Role role)
    {
        return AssociateRoleAsync(user, role);
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
    public async Task<(IEnumerable<User> Users, int TotalCount)> SearchUsersAsync(string query, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var users = _dbContext.Users;

        // Get total count before applying pagination
        var totalCount = await users.CountAsync();

        // TODO: For anyone brave enough to try to make this code cleaner, remove
        // the cast to string while retaining functionality ;)
        var result = await users
            .OrderBy(user => user.Name)
            .ThenBy(user => user.Id)
            .Where(user => ((string)user.Name).Contains(query) || ((string)user.Id).Contains(query) || ((string)user.Email).Contains(query))
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
    public async Task<(IEnumerable<User> Users, int TotalCount)> ListActiveUsersPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.Users;

        // Get total count before applying pagination
        var totalCount = await query.CountAsync();

        // Retrieve paged results
        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }

    /// <summary>
    /// Saves the avatar ID for a given user.
    /// </summary>
    /// <param name="idKey">The ID of the user to save the avatar ID for.</param>
    /// <param name="avatarId">The AvatarId to save.</param>
    public async Task SaveAvatarId(int idKey, AvatarId avatarId)
    {
        User? user = null;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
        {
            user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.IdKey == idKey);
        });

        if (user is null)
            throw new UserNotFoundException(idKey);

        user.AvatarId = avatarId;

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(async () =>
        {
            await _dbContext.SaveChangesAsync();
        });

        return;
    }
}