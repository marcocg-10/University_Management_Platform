using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the response returned after attempting to register a user from Azure Entra ID claims.
/// </summary>
/// <param name="Message">A descriptive message about the registration result.</param>
/// <param name="User">The user Dto.</param>
/// <param name="IsNewUser">Indicates whether this was a new user registration (true) or existing user (false).</param>
/// <param name="IsNewUserInAzureAD">Indicates whether the user is new to Entra ID.</param>
/// <param name="Identification">The identification.</param>
public record RegisterUserResponse(
    string Message,
    UserDto User,
    bool IsNewUser,
    bool IsNewUserInAzureAD,
    string? Identification
);
