using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Response for listing roles with their permissions.
/// </summary>
public record ListRolesPermissionsResponse(
    IEnumerable<PermissionIdDto> permissions
);
