using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Permissions.Mappers;

/// <summary>
/// This static class provides extension methods for mapping between <see cref="Permission"/> entities and
/// </summary>
internal static class PermissionDtoMapper
{
    /// <summary>
    /// Transforms a <see cref="Permission"/> entity into its corresponding <see cref="PermissionDto"/> representation.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    internal static Permission? ToEntity(this PermissionDto dto)
    {
        var name = PermissionName.Create(dto.Name);
        return new Permission(
            name
        );
    }

    /// <summary>
    /// Converts a <see cref="Permission"/> entity to its corresponding <see cref="PermissionDto"/> representation.
    /// </summary>
    /// <param name="entity">The <see cref="Permission"/> entity to convert. Must not be <see langword="null"/>.</param>
    /// <returns>A <see cref="PermissionDto"/> instance representing the provided <see cref="Permission"/> entity.</returns>
    internal static PermissionDto ToDto(this Permission entity)
    {
        return new PermissionDto(
            entity.Name.Value
        );
    }

    /// <summary>
    /// Converts a <see cref="Permission"/> entity to a <see cref="PermissionIdDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Permission"/> entity to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="PermissionIdDto"/> containing the ID and name of the specified <see cref="Permission"/> entity.</returns>
    internal static PermissionIdDto ToIdDto(this Permission entity)
    {
        return new PermissionIdDto(
            entity.Name.Value,
            entity.Id
        );
    }
}
