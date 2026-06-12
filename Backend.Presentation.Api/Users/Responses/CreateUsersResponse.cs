namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the response returned after attempting to create a user.
/// </summary>
public record CreateUserResponse(
    bool Success,
    string Message
);
