using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Dtos;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

/// <summary>
/// Response for getting a list of buildings.
/// </summary>
/// <param name="Buildings">List of building data transfer objects.</param>
public record GetBuildingsResponse(IEnumerable<BuildingDtoWithId> Buildings);


