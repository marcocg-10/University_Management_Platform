using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Buildings.Repositories;

/// <summary>
/// Provides unit tests for the <see cref="BuildingRepository"/> class, focusing on methods that interact with building data in
/// the database.
/// </summary>
public class BuildingRepositoryTests : IClassFixture<BuildingRepositoryTestData>
{
    private readonly BuildingRepositoryTestData _testData;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly BuildingRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingRepositoryTests"/> class with the specified test data.
    /// </summary>
    /// <param name="testData">The test data to be used for the building repository tests. Cannot be <see langword="null"/>.</param>
    public BuildingRepositoryTests(BuildingRepositoryTestData testData)
    {
        _testData = testData;
        _dbContextMock = new Mock<AppDbContext>();
        _repository = new BuildingRepository(_dbContextMock.Object);
    }

    /// <summary>
    /// Tests that <see cref="BuildingRepository.GetBuildingsAsync"/> returns an empty enumerable when the database doesn't
    /// contain buildings.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetBuildingsAsync_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var buildingsDbSetMock = _testData.EmptyData.BuildMockDbSet(); 
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Building)
            .Returns(buildingsDbSetMock.Object);
        var sut = new BuildingRepository(dbContextMock.Object);

        // Act
        var buildings = await sut.GetBuildingsAsync();

        // Assert
        buildings.Should().BeEmpty(because: "There are no active buildings in the database");
    }

    /// <summary>
    /// Tests that the <see cref="BuildingRepository.GetBuildingsAsync"/> method returns the expected data when the
    /// database contains a single building entry.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetBuildingsAsync_WhenGivenSingleEntryData_ReturnsData()
    {
        // Arrange
        var buildingsDbSetMock = _testData.SingleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Building)
            .Returns(buildingsDbSetMock.Object);
        var sut = new BuildingRepository(dbContextMock.Object);

        // Act
        var buildings = await sut.GetBuildingsAsync();

        // Assert
        buildings.Should().BeEquivalentTo(
            _testData.SingleEntryData,
            because: "There are just one active building in the database");
    }

    /// <summary>
    /// Tests the <see cref="BuildingRepository.GetBuildingsAsync"/> method to ensure it returns the expected data when
    /// the database contains multiple building entries.
    /// </summary>
    /// <remarks>This test populates the database with multiple building entries using the test data and asserts that the returned buildings match the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetBuildingsAsync_WhenGivenMultipleEntryData_ReturnsData()
    {
        // Arrange
        var buildingsDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Building)
            .Returns(buildingsDbSetMock.Object);
        var sut = new BuildingRepository(dbContextMock.Object);

        // Act
        var buildings = await sut.GetBuildingsAsync();

        // Assert
        buildings.Should().BeEquivalentTo(
            _testData.MultipleEntryData,
            because: "There are many buildings in the database");
    }

    /// <summary>
    /// Tests that <see cref="BuildingRepository.AddBuildingAsync"/> adds a building to the database and saves changes.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddBuildingAsync_Should_Add_Building_To_Database()
    {
        // Arrange
        var buildings = new List<Building>().BuildMockDbSet();

        _dbContextMock.Setup(db => db.Building)
            .Returns(buildings.Object);

        var newBuilding = new Building(
            BuildingOfficialId.Create("B007"),
            BuildingName.Create("Derecho"),
            FloorCount.Create(5),
            new BuildingRenderInfo(
                Color.Create("#FFF"),
                Heigth.Create(1222),
                Width.Create(500),
                Depth.Create(300),
                X.Create(6),
                Y.Create(67),
                Z.Create(545),
                BuildingTexture.Create("default_texture.png")
            )
        );

        // Act
        await _repository.AddBuildingAsync(newBuilding);

        // Assert
        _dbContextMock.Verify(db => db.Building.AddAsync(newBuilding, default), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
}
