using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="User"/> and <see cref="UserDto"/> objects.
/// </summary>
/// <remarks>This class contains methods to convert a <see cref="User"/> entity to a <see cref="UserDto"/>  and
/// vice versa. The mapping ensures that the essential properties of the objects are transferred  while validating the
/// data where necessary.</remarks>
internal static class UserDtoMapper
{

    /// <summary>
    /// Converts a <see cref="User"/> entity to its corresponding <see cref="UserDto"/> representation.
    /// </summary>
    /// <param name="entity">The <see cref="User"/> instance to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="UserDto"/> containing the data from the specified <see cref="User"/> entity.</returns>
    internal static UserDto ToDto(this User entity)
    {
        return new UserDto(
            entity.Id.Value,
            entity.Name.Value,
            entity.IsActive,
            entity.Email.Value);
    }

    /// <summary>
    /// Converts a <see cref="User"/> entity to its corresponding <see cref="UserDto"/> representation with idkey.
    /// </summary>
    /// <param name="entity">The <see cref="User"/> instance to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="UserDto"/> containing the data from the specified <see cref="User"/> entity.</returns>
    internal static UserIdDto ToIdDto(this User entity)
    {
        return new UserIdDto(
            entity.IdKey,
            entity.Id.Value,
            entity.Name.Value,
            entity.IsActive,
            entity.Email.Value);
    }

    /// <summary>
    /// Converts the current <see cref="UserDto"/> instance to a <see cref="User"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="UserDto"/> instance to convert.</param>
    /// <returns>A <see cref="User"/> instance representing the converted data, or <see langword="null"/>  if the conversion
    /// fails due to invalid email data.</returns>
    internal static bool ToEntity(this UserDto dto, out User? user, out string? error)
    {
        user = null;
        error = null;

        var emailResult = Email.TryCreate(dto.Email, out var email, out var emailError);
        if (!emailResult || email is null)
        {
            error = emailError;
            return false;
        }
        var userNameResult = UserName.TryCreate(dto.Name, out var userName, out var userError);
        if (!userNameResult || userName is null)
        {
            error = userError;
            return false;
        }
        var userIdResult = UserId.TryCreate(dto.Id, out var userId, out var idError);
        if (!userIdResult || userId is null)
        {
            error = idError;
            return false;
        }

        user = new User(
            userId,
            userName,
            dto.IsActive,
            email);
        return true;
    }
}
