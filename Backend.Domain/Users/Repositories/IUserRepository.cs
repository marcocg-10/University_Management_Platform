using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Gets a list of all active users in the database
    /// </summary>
    /// <returns>A list of all active users in the database</returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<IEnumerable<User>> GetActiveUsersAsync();

    /// <summary>
    /// Attempts to create a user and save it in the database
    /// </summary>
    /// <returns>The number of users created and saved to the database</returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Associates a pre-existing <see cref="User"/> entity
    /// with a pre-existing <see cref="Role"> asynchronously.
    /// </summary>
    /// <param name="user">The user entity to be associated with the role.</param>
    /// <param name="role">The role entity to be associated with the user.</param>
   
    Task AssociateRoleAsync(User user, Role role);

    /// <summary>
    /// Fetches a a pre-existing <see cref="User"/> entity with a matching IdKey asynchronously
    /// </summary>
    /// <param name="idKey">The id to search for</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    Task<User> GetUserFromIdKeyAsync(int idKey);

    /// <summary>
    /// Fetches a a pre-existing <see cref="User"/> entity with a matching AzureObjectIdentifier asynchronously
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object ID</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    Task<User> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier);

    /// <summary>
    /// Retrieves all <see cref="Role"/> entities associated with a given <see cref="User"/> Id asynchronously.
    /// </summary>
    /// <param name="userId">The id of the user to retrieve roles for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Role"/> entities associated with the user (or null on error)
    /// and an optional error message.
    /// </returns>
    Task<IEnumerable<Role>> GetUserRolesAsync(int userId);

    /// <summary>
    /// Retrieves all <see cref="Permission"/> entities associated with a given <see cref="User"/> Id asynchronously.
    /// </summary>
    /// <param name="userId">The id of the user to retrieve permissions for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with a list of all <see cref="Permission"/> entities associated with the user (or null on error)
    /// and an optional error message.
    /// </returns>
    Task<IEnumerable<Permission>> GetCurrentUserPermissionsAsync(int userId);

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
    Task<(IEnumerable<User> Users, int TotalCount)> SearchUsersAsync(string query, int pageNumber, int pageSize);

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
    Task<(IEnumerable<User> Users, int TotalCount)> ListActiveUsersPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Persists a Ready Player Me <see cref="AvatarId"/> for the specified user.
    /// </summary>
    /// <param name="userId">The internal user IdKey to update.</param>
    /// <param name="avatarId">The validated avatar identifier value object.</param>
    Task SaveAvatarId(int userId, AvatarId avatarId);
}