using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Mappers;

/// <summary>
/// Mapper for converting between BuildingRenderInfo entities and BuildingRenderInfo DTOs.
/// </summary>
internal static class BuildingRenderInfoDtoMapper
{
    /// <summary>
    /// Converts a BuildingRenderInfo entity to a BuildingRenderInfoDto.
    /// </summary>
    /// <param name="buildingRenderInfo"></param>
    /// <returns>Data transfer object for building render information.</returns>
    internal static BuildingRenderInfoDto ToDto(this BuildingRenderInfo buildingRenderInfo)
    {
        return new BuildingRenderInfoDto(
            buildingRenderInfo.Color.Value,
            buildingRenderInfo.Heigth.Value,
            buildingRenderInfo.Width.Value,
            buildingRenderInfo.Depth.Value,
            buildingRenderInfo.XCoodinate.XValue,
            buildingRenderInfo.YCoodinate.YValue,
            buildingRenderInfo.ZCoodinate.ZValue,
            buildingRenderInfo.Texture.Value
            );
    }

/// <summary>
/// Converts a BuildingRenderInfoDto to a BuildingRenderInfo entity.
/// </summary>
/// <param name="dto"></param>
/// <returns>BuildingRenderInfo entity.</returns>
    internal static BuildingRenderInfo ToEntity(BuildingRenderInfoDto dto)
    {
        return new BuildingRenderInfo(
            Color.Create(dto.Color),
            Heigth.Create(dto.Height),
            Width.Create(dto.Width),
            Depth.Create(dto.Depth),
            X.Create(dto.XCoordinate),
            Y.Create(dto.YCoordinate),
            Z.Create(dto.ZCoordinate),
            BuildingTexture.Create(dto.Texture)
            );
    }
}
