using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

/// <summary>
/// Contains unit tests for the ListLearningSpacesByBuildingIdAsync method.
/// </summary>
public class LearningSpaceRepositoryListsLearningSpaceByBuildingIdTests
    : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryListsLearningSpaceByBuildingIdTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryListsLearningSpaceByBuildingIdTests(
        LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenLearningSpacesExistForBuildingId_ReturnsMatchingLearningSpaces()
    {
        // Arrange
        var targetBuildingId = 3;
        var expectedLearningSpaces = _testData.LearningSpaceMultipleEntryData
            .Where(ls => ls.BuildingId == targetBuildingId).ToList();
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(targetBuildingId);

        // Assert
        result.Should().BeEquivalentTo(expectedLearningSpaces, because: "should return only learning spaces that match the building ID");
        result.Should().HaveCount(1, because: "there is only one learning space with building ID 3 in the test data");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenMultipleLearningSpacesExistForSameBuildingId_ReturnsAllMatchingLearningSpaces()
    {
        // Arrange
        var targetBuildingId = 4;
        var expectedLearningSpaces = _testData.LearningSpaceMultipleEntryData
            .Where(ls => ls.BuildingId == targetBuildingId).ToList();
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(targetBuildingId);

        // Assert
        result.Should().BeEquivalentTo(expectedLearningSpaces, because: "should return all learning spaces that match the building ID");
        result.Should().HaveCount(1, because: "there is only one learning space with building ID 4 in the test data");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenSingleLearningSpaceExistsForBuildingId_ReturnsSingleLearningSpace()
    {
        // Arrange
        var targetBuildingId = 3;
        var expectedLearningSpace = _testData.LearningSpaceSingleEntryData[0];
        var learningSpacesDbSetMock = _testData.LearningSpaceSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(targetBuildingId);

        // Assert
        result.Should().ContainSingle(because: "should return exactly one learning space matching the building ID");
        result.First().Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(999)]
    [InlineData(0)]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenNoLearningSpacesExistForBuildingId_ReturnsEmptyCollection(int buildingIdThatDoesNotExist)
    {
        // Arrange
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingIdThatDoesNotExist);

        // Assert
        result.Should().BeEmpty(because: "should return empty collection when no learning spaces match the building ID");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenEmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange
        var targetBuildingId = 1;
        var learningSpacesDbSetMock = _testData.LearningSpaceEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(targetBuildingId);

        // Assert
        result.Should().BeEmpty(because: "should return empty collection when the database contains no learning spaces");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenLearningSpacesHaveNullBuildingId_DoesNotReturnThoseSpaces()
    {
        // Arrange
        var targetBuildingId = 1;
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(targetBuildingId);

        // Assert
        result.Should().NotContain(ls => ls.BuildingId == null, because: "should not return learning spaces with null building ID when filtering by a specific building ID");
        result.All(ls => ls.BuildingId == targetBuildingId).Should().BeTrue(because: "all returned learning spaces should have the target building ID");
    }
}