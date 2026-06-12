using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Utilities;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;

/// <summary>
/// Handler for user registration by extracting claims from Azure Entra ID authentication
/// and creating the user in the database if they don't already exist.
/// </summary>
public static class RegisterUserFromClaimsHandler
{
    /// <summary>
    /// Registers a user based on Azure Entra ID.
    /// </summary>
    /// <param name="userService">The service used to manage user-related operations.</param>
    /// <param name="httpContext">The HTTP context containing user claims.</param>
    /// <returns>A task that represents the operation with the result indicating success or failure.</returns>
    public static async Task<IResult> HandleAsync(
        [FromServices] IUserService userService,
        [FromServices] IRoleService roleService,
        ClaimsPrincipal principal)
    {
        try
        {
            // Ensure user is authenticated
            if (principal.Identity?.IsAuthenticated != true)
            {
                return Results.Unauthorized();
            }

            // Extract claims from the authenticated user
            var claimsResult = AzureClaimsExtractor.ExtractUserClaims(principal);
            if (!claimsResult.IsSuccess)
            {
                return Results.BadRequest($"Missing required claims: {claimsResult.ErrorMessage}");
            }

            var userClaims = claimsResult.Claims!;

            // Create email value object
            if (!Email.TryCreate(userClaims.Email, out var email, out var error) || email is null)
            {
                return Results.BadRequest(error);
            }

            // Create username value object
            if (!UserName.TryCreate(userClaims.FullName, out var fullname, out var error1) || fullname is null)
            {
                return Results.BadRequest(error1);
            }

            // Create user ID value object
            if (!UserId.TryCreate(userClaims.Identification, out var id, out var error2) || id is null)
            {
                return Results.BadRequest(error2);
            }


            // Check if user already exists by Azure Object Identifier
            var existingUser = await userService.GetUserByAzureObjectIdentifierAsync(userClaims.AzureObjectIdentifier);

            if (existingUser != null)
            {
                var existingUserResponse = UserRegistrationHelper.CreateExistingUserResponse(existingUser, userClaims);
                return Results.Ok(existingUserResponse);
            }

            // Create new user from claims
            var newUser = UserRegistrationHelper.CreateUserFromClaims(userClaims, email, fullname, id);

            var createResult = await userService.CreateUserAsync(newUser);
            if (createResult == null)
            {
                return Results.Conflict(createResult);
            }

            var guestRoleName = RoleName.Create("Guest");
            

            var guestRole = await roleService.GetRoleByNameAsync(guestRoleName);
            
            await userService.AssociateRoleAsync(newUser, guestRole);

            // Create successful registration response
            var successResponse = UserRegistrationHelper.CreateNewUserResponse(newUser, userClaims);
            return Results.Created($"/users/{newUser.Id}", successResponse);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred during user registration: {ex.Message}");
        }
    }
}