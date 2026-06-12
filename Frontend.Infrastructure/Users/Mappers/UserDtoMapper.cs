using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Mappers;

/// <summary>
/// Provides extension methods for mapping between <see cref="UserDto"/> and <see cref="User"/> entities.
/// </summary>
/// <remarks>This class contains methods to facilitate the conversion of data transfer objects (DTOs) into domain
/// entities. It ensures that all required fields are present and valid during the mapping process.</remarks>
internal static class UserDtoMapper
{
    /// <summary>
    /// Converts a <see cref="UserDto"/> instance to a <see cref="User"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="UserDto"/> instance to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="User"/> entity populated with the data from the specified <see cref="UserDto"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <see langword="null"/> or if any required properties of <paramref
    /// name="dto"/>  (such as <c>Id</c>, <c>Name</c>, <c>Email</c>, or <c>IsActive</c>) are <see langword="null"/>.</exception>
    public static User ToEntity(this UserDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var idStr = dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "User.Id is null"); 
        var id = UserId.Create(idStr);
        var nameStr = dto.Name ?? throw new ArgumentNullException(nameof(dto.Name), "User.Name is null");
        var name = UserName.Create(nameStr);
        var emailStr = dto.Email ?? throw new ArgumentNullException(nameof(dto.Email), "User.Email is null");
        var email = Email.Create(emailStr);
        var isActive = dto.IsActive ?? throw new ArgumentNullException(nameof(dto.IsActive), "User.IsActive is null");

        return new User(id, name, isActive, email);
    }

    /// <summary>
    /// Converts a <see cref="UserIdDto"/> instance to a <see cref="User"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="UserIdDto"/> instance to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="User"/> entity populated with the data from the specified <see cref="UserDto"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <see langword="null"/> or if any required properties of <paramref
    /// name="dto"/>  (such as <c>Id</c>, <c>Name</c>, <c>Email</c>, or <c>IsActive</c>) are <see langword="null"/>.</exception>
    public static User ToIdEntity(this UserIdDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var idKey = dto.IdKey ?? throw new ArgumentNullException(nameof(dto.IdKey), "User.IdKey is null");
        var idStr = dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "User.Id is null");
        var id = UserId.Create(idStr);
        var nameStr = dto.Name ?? throw new ArgumentNullException(nameof(dto.Name), "User.Name is null");
        var name = UserName.Create(nameStr);
        var emailStr = dto.Email ?? throw new ArgumentNullException(nameof(dto.Email), "User.Email is null");
        var email = Email.Create(emailStr);
        var isActive = dto.IsActive ?? throw new ArgumentNullException(nameof(dto.IsActive), "User.IsActive is null");

        return new User(idKey, id, name, isActive, email);
    }

    /// <summary>
    /// Converts a domain <see cref="User"/> entity to a <see cref="UserDto"/> suitable for API requests.
    /// </summary>
    /// <param name="entity">The domain user entity.</param>
    /// <returns>A populated <see cref="UserDto"/>.</returns>
    public static UserDto ToDto(this User entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        return new UserDto
        {
            Id = entity.Id.Value,
            Name = entity.Name.Value,
            Email = entity.Email.Value,
            IsActive = entity.IsActive
        };
    }
}
