using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Handles requests to get all permissions for a specific user by their Azure Object Identifier.
/// </summary>
public static class GetCurrentUserPermissionsHandler
{
    /// <summary>
    /// Handles the request to get all permissions for a user by their Azure Object Identifier.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier of the user.</param>
    /// <returns>A response containing the user's permissions.</returns>
    public static async Task<GetCurrentUserPermissionsResponse> HandleAsync(
        IUserService userService,
        string azureObjectIdentifier)
    {
        try
        {
            // Get user by Azure OID
            var user = await userService.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);
            if (user == null)
            {
                return new GetCurrentUserPermissionsResponse([]);
            }

            // Get user permissions
            var permissions = await userService.GetCurrentUserPermissionsAsync(user.IdKey);
            if (permissions == null)
            {
                return new GetCurrentUserPermissionsResponse([]);
            }

            // Convert permissions to string array
            var permissionNames = permissions
                .Select(permission => permission.Name.Value)
                .ToList();

            return new GetCurrentUserPermissionsResponse(permissionNames);
        }
        catch (Exception)
        {
            return new GetCurrentUserPermissionsResponse([]);
        }
    }
}
