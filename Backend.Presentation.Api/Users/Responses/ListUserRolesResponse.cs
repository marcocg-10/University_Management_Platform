using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

/// <summary>
/// Represents the response containing a collection of roles assigned to a user.
/// </summary>
public record ListUserRolesResponse(IEnumerable<RoleIdDto> roles);

