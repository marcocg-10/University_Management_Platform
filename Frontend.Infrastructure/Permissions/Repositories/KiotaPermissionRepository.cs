using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Permissions.Mappers;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Permissions.Repositories;

/// <summary>
/// Provides methods for managing and retrieving permissions within the system.
/// </summary>
/// <remarks>This repository serves as an abstraction for accessing permission data, allowing the caller to
/// retrieve available permissions asynchronously. It is typically used to query and manage permissions for actions or
/// resources within the application.</remarks>
internal class KiotaPermissionRepository : IPermissionRepository
{

    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaPermissionRepository"/> class.
    /// </summary>
    /// <remarks>The <see cref="KiotaPermissionRepository"/> class depends on the provided <see
    /// cref="ApiClient"/>  to perform operations related to permissions. Ensure that the <paramref name="apiClient"/>
    /// is  properly configured before passing it to this constructor.</remarks>
    /// <param name="apiClient">The <see cref="ApiClient"/> instance used to interact with the API.</param>
    public KiotaPermissionRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously retrieves all available permissions.
    /// </summary>
    /// <remarks>This method returns a collection of permissions that represent the available actions or
    /// resources  within the system. The caller can enumerate the returned collection to access individual
    /// permissions.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of
    /// <see cref="Permission"/> objects representing all available permissions.</returns>
    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        var response = await _apiClient.Permissions.GetAsync();
        var permissions = response?.Permissions?.Select(PermissionDtoMapper.ToEntity)
            ?? Enumerable.Empty<Permission>();

        return permissions;
    }
    
    /// <summary>
    /// Gets all permissions for a user by their Azure Object Identifier via API call.
    /// </summary>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier (oid) to get permissions for.</param>
    /// <returns>A tuple with permissions and error message.</returns>
    public async Task<(IEnumerable<Permission>? permissions, string? errorMessage)> GetCurrentUserPermissionsAsync(string azureObjectIdentifier)
    {
        try
        {
            // Use the correct endpoint with Azure Object Identifier as path parameter
            var response = await _apiClient.Users[azureObjectIdentifier].Permissions.GetAsync();
            
            if (response?.Permissions == null)
            {
                return (Enumerable.Empty<Permission>(), null);
            }

            // Convert response to Permission entities
            var permissions = response.Permissions
                .Select(permString => new Permission(PermissionName.Create(permString)))
                .ToList();

            return (permissions, null);
        }
        catch (Microsoft.Kiota.Abstractions.ApiException apiEx) when (apiEx.ResponseStatusCode == 404)
        {
            return (Enumerable.Empty<Permission>(), null);
        }
        catch (Exception ex)
        {
            return (null, $"Error getting permissions for user {azureObjectIdentifier}: {ex.Message}");
        }
    }
    /// ToDo: add Create method
}
