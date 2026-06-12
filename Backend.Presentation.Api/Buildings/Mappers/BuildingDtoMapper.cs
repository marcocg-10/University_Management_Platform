using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Mappers;

/// <summary>
/// Mapper for converting between Building entities and Building DTOs.
/// </summary>
internal static class BuildingDtoMapper
{
    /// <summary>
    /// Converts a Building entity to a BuildingDto.
    /// </summary>
    /// <param name="building"></param>
    /// <returns>Data transfer object for a building.</returns>
    internal static BuildingDto ToDto(this Building building)
    {
        return new BuildingDto(
            building.OfficialId.Value,
            building.Name.Value,
            building.FloorCount.Value,
            BuildingRenderInfoDtoMapper.ToDto(building.RenderInfo)
            );
    }

/// <summary>
/// Converts a BuildingDto to a Building entity.
/// </summary>
/// <param name="buildingDto">Data transfer object for a building.</param>
/// <returns>Building entity.</returns>
    internal static Building toEntity(BuildingDto buildingDto)
    {
        return new Building(
            BuildingOfficialId.Create(buildingDto.OfficialId),
            BuildingName.Create(buildingDto.Name),
            FloorCount.Create(buildingDto.FloorCount),
            BuildingRenderInfoDtoMapper.ToEntity(buildingDto.BuildingRenderInfo)
            );
    }

    internal static BuildingDtoWithId ToDtoWithId(this Building building)
    {
        return new BuildingDtoWithId(
            building.Id,
            building.OfficialId.Value,
            building.Name.Value,
            building.FloorCount.Value,
            BuildingRenderInfoDtoMapper.ToDto(building.RenderInfo)
            );
    }
}
