namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Roles.Dtos;

/// <summary>
/// Represents a data transfer object for a role, containing basic role information.
/// </summary>
/// <remarks>This record is typically used to transfer role data between application layers or services.</remarks>
/// <param name="Name">The full name of the role.</param>
public record RoleDto(string Name);

/// <summary>
/// Data transfer object for role information.
/// </summary>
/// <param name="Id">The unique identifier of the role.</param>
/// <param name="Name">The name of the role.</param>
public record RoleIdDto(int Id, string Name);