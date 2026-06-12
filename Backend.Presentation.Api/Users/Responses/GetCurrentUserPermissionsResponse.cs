namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Response containing a user's permissions.
/// </summary>
/// <param name="Permissions">List of permission names the user has.</param>
public record GetCurrentUserPermissionsResponse(IEnumerable<string> Permissions);