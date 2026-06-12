using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Buildings.Services.Implementations;

/// <summary>
/// Provides test data for unit tests of the <see cref="BuildingService"/> class.
/// Includes various building entity collections and sample entries for different test scenarios.
/// </summary>
public class BuildingServiceTestData
{
    /// <summary>
    /// Gets an empty list of buildings for testing scenarios with no data.
    /// </summary>
    public List<Building> BuildingEmptyData { get; } = [];

    /// <summary>
    /// Gets a single building entry for use in unit tests.
    /// </summary>
    public Building BuildingEntry { get; } =
        new Building(
            BuildingOfficialId.Create("B001"),
            BuildingName.Create("ECCI"),
            FloorCount.Create(3),
            new BuildingRenderInfo(
                Color.Create("#FFF"),
                Heigth.Create(1222),
                Width.Create(500),
                Depth.Create(300),
                X.Create(6),
                Y.Create(67),
                Z.Create(545),
                BuildingTexture.Create("Default_texture.png"))
            );

    /// <summary>
    /// Gets a list containing a single building for testing single-entity scenarios.
    /// </summary>
    public List<Building> BuildingSingleData { get; } = [
        new Building(
        BuildingOfficialId.Create("B001"),
        BuildingName.Create("ECCI"),
        FloorCount.Create(3),
        new BuildingRenderInfo(
            Color.Create("#FFF"),
            Heigth.Create(1222),
            Width.Create(500),
            Depth.Create(300),
            X.Create(6),
            Y.Create(67),
            Z.Create(545),
            BuildingTexture.Create("Default_texture.png"))
        )];

    /// <summary>
    /// Gets a list containing multiple buildings for testing scenarios with several entities.
    /// </summary>
    public List<Building> BuildingMultipleData { get; } = [
        new Building(
            BuildingOfficialId.Create("B002"),
            BuildingName.Create("ECCI"),
            FloorCount.Create(3),
            new BuildingRenderInfo(
                Color.Create("#FFF"),
                Heigth.Create(1222),
                Width.Create(500),
                Depth.Create(300),
                X.Create(6),
                Y.Create(67),
                Z.Create(545),
                BuildingTexture.Create("Default_texture.png"))
            ),
        new Building(
            BuildingOfficialId.Create("B003"),
            BuildingName.Create("Tinoco Library"),
            FloorCount.Create(5),
            new BuildingRenderInfo(
                Color.Create("#000"),
                Heigth.Create(1500),
                Width.Create(600),
                Depth.Create(400),
                X.Create(10),
                Y.Create(20),
                Z.Create(30),
                BuildingTexture.Create("Default_texture.png"))
            ),
        new Building(
            BuildingOfficialId.Create("B004"),
            BuildingName.Create("Economics"),
            FloorCount.Create(4),
            new BuildingRenderInfo(
                Color.Create("#123456"),
                Heigth.Create(800),
                Width.Create(300),
                Depth.Create(200),
                X.Create(5),
                Y.Create(15),
                Z.Create(25),
                BuildingTexture.Create("Default_texture.png"))
            )
    ];
}