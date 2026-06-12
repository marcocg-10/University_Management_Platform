namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

/// <summary>
/// DTO for sending building data without its ID.
/// </summary>
/// <param name="OfficialId">Official code of the building.</param>
/// <param name="Name">Name of the building.</param>
/// <param name="FloorCount">Number of floors.</param>
/// <param name="BuildingRenderInfo">Rendering and spatial data.</param>
public record BuildingDto(
    string OfficialId,
    string Name,
    int FloorCount,
    BuildingRenderInfoDto BuildingRenderInfo
);

/// <summary>
/// DTO for sending building data including its ID.
/// </summary>
/// <param name="Id">System-generated identifier.</param>
/// <param name="OfficialId">Official code of the building.</param>
/// <param name="Name">Name of the building.</param>
/// <param name="FloorCount">Number of floors.</param>
/// <param name="BuildingRenderInfo">Rendering and spatial data.</param>
public record BuildingDtoWithId(
    int Id,
    string OfficialId,
    string Name,
    int FloorCount,
    BuildingRenderInfoDto BuildingRenderInfo
);
