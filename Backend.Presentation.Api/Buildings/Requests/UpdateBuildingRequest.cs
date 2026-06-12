namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Requests;


/// <summary>
/// Represents a request to update a building entity.
/// </summary>
/// <param name="OfficialID">The official ID of the building to update.</param>
/// <param name="Name">The updated name of the building.</param>
/// <param name="FloorCount">The updated floor count of the building.</param>
/// <param name="Color">The updated color of the building.</param>
/// <param name="Height">The updated height of the building.</param>
/// <param name="Width">The updated width of the building.</param>
/// <param name="Depth">The updated depth of the building.</param>
/// <param name="X">The updated X coordinate of the building.</param>
/// <param name="Y">The updated Y coordinate of the building.</param>
/// <param name="Z">The updated Z coordinate of the building.</param>
public record UpdateBuildingRequest(string OfficialID, string Name, int FloorCount, string Color, decimal Height, decimal Width,
    decimal Depth, decimal X, decimal Y, decimal Z, string Texture);