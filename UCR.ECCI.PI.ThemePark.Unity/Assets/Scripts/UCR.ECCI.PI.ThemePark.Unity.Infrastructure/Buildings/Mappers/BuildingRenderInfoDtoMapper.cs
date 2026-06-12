using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects.BuildingRenderInfo;
using System;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Buildings.Mappers
{
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
                Color.Create(dto.Color),
                Heigth.Create(Convert.ToDecimal(dto.Height)),
                Width.Create(Convert.ToDecimal(dto.Width)),
                Depth.Create(Convert.ToDecimal(dto.Depth)),
                X.Create(Convert.ToDecimal(dto.XCoordinate)),
                Y.Create(Convert.ToDecimal(dto.YCoordinate)),
                Z.Create(Convert.ToDecimal(dto.ZCoordinate)),
                BuildingTexture.Create(dto.Texture));
        }
    }
}