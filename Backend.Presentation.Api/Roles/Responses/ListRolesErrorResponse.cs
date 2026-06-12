namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Represents the response returned when an error occurs while listing roles.
/// </summary>
public record ListRolesErrorResponse(string ErrorMessage, string ErrorCode);