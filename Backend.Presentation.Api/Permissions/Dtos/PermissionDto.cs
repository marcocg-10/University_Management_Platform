namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;

/// <summary>
/// Represents a data transfer object for a permission, containing basic permission information.
/// </summary>
/// <remarks>This record is typically used to transfer permission data between application layers or services.</remarks>
/// <param name="Name">The full name of the permission. This should be a non-empty string, no longer than 20 characters.</param>

public record PermissionDto(string Name);

/// <summary>
/// Represents a data transfer object (DTO) for a permission identifier.
/// </summary>
/// <remarks>This DTO encapsulates the name and unique identifier of a permission, typically used for transferring
/// permission-related data between application layers.</remarks>
/// <param name="Name">The name of the permission. This value cannot be null or empty.</param>
/// <param name="id">The unique identifier of the permission. Must be a non-negative integer.</param>
public record PermissionIdDto(string Name, int id);