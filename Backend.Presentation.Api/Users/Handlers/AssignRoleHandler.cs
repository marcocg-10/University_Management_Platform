using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

internal class AssignRoleHandler
{
    public static async Task<Results<
        Ok<AssignRoleToUserResponse>,
        NotFound<AssignRoleToUserResponse>,
        Conflict<AssignRoleToUserResponse>,
        BadRequest<AssignRoleToUserResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync(
        [FromServices] IUserService userService,
        [FromServices] IRoleService roleService,
        [FromRoute] int userId,
        [FromBody] RoleDto roleDto)
    {
        // 1️⃣ Fetch User
        User? userResult;
        try
        {
            userResult = await userService.GetUserFromIdKeyAsync(userId);
        }
        catch (UserNotFoundException ex)
        {
            return TypedResults.NotFound(new AssignRoleToUserResponse(false, ex.Message));
        }

        if (userResult is null)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while retrieving the user.")
            );
        }

        // 2️⃣ Map RoleDto to Role entity
        Role? role;
        try
        {
            role = RoleDtoMapper.ToEntity(roleDto);
        }
        catch (RoleInvalidDataException ex)
        {
            return TypedResults.BadRequest(new AssignRoleToUserResponse(false, ex.Message));
        }

        // 3️⃣ Try to assign Role to User
        try
        {
            await userService.AssociateRoleAsync(userResult, role!);
        }
        catch (AssignableRoleNotFoundException ex)
        {
            return TypedResults.NotFound(new AssignRoleToUserResponse(false, ex.Message));
        }
        catch (RoleAlreadyAssignedException ex)
        {
            return TypedResults.Conflict(new AssignRoleToUserResponse(false, ex.Message));
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
       
        // 4️ Return success
        return TypedResults.Ok(new AssignRoleToUserResponse (true, $"Role {role.Name.Value} was assigned successfully to user {userResult.Name.Value}"));
    }
}
