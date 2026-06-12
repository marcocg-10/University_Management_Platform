using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.Buildings.BuildingServiceDataTests;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Tests.Unit.Application.Buildings.Services;

/// <summary>
/// Contains unit tests for the <see cref="BuildingService"/> class,
/// verifying its behavior when interacting with the building repository.
/// </summary>
public class BuildingServiceTests : IClassFixture<BuildingServiceDataTests>
{
    private readonly BuildingServiceDataTests _testData;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingServiceTests"/> class with test data.
    /// </summary>
    /// <param name="testData">The test data fixture for building service tests.</param>
    public BuildingServiceTests(BuildingServiceDataTests testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.GetBuildingsAsync"/> returns the same data
    /// provided by the repository.
    /// </summary>
    [Fact]
    public async Task GetBuildingsAsync_WhenRepositoryReturnsData_ReturnsSameData()
    {
        // Arrange
        var expectedBuildings = _testData.MultipleBuildings;
        var repoMock = new Mock<IBuildingRepository>();
        repoMock.Setup(r => r.GetBuildingsAsync()).ReturnsAsync(expectedBuildings);

        var sut = new BuildingService(repoMock.Object);

        // Act
        var result = await sut.GetBuildingsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedBuildings, because: "the service should return the same data from the repository");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.GetBuildingsAsync"/> returns an empty collection
    /// when the repository provides no building data.
    /// </summary>
    [Fact]
    public async Task GetBuildingsAsync_WhenRepositoryReturnsEmpty_ReturnsEmptyCollection()
    {
        // Arrange
        var repoMock = new Mock<IBuildingRepository>();
        repoMock.Setup(r => r.GetBuildingsAsync()).ReturnsAsync(Array.Empty<Domain.Buildings.Entities.Building>());

        var sut = new BuildingService(repoMock.Object);

        // Act
        var result = await sut.GetBuildingsAsync();

        // Assert
        result.Should().BeEmpty(because: "the repository returned no buildings");
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.CreateBuildingAsync"/> returns the created building
    /// and invokes the repository once.
    /// </summary>
    [Fact]
    public async Task CreateBuildingAsync_WhenCalled_ReturnsCreatedBuilding()
    {
        // Arrange
        var building = _testData.ValidBuilding;
        var repoMock = new Mock<IBuildingRepository>();
        repoMock.Setup(r => r.CreateBuildingAsync(building)).ReturnsAsync(building);

        var sut = new BuildingService(repoMock.Object);

        // Act
        var result = await sut.CreateBuildingAsync(building);

        // Assert
        result.Should().BeEquivalentTo(building, because: "the service should return the created building");
        repoMock.Verify(r => r.CreateBuildingAsync(building), Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.UpdateBuildingAsync"/> invokes the repository update method.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_WhenCalled_InvokesRepositoryUpdate()
    {
        // Arrange
        var building = _testData.ValidBuilding;
        var repoMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repoMock.Object);

        // Act
        await sut.UpdateBuildingAsync(building);

        // Assert
        repoMock.Verify(r => r.UpdateBuildingAsync(building), Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="BuildingService.DeleteBuildingAsync"/> invokes the repository delete method.
    /// </summary>
    [Fact]
    public async Task DeleteBuildingAsync_WhenCalled_InvokesRepositoryDelete()
    {
        // Arrange
        var officialId = _testData.ValidBuilding.OfficialId;
        var repoMock = new Mock<IBuildingRepository>();
        var sut = new BuildingService(repoMock.Object);

        // Act
        await sut.DeleteBuildingAsync(officialId);

        // Assert
        repoMock.Verify(r => r.DeleteBuildingAsync(officialId), Times.Once);
    }

    /// <summary>
    /// Verifies that the constructor throws <see cref="ArgumentNullException"/> when the repository is null.
    /// </summary>
    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act
        var act = () => new BuildingService(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>(because: "repository is required for service construction");
    }
}
