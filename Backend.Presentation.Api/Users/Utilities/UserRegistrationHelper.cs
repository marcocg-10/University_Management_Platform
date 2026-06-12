using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Utilities;

/// <summary>
/// Helper class for user registration operations.
/// </summary>
public static class UserRegistrationHelper
{
    /// <summary>
    /// Creates a new user entity from claims data.
    /// </summary>
    /// <param name="userClaims">The user claims data.</param>
    /// <param name="email">The validated email value object.</param>
    /// <returns>A new User entity.</returns>
    public static User CreateUserFromClaims(UserClaimsDataDto userClaims, Email email, UserName fullname, UserId userId)
    {
        return new User(
            id: userId,
            name: fullname,
            isActive: true,
            email: email,
            azureObjectIdentifier: userClaims.AzureObjectIdentifier
        );
    }

    /// <summary>
    /// Creates a registration response for an existing user.
    /// </summary>
    /// <param name="existingUser">The existing user entity.</param>
    /// <param name="userClaims">The user claims data.</param>
    /// <returns>A RegisterUserResponse for an existing user.</returns>
    public static RegisterUserResponse CreateExistingUserResponse(User existingUser, UserClaimsDataDto userClaims)
    {
        var existingUserDto = UserDtoMapper.ToDto(existingUser);
        return new RegisterUserResponse(
            Message: "User already registered",
            User: existingUserDto,
            IsNewUser: false,
            IsNewUserInAzureAD: userClaims.IsNewUser,
            Identification: userClaims.Identification
        );
    }

    /// <summary>
    /// Creates a registration response for a newly created user.
    /// </summary>
    /// <param name="newUser">The newly created user entity.</param>
    /// <param name="userClaims">The user claims data.</param>
    /// <returns>A RegisterUserResponse for a new user.</returns>
    public static RegisterUserResponse CreateNewUserResponse(User newUser, UserClaimsDataDto userClaims)
    {
        var newUserDto = UserDtoMapper.ToDto(newUser);
        return new RegisterUserResponse(
            Message: "User successfully registered",
            User: newUserDto,
            IsNewUser: true,
            IsNewUserInAzureAD: userClaims.IsNewUser,
            Identification: userClaims.Identification
        );
    }
}