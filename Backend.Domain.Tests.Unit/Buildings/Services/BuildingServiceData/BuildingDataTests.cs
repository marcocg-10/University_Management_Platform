using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.TestData;

/// <summary>
/// Provides factory methods to create valid <see cref="Building"/> instances for unit testing.
/// </summary>
public static class BuildingDataTests
{
    /// <summary>
    /// Creates a valid <see cref="Building"/> instance with customizable parameters.
    /// </summary>
    /// <param name="id">Official ID of the building (e.g., "B001").</param>
    /// <param name="name">Name of the building (e.g., "ECCI").</param>
    /// <param name="floors">Number of floors (must be between 1 and 10).</param>
    /// <param name="height">Height of the building in units.</param>
    /// <param name="width">Width of the building in units.</param>
    /// <param name="depth">Depth of the building in units.</param>
    /// <param name="x">X-coordinate of the building's position.</param>
    /// <param name="y">Y-coordinate of the building's position.</param>
    /// <param name="z">Z-coordinate of the building's position.</param>
    /// <param name="color">Color code of the building (e.g., "#FF0000").</param>
    /// <returns>A fully constructed and valid <see cref="Building"/> instance.</returns>
    public static Building CreateValidBuilding(
        string id = "B001",
        string name = "ECCI",
        int floors = 3,
        int height = 100,
        int width = 50,
        int depth = 30,
        int x = 10,
        int y = 20,
        int z = 5,
        string color = "#FF0000",
        string texture = "default_texture.png")
    {
        return new Building(
            BuildingOfficialId.Create(id),
            BuildingName.Create(name),
            FloorCount.Create(floors),
            new BuildingRenderInfo(
                Color.Create(color),
                Heigth.Create(height),
                Width.Create(width),
                Depth.Create(depth),
                X.Create(x),
                Y.Create(y),
                Z.Create(z),
                BuildingTexture.Create(texture)
            )
        );
    }
}
