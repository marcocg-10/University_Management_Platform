using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;

internal class ListRolePermissionsHandler
{
    public static async Task<IResult> HandleAsync(
        [FromServices] IRoleService roleService,
        [FromRoute] int roleId)
    {
        var (permissions, errorMessage) = await roleService.GetRolePermissionsAsync(roleId);
        if (permissions is null)
        {
            return Results.InternalServerError(
                new ListRolesPermissionsErrorResponse(
                    ErrorMessage: errorMessage ?? "Could not retrieve role permissions",
                    ErrorCode: "ROLE_PERMISSIONS_RETRIEVAL_ERROR"
                )
            );
        }
        var permissionsDtos = permissions.Select(PermissionDtoMapper.ToIdDto);
        return Results.Ok(
            new ListRolesPermissionsResponse(
                permissionsDtos
            )
        );
    }
}