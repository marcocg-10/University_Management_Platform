using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Request;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;

/// <summary>
/// Handles the creation of a new role by processing the provided role data and delegating the operation to the role service.
/// </summary>
internal class CreateRoleHandler
{
    public static async Task<Results<
        Created<CreateRoleResponse>,
        BadRequest<ValidationErrorResponse>,
        Conflict<ConflictErrorResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync(
        [FromServices] IRoleService roleService,
        [FromBody] CreateRoleRequest request)
    {

        Role role;
        try
        {
            role = request.Role.ToEntity();
        }
        catch (RoleInvalidDataException exception)
        {
            return TypedResults.BadRequest(
                new ValidationErrorResponse(exception.Message));
        }
        try
        {
            role = await roleService.CreateRoleAsync(role);
        } 
        catch (DuplicateValueInEntityException exception)
        {
            return TypedResults.Conflict(
                new ConflictErrorResponse(
                    exception.Message));
        }
        catch (Exception ex)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                    StatusCode: 500,
                    Type: "InternalServerError",
                    Title: "Internal Server Error",
                    Detail: ex.Message));
        }

        return TypedResults.Created(
            uri: $"/api/roles/{role.Id}",
            value: new CreateRoleResponse(
                Role: RoleDtoMapper.ToIdDto(role)
            )
        );
    }
}
