using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;

internal class AssignPermissionHandler
{
    public static async Task<Results<
        Ok<string>,
        NotFound<string>,
        Conflict<ConflictErrorResponse>,
        BadRequest<ValidationErrorResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync(
      [FromServices] IRoleService roleService,
      [FromRoute] int roleId,
      [FromBody] PermissionDto permissionDto)
    {
        // Fetch Role
        Role? roleResult;
        try 
        { 
            (roleResult, _) = await roleService.GetRoleFromIdAsync(roleId);
        } 
        catch (RoleNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        if (roleResult is null)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while retrieving the role.")
                );
        }
        // Map PermissionDto to Permission entity
        Permission? permission;
        try
        {
             permission = PermissionDtoMapper.ToEntity(permissionDto);
        } 
        catch (PermissionInvalidDataException ex)
        {
            return TypedResults.BadRequest(
                new ValidationErrorResponse(ex.Message)
            );
        }
        // try to Assign Permission to Role

        try
        {
            (roleResult, permission) = await roleService.AssociatePermissionAsync(roleResult, permission!);
        }    
        catch (AssignablePermissionNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (PermissionAlreadyAssignedException ex)
        {
            return TypedResults.Conflict(
                new ConflictErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: ex.Message)
                );
        }
        return TypedResults.Ok($"Permission {permission.Name.Value} was assigned succesfully to role {roleResult.Name.Value}");
       
    }
}
