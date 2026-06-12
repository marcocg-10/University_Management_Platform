using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Response for listing roles.
/// </summary>
public record ListRolesResponse(
    IEnumerable<RoleIdDto> Roles
);
