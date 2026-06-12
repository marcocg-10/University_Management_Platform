namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Requests;

/// <summary>
/// Request for deleting a building.
/// </summary>
/// <param name="OfficialID">The official ID of the building to delete.</param>
public record DeleteBuildingRequest(string OfficialID);
