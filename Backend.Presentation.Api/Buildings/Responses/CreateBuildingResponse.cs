using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

/// <summary>
/// Response for creating a building.
/// </summary>
/// <param name="Building">Data transfer object for the created building.</param>
public record CreateBuildingResponse(BuildingDtoWithId Building);


