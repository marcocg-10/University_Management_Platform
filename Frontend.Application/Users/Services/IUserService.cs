using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services;

public interface IUserService
{

    Task<User> CreateUserAsync(string Id, string Name, bool IsActive, string EmailString);

    /// <summary>
    /// Asynchronously retrieves a collection of users who are currently active.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IEnumerable{T}"/> of
    /// <see cref="User"/> objects representing the active users.  The collection will be empty if no users are active.</returns>
    Task<IEnumerable<User>> GetActiveUsersAsync();

    /// <summary>
    /// Associates a specified role with a given user asynchronously.
    /// </summary>
    /// <remarks>This method performs the association asynchronously and may involve external systems or
    /// databases.  Ensure that both the user and role objects are valid and properly initialized before calling this
    /// method.</remarks>
    /// <param name="user">The user to whom the role will be associated. Cannot be null.</param>
    /// <param name="role">The role to associate with the user. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a tuple containing: <list type="bullet">
    /// <item> <description><c>status</c>: An integer representing the status of the operation, or <c>null</c> if the
    /// operation failed.</description> </item> <item> <description><c>errorMessage</c>: A string containing an error
    /// message if the operation failed, or <c>null</c> if it succeeded.</description> </item> </list></returns>
    Task<string?> AssociateRoleAsync(User user, Role role);

    /// <summary>
    /// Retrieves a user based on their Azure Object Identifier.
    /// </summary>
    /// <remarks>This method is asynchronous and should be awaited. Ensure the provided Azure Object
    /// Identifier is valid and corresponds to an existing user.</remarks>
    /// <param name="azureObjectIdentifier">The unique Azure Object Identifier of the user to retrieve.</param>
    /// <returns>A tuple containing the user and an error message. The first item is the user if found, or <see langword="null"/>
    /// if no user is found. The second item is an error message if an error occurred, or <see langword="null"/> if the
    /// operation was successful.</returns>
    Task<(User? maybeUser, string? errorMessage)> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier);


    Task<(IEnumerable<Role>? roles, string? errorMessage)> GetUserRolesAsync(User user);

    /// <summary>
    /// Gets the current authenticated user from Azure AD claims.
    /// </summary>
    /// <returns>The current user or null if not found/authenticated.</returns>
    Task<User?> GetCurrentUserAsync();


    /// <summary>
    /// Gets a list of all users in the database matching a given query
    /// </summary>
    /// <returns>A list of all users in the database matching a given query</returns>
    /// <param name="query">The queried name to match against</param>
    Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> SearchUsersAsync(string query, int pageNumber, int pageSize);


    /// <summary>
    /// Gets a paginated list of active users along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> ListActiveUsersPagedAsync(int pageNumber, int pageSize);

}
