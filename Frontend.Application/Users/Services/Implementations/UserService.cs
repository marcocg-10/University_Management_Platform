using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class with the specified user repository.
    /// </summary>
    /// <param name="userRepository">The repository used to manage user data. This parameter cannot be <see langword="null"/>.</param>
    /// <param name="authenticationStateProvider">The authentication state provider for accessing current user claims.</param>
    public UserService(IUserRepository userRepository, AuthenticationStateProvider authenticationStateProvider)
    {
        _userRepository = userRepository;
        _authenticationStateProvider = authenticationStateProvider;
    }

    /// <summary>
    /// Creates a new user with the specified details and adds it to the repository.
    /// </summary>
    /// <remarks>This method validates the provided email and name before creating the user. The user is then
    /// added to the repository asynchronously.</remarks>
    /// <param name="Id">The unique identifier for the user. This value must not be null or empty.</param>
    /// <param name="NameString">The name of the user. This value must not be null or empty.</param>
    /// <param name="IsActive">A value indicating whether the user is active. <see langword="true"/> if the user is active; otherwise, <see
    /// langword="false"/>.</param>
    /// <param name="EmailString">The email address of the user. This value must be a valid email format.</param>
    /// <returns>A <see cref="User"/> object representing the newly created user.</returns>
    public async Task<User> CreateUserAsync(string IdString, string NameString, bool IsActive, string EmailString)
    {
        // Create Email VO using fully qualified name to avoid any name collision.
        var emailVo = Email.Create(EmailString);
        var nameVo = UserName.Create(NameString);
        var IdVo = UserId.Create(IdString);
        var user = new User(IdVo, nameVo, IsActive, emailVo);
        await _userRepository.AddUserAsync(user);
        return user;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of active users.
    /// </summary>
    /// <remarks>This method queries the underlying data source to return all users who are currently marked
    /// as active. The returned collection may be empty if no active users are found.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of
    /// <see cref="User"/> objects representing the active users.</returns>
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _userRepository.GetActiveUsersAsync();
    }

    /// <summary>
    /// Associates a specified role with a user asynchronously.
    /// </summary>
    /// <param name="user">The user to whom the role will be associated. Cannot be <see langword="null"/>.</param>
    /// <param name="role">The role to associate with the user. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the association 
    /// if the operation is successful; otherwise, <see langword="null"/> if the association could not be created.</returns>
    public async Task<string?> AssociateRoleAsync(User user, Role role)
    {
        return await _userRepository.AssociateRoleAsync(user, role);
    }

    /// <summary>
    /// Retrieves the roles associated with the specified user.
    /// </summary>
    /// <param name="user">The user for whom to retrieve roles. Cannot be <see langword="null"/>.</param>
    /// <returns>A tuple containing the roles associated with the user and an error message, if any. <list type="bullet">
    /// <item><description><c>roles</c>: A collection of roles associated with the user, or <see langword="null"/> if an
    /// error occurs.</description></item> <item><description><c>errorMessage</c>: A message describing the error, or
    /// <see langword="null"/> if the operation is successful.</description></item> </list></returns>
    public async Task<(IEnumerable<Role>? roles, string? errorMessage)> GetUserRolesAsync(User user)
    {
        return await _userRepository.GetUserRolesAsync(user);
    }

    /// <summary>
    /// Retrieves a user by their Azure Object Identifier.
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier to search for.</param>
    /// <returns>A tuple containing the user and an error message, if any.</returns>
    public async Task<(User? maybeUser, string? errorMessage)> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier)
    {
        return await _userRepository.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);
    }

    /// <summary>
    /// Gets the current authenticated user from Azure AD claims.
    /// </summary>
    /// <returns>The current user or null if not found/authenticated.</returns>
    public async Task<User?> GetCurrentUserAsync()
    {
        try
        {
            // Get the current authentication state
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            
            // Check if user is authenticated
            if (authState.User?.Identity?.IsAuthenticated != true)
            {
                return null;
            }

            // Extract Azure Object Identifier from claims
            var azureObjectIdentifier = GetAzureObjectIdentifier(authState.User);
            if (string.IsNullOrEmpty(azureObjectIdentifier))
            {
                return null;
            }

            // Get user from repository using Azure Object Identifier
            var (user, errorMessage) = await GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);
            
            // Return the user (could be null if not found)
            return user;
        }
        catch (Exception)
        {
            // Log exception if needed and return null for any errors
            return null;
        }
    }

    /// <summary>
    /// Extracts the Azure Object Identifier from user claims.
    /// </summary>
    /// <param name="user">The claims principal representing the authenticated user.</param>
    /// <returns>The Azure Object Identifier or null if not found.</returns>
    private static string? GetAzureObjectIdentifier(ClaimsPrincipal user)
    {
        // Try multiple claim types in order of preference
        var claimTypes = new[]
        {
            "http://schemas.microsoft.com/identity/claims/objectidentifier", // Azure AD B2C primary claim
            ClaimTypes.NameIdentifier,                                       // Standard name identifier
            "oid",                                                          // Short form for object identifier
            "sub"                                                           // Subject claim (fallback)
        };

        foreach (var claimType in claimTypes)
        {
            var value = user.FindFirst(claimType)?.Value;
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
        }

        return null;
    }

    /// TODO: Update this documentation
    /// <summary>
    /// Gets a list of all users in the database matching a given query
    /// </summary>
    /// <returns>A list of all users in the database matching a given query</returns>
    /// <param name="query">The queried name to match against</param>
    public async Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> SearchUsersAsync(string query, int pageNumber, int pageSize)
    {
        return await _userRepository.SearchUsersAsync(query, pageNumber, pageSize);
    }

    /// <summary>
    /// Gets a paginated list of active users along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> ListActiveUsersPagedAsync(int pageNumber, int pageSize) { 

        return await _userRepository.ListActiveUsersPagedAsync(pageNumber, pageSize);
    }

}
