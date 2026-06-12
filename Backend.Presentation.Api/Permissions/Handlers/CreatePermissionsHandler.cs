using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Mappers;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Handlers;

/// <summary>
/// Handles the creation of a new permission by processing the provided permission data and delegating the operation to the permission service.
/// </summary>
internal class CreatePermissionsHandler
{

    /// <summary>
    /// Handles the creation of a new permission based on the provided data.
    /// </summary>
    /// <remarks>The method validates the provided permission data and ensures that the permission name is
    /// unique and within the allowed length.</remarks>
    /// <param name="permissionService">The service used to manage permissions.</param>
    /// <param name="permissionDto">The data transfer object containing the details of the permission to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IResult"/> indicating the
    /// outcome of the operation: <list type="bullet"> <item><description><see cref="Results.Created"/> if the
    /// permission is successfully created.</description></item> <item><description><see cref="Results.Conflict"/> if a
    /// permission with the same name already exists or the name exceeds 20 characters.</description></item>
    /// <item><description><see cref="Results.UnprocessableEntity"/> if the provided permission data is
    /// invalid.</description></item> </list></returns>
    public static async Task<IResult> HandleAsync([FromServices] IPermissionService permissionService, [FromBody] PermissionDto permissionDto)
    {
        var permission = PermissionDtoMapper.ToEntity(permissionDto)!;
         await permissionService.CreatePermissionAsync(permission);

        return Results.Created($"/permissions/{permissionDto.Name}", permission);

        /* 
         * if (permission == null)
        {
           
            return Results.UnprocessableEntity("Name is invalid: should be from 3 to 30 characters (letters, numbers, hyphen) and must start with a letter");
        }
        var status = await permissionService.CreatePermissionAsync(permission);
        if (status == null)
        {
            return Results.Conflict("Permission with the same name already exists");
        }


        return Results.Created($"/permissions/{permissionDto.Name}", permission);
        */
       }
}
