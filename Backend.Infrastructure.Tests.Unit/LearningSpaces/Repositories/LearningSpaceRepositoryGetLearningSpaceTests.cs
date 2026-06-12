using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

/// <summary>
/// Contains unit tests for the GetLearningSpace method.
/// </summary>
public class LearningSpaceRepositoryGetLearningSpaceTests 
    : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryGetLearningSpaceTests(
        LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }
    
    [Fact]
    public async Task GetLearningSpaceByIdAsync_WhenLearningSpaceExists_ReturnsLearningSpace()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceSingleEntryData[0];
        var learningSpacesDbSetMock = _testData.LearningSpaceSingleEntryData.BuildMockDbSet();
                
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);
                
        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Fact]
    public async Task GetLearningSpaceByIdAsync_WhenMultipleLearningSpacesExist_ReturnsLaboratory()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceMultipleEntryData[0];  // Laboratory
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Fact]
    public async Task GetLearningSpaceByIdAsync_WhenMultipleLearningSpacesExist_ReturnsClassroom()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceMultipleEntryData[1];  // Classroom
        var learningSpacesDbSetMock = _testData.LearningSpaceMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(777)]
    public async Task GetLearningSpaceByIdAsync_WhenLearningSpaceDoesNotExist_ReturnsNull(int idThatDoesNotExist)
    {
        // Arrange
        var learningSpacesDbSetMock = _testData.LearningSpaceEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.LearningSpaces)
            .Returns(learningSpacesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLearningSpaceByIdAsync(idThatDoesNotExist);

        // Assert
        result.Should().BeNull(because: "should return null when the learning space does not exist");
    }
}
