namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

/// <summary>
/// Represents a data transfer object for a user, containing basic user information.
/// </summary>
/// <remarks>This record is typically used to transfer user data between application layers or services.</remarks>
/// <param name="Id">The unique identifier of the user. Typically a GUID or string representing the user's identity.</param>
/// <param name="Name">The full name of the user. This should be a non-empty string.</param>
/// <param name="IsActive">Indicates whether the user account is currently active. True if active; otherwise, false.</param>
/// <param name="Email">The email address associated with the user. Should be a valid email format.</param>
public record UserDto(string Id, string Name, bool IsActive, string Email);


/// <summary>
/// Represents a data transfer object for a user, containing basic user information.
/// </summary>
/// <remarks>This record is typically used to transfer user data between application layers or services.</remarks>
/// <param name="IdKey">The primary key identifier of the user in the database.</param>
/// <param name="Id">The unique identifier of the user. Typically a GUID or string representing the user's identity.</param>
/// <param name="Name">The full name of the user. This should be a non-empty string.</param>
/// <param name="IsActive">Indicates whether the user account is currently active. True if active; otherwise, false.</param>
/// <param name="Email">The email address associated with the user. Should be a valid email format.</param>
public record UserIdDto(int IdKey, string Id, string Name, bool IsActive, string Email);