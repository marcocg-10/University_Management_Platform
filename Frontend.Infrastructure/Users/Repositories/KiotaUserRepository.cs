using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Roles.Mappers;
using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Repositories;

/// <summary>
/// Provides methods for retrieving user data from an external API.
/// </summary>
/// <remarks>This repository interacts with an external API to fetch user information.  It implements the <see
/// cref="IUserRepository"/> interface to provide a consistent abstraction for user-related operations.</remarks>
internal class KiotaUserRepository : IUserRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaUserRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The <see cref="ApiClient"/> instance used to interact with the API.</param>
    public KiotaUserRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously adds a new user to the system.
    /// </summary>
    /// <param name="user">The user to be added. Must not be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is <see langword="null"/>.</exception>
    public async Task AddUserAsync(User user)
    {
        try
        {
            // Mapear la entidad de dominio del frontend a un DTO de API
            var userDto = user.ToDto();

            // Llamar al endpoint POST /api/users
            var response = await _apiClient.Users.PostAsync(userDto);

            // FIX: Extract UserDto from CreateUserResponse before mapping
            var createdUserDto = response?.AdditionalData != null && response.AdditionalData.TryGetValue("user", out var userObj) && userObj is UserDto dto
                ? dto
                : null;

            var createdUser = createdUserDto != null
                ? UserDtoMapper.ToEntity(createdUserDto)
                : null;

        }
        catch (ApiException ex)
        {
            throw ex.ResponseStatusCode switch
            {
                // 400 - bad requests
                400 when ex.Message.Contains("UserDataException", StringComparison.OrdinalIgnoreCase) =>
                    new UserDataException("The provided user data is invalid. Please review the form fields."),
                400 => new UserDataException(ex.Message),

                // 409 - conflict
                409 when ex.Message.Contains(user.Email.Value, StringComparison.OrdinalIgnoreCase) =>
                    new DuplicateEmailException(user.Email),
                
                409 when ex.Message.Contains(user.Id.Value, StringComparison.OrdinalIgnoreCase) =>
                   new DuplicateIdException(user.Id),
                
                409 when ex.Message.Contains(user.AzureObjectIdentifier!, StringComparison.OrdinalIgnoreCase) =>
                    new UserDataException($"The Azure Object Identifier '{user.AzureObjectIdentifier}' already exists."),

                // Internal server error
                500 => new UserException("An internal server error occurred while creating the user."),

                // Any other error
                _ => new UserException($"Unexpected API error while creating user: {ex.Message}")
            };
        }
        catch (UserDataException)
        {
            throw;
        }
        catch (DuplicateEmailException)
        {
            throw;
        }
        catch (DuplicateIdException)
        {
            throw;
        }
        catch (Exception ex)
        {
                throw new UserDataException ($"Error creating user: {ex.Message}");
        }
    }

    /// <summary>
    /// Asynchronously retrieves a collection of active users.
    /// </summary>
    /// <remarks>This method fetches the list of users from the underlying API and maps them to domain
    /// entities. If no users are available, an empty collection is returned.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  <IEnumerable{T}> of <User>
    /// objects representing the active users.</returns>
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        var response = await _apiClient.Users.GetAsync();

        IEnumerable<User> users;

        try
        {
            users = response?.Users?.Select(UserDtoMapper.ToIdEntity)
                ?? [];
        }
        catch (ApiException ex)
        {
            throw ex.ResponseStatusCode switch
            {
                _ => new Exception($"API error occurred while listing users: {ex.Message}")
            };
        }
        catch (Exception ex)
        {
            throw new ListUsersException($"Error listing users: {ex.Message}");
        }

        return users;
    }

    /// <summary>
    /// Associates the specified role with the given user asynchronously.
    /// </summary>
    /// <param name="user">The user to whom the role will be associated. Cannot be <see langword="null"/>.</param>
    /// <param name="role">The role to associate with the user. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier  of the
    /// association if successful; otherwise, <see langword="null"/> if the operation fails.</returns>
    public async Task<string?> AssociateRoleAsync(User user, Role role)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), "user cannot be null.");
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role cannot be null.");

        try
        {
            var request = new RoleDto
            {
                Name = role.Name.Value
            };
            await _apiClient.AsignRoleToUser[user.IdKey.ToString()].PutAsync(request);
            
            return null;
        }
        catch (ApiException ex)
        {
           throw ex.ResponseStatusCode switch
           {
               404 when ex.Message.Contains("user", StringComparison.OrdinalIgnoreCase) =>
                   new UserNotFoundException(user.Id),
               404 when ex.Message.Contains("role", StringComparison.OrdinalIgnoreCase) =>
                   new AssignableRoleNotFoundException(role.Name),
               409 => new RoleAlreadyAssignedException(user.Id, role.Name),
               400 when ex.Message.Contains("role", StringComparison.OrdinalIgnoreCase) =>
                   new RoleInvalidDataException($"Invalid role data provided for role {role.Name.Value}."),
               400 => new Exception($"Invalid role data provided for role {role.Name.Value}."),
               500 => new UserException("An internal server error occurred while associating the role."),   
               _ => new Exception($"API error occurred while associating role: {ex.Message}")
           };
        }
        catch (Exception ex)
        {
            return $"Error associating permission: {ex.Message}";
        }
    }

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
    public async Task<(IEnumerable<Role>? roles, string? errorMessage)> GetUserRolesAsync(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        try
        {
            var response = await _apiClient.Users[user.IdKey.ToString()].Roles.GetAsync();
            var roles = response?.Roles?.Select(RoleDtoMapper.toRolIdEntity)
                ?? Enumerable.Empty<Role>();

            return (roles, null);
        }
        catch (Exception ex)
        {
            return (null, $"Error getting roles for user {user.Name.Value}: {ex.Message}");
        }
    }

    /// <summary>
    /// Fetches a pre-existing User entity with a matching AzureObjectIdentifier asynchronously.
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object ID.</param>
    /// <returns>A tuple with the found user (null on error) and an optional message.</returns>
    public async Task<(User? maybeUser, string? errorMessage)> GetUserByAzureObjectIdentifierAsync(string azureObjectIdentifier)
    {
        if (string.IsNullOrEmpty(azureObjectIdentifier))
            return (null, "Azure Object Identifier cannot be null or empty");

        try
        {
            var response = await _apiClient.Users.Oid[azureObjectIdentifier].GetAsync();

            if (response?.User == null)
                return (null, null); // Not found, but not an error

            var user = UserDtoMapper.ToIdEntity(response.User);
            return (user, null);
        }
        catch (Microsoft.Kiota.Abstractions.ApiException apiEx) when (apiEx.ResponseStatusCode == 404)
        {
            // User not found is not an error, just return null
            return (null, null);
        }
        catch (Microsoft.Kiota.Abstractions.ApiException apiEx) when (apiEx.ResponseStatusCode == 400)
        {
            return (null, "Invalid Azure Object Identifier format");
        }
        catch (Exception ex)
        {
            return (null, $"Error searching for user with Azure Object Identifier {azureObjectIdentifier}: {ex.Message}");
        }
    }

    /// TODO: Update this documentation
    /// <summary>
    /// Gets a list of all users in the database matching a given query
    /// </summary>
    /// <returns>A list of all users in the database matching a given query</returns>
    /// <param name="query">The queried name to match against</param>
    public async Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> SearchUsersAsync(
        string query,
        int pageNumber,
        int pageSize)
    {
        try
        {
            // Kiota generated builder accepts a configuration action where we set the query parameter
            var response = await _apiClient.SearchUsers.GetAsync(cfg =>
                {
                cfg.QueryParameters.Name = query;
                cfg.QueryParameters.PageNumber = pageNumber;
                cfg.QueryParameters.PageSize = pageSize;
                }).ConfigureAwait(false);

            var users = response?.Users?.Select(UserDtoMapper.ToEntity) ?? Enumerable.Empty<User>();
            var md = response?.Metadata;

            var currentPage = md?.CurrentPage ?? pageNumber;
            var size = md?.PageSize ?? pageSize;
            var totalCount = md?.TotalCount ?? 0;
            var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

            var metadata = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = size,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return (users, metadata);
        }
        catch (Microsoft.Kiota.Abstractions.ApiException apiEx)
        {
            // map common API failures to domain-friendly exception
            throw apiEx.ResponseStatusCode switch
            {
                500 => new UserException("An internal server error occurred while searching users."),
                _ => new UserException($"API error occurred while searching users: {apiEx.Message}")
            };
        }
        catch (Exception ex)
        {
            throw new UserException($"An error occurred while searching users: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Retrieves a paginated list of users along with pagination metadata.
    /// </summary>
    /// <remarks>This method queries the underlying API to retrieve the users for the specified page. If the
    /// requested page number exceeds the total number of pages, the method will return an empty collection of
    /// boards.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of users to include in each page. Must be greater than 0.</param>
    /// <returns>A tuple containing the users in the requested page and pagination metadata.</returns>
    public async Task<(IEnumerable<User> Users, PaginationMetadata Metadata)> ListActiveUsersPagedAsync(int pageNumber, int pageSize)
    {
        var response = await _apiClient.Users.Paginated.GetAsync(c =>
        {
            c.QueryParameters.PageNumber = pageNumber;
            c.QueryParameters.PageSize = pageSize;
        }).ConfigureAwait(false);

        var users = response?.Users?.Select(UserDtoMapper.ToEntity) ?? Enumerable.Empty<User>();
        var md = response?.Metadata;

        var currentPage = md?.CurrentPage ?? pageNumber;
        var size = md?.PageSize ?? pageSize;
        var totalCount = md?.TotalCount ?? 0;
        var totalPages = md?.TotalPages ?? (size > 0 ? (int)System.Math.Ceiling(totalCount / (double)size) : 0);

        var metadata = new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = size,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return (users, metadata);
    }
}
