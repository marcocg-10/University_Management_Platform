using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Provides predefined test data sets for use in testing building repository functionality.
/// </summary>
/// <remarks>This class contains collections of <see cref="Building"/> objects representing different scenarios,  such
/// as an empty data set, a single entry, and multiple entries. These data sets are intended  to simplify the creation
/// of test cases for building repository operations.</remarks>
public class BuildingRepositoryTestData
{
     /// <summary>
    /// Gets an empty list of buildings.
    /// </summary>
    public List<Building> EmptyData { get; } = [];

    /// <summary>
    /// Gets a predefined list containing a single building entry.
    /// </summary>
    public List<Building> SingleEntryData { get; } = [
        new Building(
            BuildingOfficialId.Create("B001"),
            BuildingName.Create("Main Hall"),
            FloorCount.Create(3),
            new BuildingRenderInfo(
                Color.Create("#c9c9c9"),
                Heigth.Create(15),
                Width.Create(20),
                Depth.Create(30),
                X.Create(5),
                Y.Create(40),
                Z.Create(10),
                BuildingTexture.Create("Default_texture.png"))
         )
      ];

    /// <summary>
    /// Gets a predefined list of buildings containing multiple entries.
    /// </summary>
    
    public List<Building> MultipleEntryData { get; } = [
        new Building(
            BuildingOfficialId.Create("B001"),
            BuildingName.Create("Main Hall"),
            FloorCount.Create(3),
            new BuildingRenderInfo(
                Color.Create("#c9c9c9"),
                Heigth.Create(15),
                Width.Create(20),
                Depth.Create(30),
                X.Create(5),
                Y.Create(40),
                Z.Create(10),
                BuildingTexture.Create("Default_texture.png"))
         ),
        new Building(
            BuildingOfficialId.Create("B002"),
            BuildingName.Create("Science Center"),
            FloorCount.Create(5),
            new BuildingRenderInfo(
                Color.Create("#c9c9c9"),
                Heigth.Create(25),
                Width.Create(30),
                Depth.Create(40),
                X.Create(15),
                Y.Create(50),
                Z.Create(20),
                BuildingTexture.Create("Default_texture.png"))
         )
      ];



}