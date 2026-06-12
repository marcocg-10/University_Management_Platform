namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the response returned when an error occurs while listing roles of a user.
/// </summary>
public record ListUserRolesErrorResponse(string ErrorMessage, string ErrorCode);
