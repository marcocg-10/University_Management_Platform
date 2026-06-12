using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Responses;

/// <summary>
/// Represents the response returned after attempting to create a role.
/// </summary>
public record CreateRoleResponse(
    RoleIdDto Role
);
