using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.TestData;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.Services.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="BuildingCollisionService"/> class,
/// verifying its behavior when checking for building collisions.
/// </summary>
public class BuildingCollisionServiceTests
{
    /// <summary>
    /// Verifies that <see cref="BuildingCollisionService.HasCollisionAsync"/> returns true
    /// when the collision detector reports a collision between the target building and existing buildings.
    /// </summary>
    [Fact]
    public async Task HasCollisionAsync_WhenDetectorFindsCollision_ReturnsTrue()
    {
        // Arrange: Create a building to check and one existing building that overlaps in position.
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20) };

        // Mock the repository to return the existing buildings.
        var repoMock = new Mock<IBuildingRepository>();
        repoMock
            .Setup(r => r.GetBuildingsAsync())
            .ReturnsAsync(existingBuildings);

        // Mock the collision detector to report a collision.
        var detectorMock = new Mock<IBuildingCollisionDetector>();
        detectorMock
            .Setup(d => d.HasCollision(buildingToCheck, existingBuildings))
            .Returns(true);

        // Create the service under test (SUT).
        var sut = new BuildingCollisionService(repoMock.Object, detectorMock.Object);

        // Act: Check for collision.
        var result = await sut.HasCollisionAsync(buildingToCheck);

        // Assert: Expect true because the detector reported a collision.
        result.Should().BeTrue(because: "the detector reported a collision");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionService.HasCollisionAsync"/> returns false
    /// when the collision detector reports no collision between the target building and existing buildings.
    /// </summary>
    [Fact]
    public async Task HasCollisionAsync_WhenDetectorFindsNoCollision_ReturnsFalse()
    {
        // Arrange: Create a building to check and one existing building far away.
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 100, y: 100) };

        // Mock the repository to return the existing buildings.
        var repoMock = new Mock<IBuildingRepository>();
        repoMock
            .Setup(r => r.GetBuildingsAsync())
            .ReturnsAsync(existingBuildings);

        // Mock the collision detector to report no collision.
        var detectorMock = new Mock<IBuildingCollisionDetector>();
        detectorMock
            .Setup(d => d.HasCollision(buildingToCheck, existingBuildings))
            .Returns(false);

        // Create the service under test (SUT).
        var sut = new BuildingCollisionService(repoMock.Object, detectorMock.Object);

        // Act: Check for collision.
        var result = await sut.HasCollisionAsync(buildingToCheck);

        // Assert: Expect false because the detector reported no collision.
        result.Should().BeFalse(because: "the detector reported no collision");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionService.HasCollisionAsync"/> with exclusion returns false
    /// when the building is only colliding with itself.
    /// </summary>
    [Fact]
    public async Task HasCollisionAsync_WhenExcludingSelfAndOnlyCollidesWithSelf_ReturnsFalse()
    {
        // Arrange: Create a building that exists in the system.
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[] { BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20) };

        // Mock the repository to return the existing buildings.
        var repoMock = new Mock<IBuildingRepository>();
        repoMock
            .Setup(r => r.GetBuildingsAsync())
            .ReturnsAsync(existingBuildings);

        // Mock the collision detector to report no collision when excluding self.
        var detectorMock = new Mock<IBuildingCollisionDetector>();
        detectorMock
            .Setup(d => d.HasCollision(buildingToCheck, existingBuildings, "B001"))
            .Returns(false);

        // Create the service under test (SUT).
        var sut = new BuildingCollisionService(repoMock.Object, detectorMock.Object);

        // Act: Check for collision with self-exclusion.
        var result = await sut.HasCollisionAsync(buildingToCheck, "B001");

        // Assert: Expect false because the building is excluded.
        result.Should().BeFalse(because: "the building should be excluded from collision check with itself");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionService.HasCollisionAsync"/> with exclusion returns true
    /// when the building collides with another building after excluding itself.
    /// </summary>
    [Fact]
    public async Task HasCollisionAsync_WhenExcludingSelfButCollidesWithOther_ReturnsTrue()
    {
        // Arrange: Create a building that collides with another building.
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[]
        {
            BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20),
            BuildingDataTests.CreateValidBuilding(id: "B002", x: 15, y: 25)
        };

        // Mock the repository to return the existing buildings.
        var repoMock = new Mock<IBuildingRepository>();
        repoMock
            .Setup(r => r.GetBuildingsAsync())
            .ReturnsAsync(existingBuildings);

        // Mock the collision detector to report collision with B002.
        var detectorMock = new Mock<IBuildingCollisionDetector>();
        detectorMock
            .Setup(d => d.HasCollision(buildingToCheck, existingBuildings, "B001"))
            .Returns(true);

        // Create the service under test (SUT).
        var sut = new BuildingCollisionService(repoMock.Object, detectorMock.Object);

        // Act: Check for collision with self-exclusion.
        var result = await sut.HasCollisionAsync(buildingToCheck, "B001");

        // Assert: Expect true because the building collides with B002.
        result.Should().BeTrue(because: "the building collides with B002 even though B001 is excluded");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingCollisionService.HasCollisionAsync"/> with exclusion returns false
    /// when the building doesn't collide with any other building after excluding itself.
    /// </summary>
    [Fact]
    public async Task HasCollisionAsync_WhenExcludingSelfAndNoOtherCollisions_ReturnsFalse()
    {
        // Arrange: Create a building that doesn't collide with any other building.
        var buildingToCheck = BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20);
        var existingBuildings = new[]
        {
            BuildingDataTests.CreateValidBuilding(id: "B001", x: 10, y: 20),
            BuildingDataTests.CreateValidBuilding(id: "B002", x: 100, y: 100)
        };

        // Mock the repository to return the existing buildings.
        var repoMock = new Mock<IBuildingRepository>();
        repoMock
            .Setup(r => r.GetBuildingsAsync())
            .ReturnsAsync(existingBuildings);

        // Mock the collision detector to report no collision.
        var detectorMock = new Mock<IBuildingCollisionDetector>();
        detectorMock
            .Setup(d => d.HasCollision(buildingToCheck, existingBuildings, "B001"))
            .Returns(false);

        // Create the service under test (SUT).
        var sut = new BuildingCollisionService(repoMock.Object, detectorMock.Object);

        // Act: Check for collision with self-exclusion.
        var result = await sut.HasCollisionAsync(buildingToCheck, "B001");

        // Assert: Expect false because the building doesn't collide with any other building.
        result.Should().BeFalse(because: "the building is excluded and does not collide with any other building");
    }
}
