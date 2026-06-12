using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Provides functionality to handle the retrieval of a user by their Azure Object Identifier.
/// </summary>
/// <remarks>
/// This handler is used to find users based on their Azure AD Object Identifier (OID),
/// which is essential for permission-based authentication systems.
/// </remarks>
public static class GetUserByAzureObjectIdentifierHandler
{
    /// <summary>
    /// Handles the retrieval of a user by their Azure Object Identifier.
    /// </summary>
    /// <param name="userService">The service used to retrieve user information.</param>
    /// <param name="azureObjectIdentifier">The Azure Object Identifier to search for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an IResult
    /// with either the user information or an appropriate error response.
    /// </returns>
    public static async Task<IResult> HandleAsync(
        [FromServices] IUserService userService,
        [FromRoute] string azureObjectIdentifier)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(azureObjectIdentifier))
        {
            return Results.BadRequest(new GetUserByAzureObjectIdentifierErrorResponse(
                "Azure Object Identifier cannot be null or empty",
                "INVALID_INPUT"));
        }

        try
        {
            // Get user from service
            var user = await userService.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

            // Handle user not found (not an error, just return 404)
            if (user is null)
            {
                return Results.NotFound(new GetUserByAzureObjectIdentifierErrorResponse(
                    $"User with Azure Object Identifier '{azureObjectIdentifier}' not found",
                    "USER_NOT_FOUND"));
            }

            // Success - return the user
            var userDto = user.ToIdDto();
            return Results.Ok(new GetUserByAzureObjectIdentifierResponse(userDto));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: "An unexpected error occurred while retrieving the user",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error");
        }
    }
}
