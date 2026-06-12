using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Responses;

/// <summary>
/// Represents the response containing a collection of permissions.
/// </summary>
/// <param name="Permissions">A collection of <see cref="PermissionIdDto"/> objects representing the permissions retrieved.</param>
public record GetAllPermissionsResponse(IEnumerable<PermissionIdDto> Permissions);
