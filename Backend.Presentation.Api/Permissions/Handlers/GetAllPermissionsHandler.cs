using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Handlers;

/// <summary>
/// Handles the retrieval of all permissions from the permission service.
/// </summary>
public static class GetAllPermissionsHandler
{
    
    /// <summary>
    /// Handles the retrieval of all permissions asynchronously.
    /// </summary>
    /// <param name="permissionService">The service used to retrieve permissions. This parameter is resolved from the application's service container.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="GetAllPermissionsResponse"/> object with the list of permissions mapped to DTOs.</returns>
    public static async Task<GetAllPermissionsResponse> HandleAsync([FromServices] IPermissionService permissionService)
    {
        var permissions = await permissionService.GetAllPermissionsAsync();
        return new GetAllPermissionsResponse(
            permissions.Select(PermissionDtoMapper.ToIdDto));
    }
}   
