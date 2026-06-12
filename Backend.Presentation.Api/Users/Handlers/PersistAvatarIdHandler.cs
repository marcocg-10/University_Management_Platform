using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Minimal API handler that validates input, authenticates the caller via claims, fetches the user,
/// and persists the Ready Player Me <see cref="AvatarId"/> on the server side.
/// </summary>
internal class PersistAvatarIdHandler
{
    /// <summary>
    /// Processes the request to persist the avatar id for the authenticated user.
    /// </summary>
    /// <param name="userService">Application service used to access user operations.</param>
    /// <param name="httpContext">The current request context (used to read claims).</param>
    /// <param name="AvatarIdDto">The DTO containing the avatar identifier.</param>
    /// <returns>Typed Minimal API results indicating success or failure reasons.</returns>
    public static async Task<Results<
        Ok<SuccesfulPersistAvatarIdResponse>,
        UnauthorizedHttpResult,
        NotFound<ErrorPersistAvatarIdResponse>,
        Conflict<ErrorPersistAvatarIdResponse>,
        BadRequest<ErrorPersistAvatarIdResponse>,
        InternalServerError<ExceptionResult>
        >> HandleAsync(
        [FromServices] IUserService userService, HttpContext httpContext,
        [FromBody] AvatarIdDto AvatarIdDto)
    {
        // Validate input
        AvatarId? avatarId = null;
        if (!AvatarIdDtoMapper.ToEntity(AvatarIdDto, out avatarId, out string? errorMessage) || avatarId == null)
        {
            return TypedResults.BadRequest(new ErrorPersistAvatarIdResponse("INVALID_AVATAR_DATA", errorMessage!));
        }

        // Get Id from authenticated user
        var userPrincipal = httpContext.User;
        // Extract a suitable claim (Azure AD oid preferred)
        string? azureObjectId = userPrincipal.FindFirst("oid")?.Value
                                ?? userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                ?? userPrincipal.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(azureObjectId))
        {
            return TypedResults.InternalServerError(new ExceptionResult
            {
                StatusCode = 500,
                Type = "ClaimMissing",
                Title = "User claim missing",
                Detail = "Could not determine the authenticated user's identifier from claims."
            });
        }

        // Fetch User
        User? userResult;
        try
        {
            userResult = await userService.GetUserByAzureObjectIdentifierAsync(azureObjectId);
        }
        catch (UserNotFoundException ex)
        {
            return TypedResults.NotFound(new ErrorPersistAvatarIdResponse("USER_NOT_FOUND", ex.Message));
        }
        catch (Exception)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while retrieving the user.")
            );
        }

        // Try to save AvatarId to User
        try
        {
            await userService.SaveAvatarId(userResult.IdKey, avatarId);
        }
        catch (UserNotFoundException ex)
        {
            return TypedResults.NotFound(new ErrorPersistAvatarIdResponse("USER_NOT_FOUND", ex.Message));
        }
        catch (Exception)
        {
            return TypedResults.InternalServerError(
                new ExceptionResult(
                StatusCode: 500,
                Type: "InternalServerError",
                Title: "Internal Server Error",
                Detail: "An unexpected error occurred while persisting the AvatarId")
            );
        }
       
        // Return success
        return TypedResults.Ok(new SuccesfulPersistAvatarIdResponse($"AvatarId '{avatarId.Value}' persisted successfully for user '{userResult.IdKey}'."));
    }
}
