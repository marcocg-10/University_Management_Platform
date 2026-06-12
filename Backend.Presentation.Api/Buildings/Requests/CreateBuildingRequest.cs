
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Buildings.Responses;

/// <summary>
/// Request for creating a building.
/// </summary>
/// <param name="OfficialID">Official ID of the building.</param>
/// <param name="Name">Name of the building.</param>
/// <param name="FloorCount">Number of floors in the building.</param>
/// <param name="Color">Color of the building.</param>
/// <param name="Height">Height of the building.</param>
/// <param name="Width">Width of the building.</param>
/// <param name="Depth">Depth of the building.</param>
/// <param name="X">X coordinate of the building.</param>
/// <param name="Y">Y coordinate of the building.</param>
/// <param name="Z">Z coordinate of the building.</param>
/// <param name="Texture">Texture of the building.</param>
public record CreateBuildingRequest(string OfficialID, string Name, int FloorCount, string Color, decimal Height,
    decimal Width, decimal Depth, decimal X, decimal Y, decimal Z, string Texture);


