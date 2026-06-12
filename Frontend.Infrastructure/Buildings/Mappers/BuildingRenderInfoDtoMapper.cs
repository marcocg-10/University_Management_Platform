using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Mappers;

/// <summary>
/// Provides mapping functionality to convert <see cref="BuildingRenderInfoDto"/> objects into domain <see cref="BuildingRenderInfo"/> entities.
/// </summary>
internal static class BuildingRenderInfoDtoMapper
{
    /// <summary>
    /// Converts a <see cref="BuildingRenderInfoDto"/> instance into a <see cref="BuildingRenderInfo"/> domain entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing render information for a building.</param>
    /// <returns>A <see cref="BuildingRenderInfo"/> entity populated with data from the DTO.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto.Color"/> is null.</exception>
    public static BuildingRenderInfo toEntity(this BuildingRenderInfoDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto), "The BuildingRenderInfoDto is null.");
        }

        return new BuildingRenderInfo(
            dto.Color ?? throw new ArgumentNullException(nameof(dto.Color), "The color is null."),
            Convert.ToDecimal(dto.Height),
            Convert.ToDecimal(dto.Width),
            Convert.ToDecimal(dto.Depth),
            Convert.ToDecimal(dto.XCoordinate),
            Convert.ToDecimal(dto.YCoordinate),
            Convert.ToDecimal(dto.ZCoordinate),
            dto.Texture ?? throw new ArgumentNullException(nameof(dto.Texture), "The texture is null."));
    }
}
