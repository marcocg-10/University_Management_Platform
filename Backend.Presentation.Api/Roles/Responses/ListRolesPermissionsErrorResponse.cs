namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Represents the response returned when an error occurs while listing permissions of role.
/// </summary>
public record ListRolesPermissionsErrorResponse(string ErrorMessage, string ErrorCode);