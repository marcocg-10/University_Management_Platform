using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Handlers;

internal class ListRolesHandler
{
    public static async Task<Results<
        Ok<ListRolesResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync(
        [FromServices] IRoleService roleService)
    {
        try
        {
            var roles = await roleService.GetRolesAsync();

            return TypedResults.Ok(
                new ListRolesResponse(
                    Roles: roles.Select(RoleDtoMapper.ToIdDto)
                )
            );
        }
        catch (Exception ex)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                    StatusCode: 500,
                    Type: "InternalServerError",
                    Title: "Internal Server Error",
                    Detail: ex.Message
                )
            );
        }
    }
}