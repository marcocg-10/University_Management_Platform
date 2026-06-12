using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Buildings.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Building"/> class,
/// verifying correct property assignment and constructor behavior.
/// </summary>
public class BuildingTests
{
    private readonly string _officialId;
    private readonly string _name;
    private readonly int _floorCount;
    private readonly BuildingRenderInfo _renderInfo;

    /// <summary>
    /// Initializes test input data for <see cref="Building"/> unit tests.
    /// </summary>
    public BuildingTests()
    {
        _officialId = "B001";
        _name = "ECCI";
        _floorCount = 3;
        _renderInfo = new BuildingRenderInfo("#FFF", 100, 50, 30, 10, 20, 5, "Default_texture.png");
    }

    /// <summary>
    /// Verifies that the constructor correctly sets the <see cref="Building.OfficialId"/> property.
    /// </summary>
    [Fact]
    public void Ctor_GivenValidArguments_SetsOfficialIdProperty()
    {
        // Arrange

        // Act
        var building = new Building
            (_officialId,
            _name, 
            _floorCount, 
            _renderInfo);

        // Assert
        building.OfficialId.Should().Be(_officialId, because: "the constructor should assign the official ID");
    }

    /// <summary>
    /// Verifies that the constructor correctly sets the <see cref="Building.Name"/> property.
    /// </summary>
    [Fact]
    public void Ctor_GivenValidArguments_SetsNameProperty()
    {
        // Arrange

        // Act
        var building = new Building
            (_officialId, 
            _name, 
            _floorCount, 
            _renderInfo);

        // Assert
        building.Name.Should().Be(_name, because: "the constructor should assign the name");
    }

    /// <summary>
    /// Verifies that the constructor correctly sets the <see cref="Building.FloorCount"/> property.
    /// </summary>
    [Fact]
    public void Ctor_GivenValidArguments_SetsFloorCountProperty()
    {
        // Arrange

        // Act
        var building = new Building
            (_officialId,
            _name,
            _floorCount,
            _renderInfo);

        // Assert
        building.FloorCount.Should().Be(_floorCount, because: "the constructor should assign the floor count");
    }

    /// <summary>
    /// Verifies that the constructor correctly sets the <see cref="Building.RenderInfo"/> property.
    /// </summary>
    [Fact]
    public void Ctor_GivenValidArguments_SetsRenderInfoProperty()
    {
        // Arrange

        // Act
        var building = new Building
            (_officialId,
            _name,
            _floorCount,
            _renderInfo);

        // Assert
        building.RenderInfo.Should().Be(_renderInfo, because: "the constructor should assign the render info");
    }
}
