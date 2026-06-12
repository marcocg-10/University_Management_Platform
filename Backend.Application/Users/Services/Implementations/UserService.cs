using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services.Implementations;

/// <summary>
/// Provides methods for managing user-related operations, including retrieving active users and creating new users.
/// This service acts as an abstraction layer between the application and the underlying user repository.
/// </summary>
internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Gets and sets an abstract version of the entity as a parameter
    /// </summary>
    /// <param name="userRepository"></param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Service implementation of the method to get active users
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return _userRepository.GetActiveUsersAsync();
    }

    /// <summary>
    /// Asynchronously creates a new user in the system.
    /// </summary>
    /// <param name="user">The <see cref="User"/> object containing the details of the user to be created. Cannot be <see
    /// langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the newly
    /// created user.</returns>
    public Task<User> CreateUserAsync(User user)
    {
        return _userRepository.CreateUserAsync(user);
    }

    /// <summary>
    /// Retrieves a user by their Azure Object Identifier.
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier to search for.</param>
    /// <returns>The user if found, otherwise null.</returns>
    public Task<User> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier)
    {
        return _userRepository.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);
    }

    /// Associates a pre-existing <see cref="User"/> entity
    /// with a pre-existing <see cref="Role"> asynchronously.
    /// </summary>
    /// <param name="user">The user entity to be associated with the role.</param>
    /// <param name="role">The role entity to be associated with the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// amount of changes saved to the database (null on error) and an optional message describing the failure.
    /// </returns>
    public Task AssociateRoleAsync(User user, Role role)
    {
        return _userRepository.AssociateRoleAsync(user, role);
    }

    /// <summary>
    /// Fetches a a pre-existing <see cref="User"/> entity with a matching IdKey asynchronously
    /// </summary>
    /// <param name="idKey">The id to search for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    public Task<User> GetUserFromIdKeyAsync(int idKey)
    {
        return _userRepository.GetUserFromIdKeyAsync(idKey);
    }

    public Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
        return _userRepository.GetUserRolesAsync(userId);
    }

    /// <summary>
    /// Retrieves all <see cref="Permission"/> entities associated with a given id asynchronously.
    /// </summary>
    /// <param name="userId">The id of the user whose permissions are to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Permission"/> entities associated with the user (or null on error)
    /// and an optional error message.
    /// </returns>
    public Task<IEnumerable<Permission>> GetCurrentUserPermissionsAsync(int userId)
    {
        return _userRepository.GetCurrentUserPermissionsAsync(userId);
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
    public Task<(IEnumerable<User> Users, int TotalCount)> SearchUsersAsync(string query, int pageNumber, int pageSize)
    {
        return _userRepository.SearchUsersAsync(query, pageNumber, pageSize);
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
    public Task<(IEnumerable<User> Users, int TotalCount)> ListActiveUsersPagedAsync(int pageNumber, int pageSize)
    {
        return _userRepository.ListActiveUsersPagedAsync(pageNumber, pageSize);
    }

    /// <summary>
    /// Saves the avatar ID for a given user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="avatarId"></param>
    /// <returns></returns>
    public Task SaveAvatarId(int userId, AvatarId avatarId)
    {
        return _userRepository.SaveAvatarId(userId, avatarId);
    }
}
