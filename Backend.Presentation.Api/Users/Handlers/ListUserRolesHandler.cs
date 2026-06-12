using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

internal class ListUserRolesHandler
{
    public static async Task<IResult> HandleAsync(
        [FromServices] IUserService userService,
        [FromRoute] int userId)
    {
        var roles = await userService.GetUserRolesAsync(userId);
        if (roles is null)
        {
            return Results.InternalServerError(
                new ListUserRolesErrorResponse(
                    ErrorMessage: "Could not retrieve user Roles",
                    ErrorCode: "ROLE_Roles_RETRIEVAL_ERROR"
                )
            );
        }
        var rolesDtos = roles.Select(RoleDtoMapper.ToIdDto);
        return Results.Ok(
            new ListUserRolesResponse(
                rolesDtos
            )
        );
    }
}