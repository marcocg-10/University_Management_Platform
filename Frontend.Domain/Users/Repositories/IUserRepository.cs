//using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Repositories;

/// <summary>
/// Defines a contract for accessing and managing user data in a data store.
/// </summary>
/// <remarks>This interface provides methods for retrieving and managing user-related information. Implementations
/// of this interface are responsible for interacting with the underlying data store to perform the specified
/// operations. The methods are designed to be asynchronous to support non-blocking I/O operations.</remarks>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to add the specified user. Ensure that the
    /// user details are valid before calling this method.</remarks>
    /// <param name="user">The user to add. The <see cref="User"/> object must not be null and should contain valid user details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddUserAsync(User user);

    /// <summary>
    /// Gets a list of all active users in the database
    /// </summary>
    /// <returns>A list of all active users in the database</returns>
    Task<IEnumerable<User>> GetActiveUsersAsync();

    /// <summary>
    /// Associates the specified role with the given user asynchronously.
    /// </summary>
    /// <param name="user">The user to whom the role will be associated. Cannot be <see langword="null"/>.</param>
    /// <param name="role">The role to associate with the user. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier  of the
    /// association if successful; otherwise, <see langword="null"/> if the operation fails.</returns>
    Task<string?> AssociateRoleAsync(User user, Role role);

    /// <summary>
    /// Asynchronously retrieves the roles associated with the specified user.
    /// </summary>
    /// <remarks>If the operation fails, the <c>roles</c> value will be <see langword="null"/> and
    /// <c>errorMessage</c> will contain a description of the error.</remarks>
    /// <param name="user">The user for whom to retrieve roles. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The result is a tuple containing: <list type="bullet"> <item>
    /// <description><see cref="IEnumerable{Role}"/> roles: The collection of roles associated with the user, or <see
    /// langword="null"/> if an error occurs.</description> </item> <item> <description><see cref="string"/>
    /// errorMessage: A message describing the error, or <see langword="null"/> if the operation is
    /// successful.</description> </item> </list></returns>
    Task<(IEnumerable<Role>? roles, string? errorMessage)> GetUserRolesAsync(User user);

    /// <summary>
    /// Fetches a pre-existing User entity with a matching AzureObjectIdentifier asynchronously
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object ID</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a tuple with the
    /// found user (null on error) and an optional message describing the failure.
    /// </returns>
    Task<(User? maybeUser, string? errorMessage)> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier);

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
    Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> SearchUsersAsync(string query, int pageNumber, int pageSize);



    /// <summary>
    /// Retrieves a paginated list of users along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the users for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// boards.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of users to include in each page. Must be greater than 0.</param>
    /// <returns>A tuple containing the users in the requested page and pagination metadata.</returns>
    Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> ListActiveUsersPagedAsync(int pageNumber, int pageSize);

}