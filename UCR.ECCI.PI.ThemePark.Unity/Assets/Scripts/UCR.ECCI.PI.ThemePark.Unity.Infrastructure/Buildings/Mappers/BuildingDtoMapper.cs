using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;
using System;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Buildings.Mappers
{
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
                BuildingOfficialId.Create(dto.OfficialId ?? throw new ArgumentNullException(nameof(dto.OfficialId), "The OfficialId is null.")),
                BuildingName.Create(dto.Name ?? throw new ArgumentNullException(nameof(dto.Name), "The Name is null.")),
                FloorCount.Create(dto.FloorCount ?? throw new ArgumentNullException(nameof(dto.FloorCount), "The FloorCount is null.")),
                dto.BuildingRenderInfo.toEntity());
        }
    } 
}