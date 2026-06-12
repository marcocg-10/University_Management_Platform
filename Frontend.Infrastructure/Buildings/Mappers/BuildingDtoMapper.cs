using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Mappers;

/// <summary>
/// Provides mapping functionality to convert <see cref="BuildingDto"/> objects into domain <see cref="Building"/> entities.
/// </summary>
internal static class BuildingDtoMapper
{
    /// <summary>
    /// Converts a <see cref="BuildingDto"/> instance into a <see cref="Building"/> domain entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing building information.</param>
    /// <returns>A <see cref="Building"/> entity populated with data from the DTO.</returns>
    public static Building toEntity(this BuildingDtoWithId dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto), "The BuildingDto is null.");
        }

        return new Building(
            dto.Id ?? throw new ArgumentNullException(nameof(dto.Id), "The Id is null."),
            dto.OfficialId ?? throw new ArgumentNullException(nameof(dto.OfficialId), "The OfficialId is null."),
            dto.Name ?? throw new ArgumentNullException(nameof(dto.Name), "The Name is null."),
            dto.FloorCount ?? throw new ArgumentNullException(nameof(dto.FloorCount), "The floor count is invalid"),
            dto.BuildingRenderInfo.toEntity());
    }
}
