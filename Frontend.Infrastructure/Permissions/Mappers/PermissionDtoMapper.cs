using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Permissions.Mappers;

/// <summary>
/// Provides methods for mapping between <see cref="PermissionIdDto"/> and <see cref="Permission"/> entities.
/// </summary>
/// <remarks>This class contains extension methods to facilitate the conversion of data transfer objects (DTOs) 
/// to domain entities. It ensures that required properties are validated during the mapping process.</remarks>
internal static class PermissionDtoMapper
{

    /// <summary>
    /// Converts a <see cref="PermissionIdDto"/> instance to a <see cref="Permission"/> entity.
    /// </summary>
    /// <param name="dto">The <see cref="PermissionIdDto"/> to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="Permission"/> entity initialized with the values from the specified <paramref name="dto"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is <see langword="null"/>, or if any required property of <paramref
    /// name="dto"/>  (such as <see cref="PermissionIdDto.Id"/> or <see cref="PermissionIdDto.Name"/>) is <see
    /// langword="null"/>.</exception>
    public static Permission ToEntity(this PermissionIdDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var id = dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "Permission.Id is null");
        var nameStr = dto.Name ?? throw new ArgumentNullException(nameof(dto.Name), "Permission.Name is null");
        var name = PermissionName.Create(nameStr);
        
        return new Permission(name)
        {
            Id = id
        };       
    }
}
