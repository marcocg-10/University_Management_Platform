using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.TestData;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.CollisionDetector.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="BuildingCollisionDetector"/> class,
/// verifying its collision detection logic between buildings.
/// </summary>
public class BuildingCollisionDetectorTests
{
    /// <summary>
    /// Verifies that <see cref="BuildingCollisionDetector.HasCollision"/> returns true
    /// when the target building overlaps with an existing building.
    /// </summary>
    [Fact]
    public void HasCollision_WhenBuildingsOverlap_ReturnsTrue()
    {
        // Arrange
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20) };

        var sut = new BuildingCollisionDetector();

        // Act
        var result = sut.HasCollision(buildingToCheck, existingBuildings);

        // Assert
        result.Should().BeTrue(because: "the buildings overlap in position and dimensions");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionDetector.HasCollision"/> returns false
    /// when the target building does not overlap with any existing buildings.
    /// </summary>
    [Fact]
    public void HasCollision_WhenBuildingsDoNotOverlap_ReturnsFalse()
    {
        // Arrange
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 100, y: 100) };

        var sut = new BuildingCollisionDetector();

        // Act
        var result = sut.HasCollision(buildingToCheck, existingBuildings);

        // Assert
        result.Should().BeFalse(because: "the buildings are far apart and do not collide");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionDetector.HasCollision"/> with exclusion returns false
    /// when the building is only colliding with itself.
    /// </summary>
    [Fact]
    public void HasCollision_WhenExcludingSelf_ReturnsFalse()
    {
        // Arrange
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20) };

        var sut = new BuildingCollisionDetector();

        // Act
        var result = sut.HasCollision(buildingToCheck, existingBuildings, "B001");

        // Assert
        result.Should().BeFalse(because: "the building should be excluded from collision check with itself");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionDetector.HasCollision"/> with exclusion returns true
    /// when the building collides with another building after excluding itself.
    /// </summary>
    [Fact]
    public void HasCollision_WhenExcludingSelfButCollidesWithOther_ReturnsTrue()
    {
        // Arrange
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[]
        {
            BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20),
            BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25)
        };

        var sut = new BuildingCollisionDetector();

        // Act
        var result = sut.HasCollision(buildingToCheck, existingBuildings, "B001");

        // Assert
        result.Should().BeTrue(because: "the building collides with B002 even though B001 is excluded");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionDetector.HasCollision"/> with exclusion returns false
    /// when the building doesn't collide with any other building after excluding itself.
    /// </summary>
    [Fact]
    public void HasCollision_WhenExcludingSelfAndNoOtherCollisions_ReturnsFalse()
    {
        // Arrange
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[]
        {
            BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20),
            BuildingDataTests.CreateValidBuilding(id: "B002", x: 100, y: 100)
        };

        var sut = new BuildingCollisionDetector();

        // Act
        var result = sut.HasCollision(buildingToCheck, existingBuildings, "B001");

        // Assert
        result.Should().BeFalse(because: "the building is excluded and does not collide with any other building");
    }
}
