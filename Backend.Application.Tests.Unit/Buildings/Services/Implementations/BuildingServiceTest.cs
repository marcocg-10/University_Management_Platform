using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;


namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Buildings.Services.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="BuildingService"/> class, verifying its behavior
/// when interacting with building entities and the repository.
/// </summary>
public class BuildingServiceTest : IClassFixture<BuildingServiceTestData>
{
    private readonly BuildingServiceTestData _testData;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingServiceTest"/> class with test data.
    /// </summary>
    /// <param name="testData">The test data fixture for building tests.</param>
    public BuildingServiceTest(BuildingServiceTestData testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.GetBuildingsAsync"/> returns an empty collection
    /// when the repository provides no building data.
    /// </summary>
    [Fact]
    public async Task GetAllBuildingsAsync_WhenRepositoryGivesEmptyData_ReturnsEmptyData()
    {
        // arrange
        var collisionServiceMock = new Mock<IBuildingCollisionService>();

        var repositoryMock = new Mock<IBuildingRepository>();
        repositoryMock
            .Setup(repo => repo.GetBuildingsAsync())
            .ReturnsAsync(Array.Empty<Building>);

        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // act
        var buildings = await sut.GetBuildingsAsync();

        // assert
        buildings.Should().BeEmpty(because: "Repository should return empty data");

    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.GetBuildingsAsync"/> returns a single building
    /// when the repository provides one building entity.
    /// </summary>
    [Fact]
    public async Task GetAllBuildingsAsync_WhenRepositoryGivesSingleData_ReturnsSingleData()
    {
        // arrange
        var collisionServiceMock = new Mock<IBuildingCollisionService>();

        var buildings = _testData.BuildingSingleData;
        var repositoryMock = new Mock<IBuildingRepository>();
        repositoryMock
           .Setup(repo => repo.GetBuildingsAsync())
           .ReturnsAsync(_testData.BuildingSingleData);

        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // act
        var buildingsResponse = await sut.GetBuildingsAsync();
        // assert
        buildingsResponse.Should().BeEquivalentTo(_testData.BuildingSingleData, because: "Repository should return single data");
    }


    /// <summary>
    /// Verifies that <see cref="BuildingService.CreateBuildingAsync"/> successfully creates a building
    /// when provided with valid parameters.
    /// </summary>
    [Fact]
    public async Task CreateBuildingAsync_WhenGivenValidParameters_ShouldCreateBuilding()
    {
        // Arrange
        var building = new Building(
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
                BuildingTexture.Create("Default_texture.png")
            )
        );

        var collisionServiceMock = new Mock<IBuildingCollisionService>();

        var repositoryMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // Act
        var createdBuilding = await sut.CreateBuildingAsync(building);

        // Assert
        createdBuilding.Should().NotBeNull(because: "a valid building should be returned");
        createdBuilding.Should().BeEquivalentTo(building, because: "the created building should match the input building");

        repositoryMock.Verify(
            r => r.AddBuildingAsync(It.IsAny<Building>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.UpdateBuildingAsync"/> successfully updates a building
    /// when there are no collisions with other buildings.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_WhenNoCollisionWithOtherBuildings_ShouldUpdateBuilding()
    {
        // Arrange
        var building = new Building(
            BuildingOfficialId.Create("B001"),
            BuildingName.Create("ECCI Updated"),
            FloorCount.Create(4),
            new BuildingRenderInfo(
                Color.Create("#FFF"),
                Heigth.Create(1500),
                Width.Create(600),
                Depth.Create(400),
                X.Create(10),
                Y.Create(70),
                Z.Create(545),
                BuildingTexture.Create("Default_texture.png")
            )
        );

        var collisionServiceMock = new Mock<IBuildingCollisionService>();
        collisionServiceMock
            .Setup(c => c.HasCollisionAsync(building, "B001"))
            .ReturnsAsync(false);

        var repositoryMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // Act
        var updatedBuilding = await sut.UpdateBuildingAsync(building);

        // Assert
        updatedBuilding.Should().NotBeNull(because: "a valid building should be returned");
        updatedBuilding.Should().BeEquivalentTo(building, because: "the updated building should match the input building");

        collisionServiceMock.Verify(
            c => c.HasCollisionAsync(building, "B001"),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateBuildingAsync(It.IsAny<Building>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.UpdateBuildingAsync"/> throws a collision exception
    /// when the building collides with another existing building.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_WhenCollidesWithAnotherBuilding_ShouldThrowCollisionException()
    {
        // Arrange
        var building = new Building(
            BuildingOfficialId.Create("B001"),
            BuildingName.Create("ECCI Updated"),
            FloorCount.Create(4),
            new BuildingRenderInfo(
                Color.Create("#FFF"),
                Heigth.Create(1500),
                Width.Create(600),
                Depth.Create(400),
                X.Create(10),
                Y.Create(70),
                Z.Create(545),
                BuildingTexture.Create("Default_texture.png")
            )
        );

        var collisionServiceMock = new Mock<IBuildingCollisionService>();
        collisionServiceMock
            .Setup(c => c.HasCollisionAsync(building, "B001"))
            .ReturnsAsync(true);

        var repositoryMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // Act
        Func<Task> act = async () => await sut.UpdateBuildingAsync(building);

        // Assert
        await act.Should().ThrowAsync<UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions.BuildingCollisionException>(
            because: "the building collides with another existing building");

        repositoryMock.Verify(
            r => r.UpdateBuildingAsync(It.IsAny<Building>()),
            Times.Never);
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.UpdateBuildingAsync"/> does not throw a collision exception
    /// when updating a building that overlaps only with its own previous position.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_WhenOnlyOverlapsWithSelf_ShouldNotThrowException()
    {
        // Arrange
        var building = new Building(
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
                BuildingTexture.Create("Default_texture.png")
            )
        );

        var collisionServiceMock = new Mock<IBuildingCollisionService>();
        collisionServiceMock
            .Setup(c => c.HasCollisionAsync(building, "B001"))
            .ReturnsAsync(false);

        var repositoryMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repositoryMock.Object, collisionServiceMock.Object);

        // Act
        var updatedBuilding = await sut.UpdateBuildingAsync(building);

        // Assert
        updatedBuilding.Should().NotBeNull(because: "the building should be updated successfully");
        updatedBuilding.Should().BeEquivalentTo(building, because: "the updated building should match the input building");

        collisionServiceMock.Verify(
            c => c.HasCollisionAsync(building, "B001"),
            Times.Once);

        repositoryMock.Verify(
            r => r.UpdateBuildingAsync(It.IsAny<Building>()),
            Times.Once);
    }

}
