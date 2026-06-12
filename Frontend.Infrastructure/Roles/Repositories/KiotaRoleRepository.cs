using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Permissions.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Mappers;
using PaginationMetadata = UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata.PaginationMetadata;


namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Roles.Repositories;

internal class KiotaRoleRepository : IRoleRepository
{
    private readonly ApiClient _apiClient;

    public KiotaRoleRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<string?> AssociatePermissionAsync(Role role, Permission permission)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role cannot be null.");
        if (permission is null)
            throw new ArgumentNullException(nameof(permission), "Permission cannot be null.");
        try
        {
            var request = new PermissionDto
            {
                Name = permission.Name.Value
            };

            await _apiClient.AssignPermissionToRole[role.Id].PutAsync(request);

            return null;
        }
        catch (ApiException apiEx)
        {
            throw apiEx.ResponseStatusCode switch
            {
                404 when apiEx.Message.Contains("role", StringComparison.OrdinalIgnoreCase) =>
                    new RoleNotFoundException(role.Name),
                404 when apiEx.Message.Contains("permission", StringComparison.OrdinalIgnoreCase) =>
                    new AssignablePermissionNotFoundException(permission.Name),
                404 => new RoleNotFoundException(role.Name),
                409 => new PermissionAlreadyAssignedException(permission.Name, role.Name),
                500 => new RoleException("An internal server error occurred while assigning the permission"),
                _ => new RoleException($"An unexpected error occurred (HTTP {apiEx.ResponseStatusCode}): {apiEx.Message}")
            };
        }
        catch (Exception ex)
        {
            return $"Error associating permission: {ex.Message}";
        }
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        var roleRequest = new RoleDto
        {
            Name = role.Name.Value
        };

        var request = new CreateRoleRequest
        {
            Role = roleRequest
        };

        try
        {
            var response = await _apiClient.Roles.PostAsync(request);

            var createdRoleDto = response?.Role ?? throw new RoleException("No role returned from API");
            return RoleDtoMapper.toRolIdEntity(createdRoleDto);
        }
        catch (ConflictErrorResponse)
        {
            throw new RoleAlreadyExistsException(role.Name);
        }
        catch (ValidationErrorResponse)
        {
            throw new RoleException("An internal error occurred while creating the role, please try later");
        }
        catch (ExceptionResult)
        {
            throw new RoleException("An internal error occurred while creating the role, please try later");
        }
    }

    public async Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetRolePermissionsAsync(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role cannot be null.");

        try
        {
            var response = await _apiClient.Roles[role.Id].Permissions.GetAsync();
            var permissions = response?.Permissions?.Select(x => PermissionDtoMapper.ToEntity(x))
                ?? Enumerable.Empty<Permission>();

            return (permissions, null);
        }
        catch (Exception ex)
        {
            return (null, $"Error getting permissions for role {role.Name.Value}: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        try
        {
            var response = await _apiClient.Roles.GetAsync();

            var roles = response?.Roles?.Select(x => x.toRolIdEntity()) ?? Enumerable.Empty<Role>();

            return roles ?? Enumerable.Empty<Role>();
        }
        catch (ExceptionResult)
        {
            throw new RoleException("An internal error occurred while asking the roles, please try later");
        }
    }

    public async Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> SearchRolesAsync(
        string query,
        int pageNumber,
        int pageSize)
    {
        try
        {
            var response = await _apiClient.SearchRoles.GetAsync(x =>
            {
                x.QueryParameters.Name = query;
                x.QueryParameters.PageNumber = pageNumber;
                x.QueryParameters.PageSize = pageSize;
            }).ConfigureAwait(false);

            var roles = response?.Roles?.Select(x => x.ToEntity()) ?? Enumerable.Empty<Role>();
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

            return (roles, metadata);
        }
        catch (Microsoft.Kiota.Abstractions.ApiException apiEx)
        {
            throw apiEx.ResponseStatusCode switch
            {
                500 => new RoleException("An internal server error occurred while searching roles."),
                _ => new RoleException($"API error occurred while searching roles: {apiEx.Message}")
            };
        }
        catch (Exception ex)
        {
            throw new RoleException($"An error occurred while searching roles: {ex.Message}", ex);
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
    public async Task<(IEnumerable<Role> Roles, PaginationMetadata Metadata)> ListRolesPagedAsync(int pageNumber, int pageSize)
    {
        var response = await _apiClient.Roles.Paginated.GetAsync(c =>
        {
            c.QueryParameters.PageNumber = pageNumber;
            c.QueryParameters.PageSize = pageSize;
        }).ConfigureAwait(false);

        var roles = response?.Role?.Select(RoleDtoMapper.ToEntity) ?? Enumerable.Empty<Role>();
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

        return (roles, metadata);
    }
}