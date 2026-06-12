//using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
//using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Handles the creation of a new user by processing the provided user data and delegating the operation to the user
/// service.
/// </summary>
/// <remarks>This method validates the provided user data, maps it to a user entity, and attempts to create the
/// user using the specified user service. It returns an appropriate HTTP result based on the outcome of the
/// operation.</remarks>
public static class CreateUsersHandler
{

    /// <summary>
    /// Handles the creation of a new user based on the provided user data.
    /// </summary>
    /// <param name="usersService">The service used to manage user-related operations.</param>
    /// <param name="userDto">The data transfer object containing the user's information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is an <see cref="IResult"/> indicating the
    /// outcome of the operation: <list type="bullet"> <item><description><see cref="Results.Created"/> if the user was
    /// successfully created.</description></item> <item><description><see cref="Results.Conflict"/> if the user's ID or
    /// email already exists.</description></item> <item><description><see cref="Results.UnprocessableEntity"/> if the
    /// provided user data is invalid.</description></item> </list></returns>
    public static async Task<Results<
        Created<CreateUserResponse>,
        Conflict<CreateUserResponse>,
        BadRequest<CreateUserResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync([FromServices] IUserService usersService, [FromBody] UserDto userDto)
    {
        bool validUser = false;
        var userError = string.Empty;
        User? user = null;
        try
        {
            validUser = UserDtoMapper.ToEntity(userDto, out user, out userError);
            if (!validUser)
            {
                return TypedResults.BadRequest(new CreateUserResponse(false, userError!));
            }
            
        }
        catch (Exception ex)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while retrieving the user.")
            );
        }
        User? createdUser;
        try
        {
            createdUser = await usersService.CreateUserAsync(user!);
        }
        catch (DuplicateValueInEntityException ex)
        {
            return TypedResults.Conflict(new CreateUserResponse(false, ex.Message));
        }
        catch (Exception ex)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: $"An unexpected error occured: {ex.Message}")
            );
        }
       
        return TypedResults.Created($"/users/{createdUser.Id}", new CreateUserResponse(true, "User created successfully."));
    }
}
