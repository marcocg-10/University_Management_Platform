using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

/// <summary>
/// Contains unit tests for the ListLearningSpacesByBuildingIdAsync method.
/// </summary>
public class LearningSpaceServiceListLearningSpaceByBuildingIdTests
    : IClassFixture<LearningSpaceServiceTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceServiceTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceServiceListLearningSpaceByBuildingIdTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceServiceListLearningSpaceByBuildingIdTests(
        LearningSpaceServiceTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenLearningSpacesExistForBuildingId_ReturnsLearningSpaces()
    {
        // Arrange
        var buildingId = 3;
        var expectedLearningSpaces = _testData.LearningSpaceMultipleEntryData
            .Where(ls => ls.BuildingId == buildingId).ToList();

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.ListLearningSpacesByBuildingIdAsync(buildingId))
            .ReturnsAsync(expectedLearningSpaces);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingId);

        // Assert
        result.Should().BeEquivalentTo(expectedLearningSpaces, because: "should return learning spaces that match the building ID");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenSingleLearningSpaceExists_ReturnsSingleLearningSpace()
    {
        // Arrange
        var buildingId = 3;
        var expectedLearningSpaces = _testData.LearningSpaceSingleEntryData;

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.ListLearningSpacesByBuildingIdAsync(buildingId))
            .ReturnsAsync(expectedLearningSpaces);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingId);

        // Assert
        result.Should().BeEquivalentTo(expectedLearningSpaces, because: "should return the single learning space for the building ID");
        result.Should().HaveCount(1, because: "there should be exactly one learning space for this building");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenMultipleLearningSpacesExist_ReturnsLaboratory()
    {
        // Arrange
        var buildingId = 3;
        var expectedLaboratory = _testData.LearningSpaceMultipleEntryData[0];  // Laboratory
        var filteredSpaces = _testData.LearningSpaceMultipleEntryData
            .Where(ls => ls.BuildingId == buildingId && ls is Laboratory).ToList();

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.ListLearningSpacesByBuildingIdAsync(buildingId))
            .ReturnsAsync(filteredSpaces);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingId);

        // Assert
        result.Should().Contain(expectedLaboratory, because: "should return the laboratory for the specified building ID");
        result.OfType<Laboratory>().Should().NotBeEmpty(because: "should contain laboratory learning spaces");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenMultipleLearningSpacesExist_ReturnsClassroom()
    {
        // Arrange
        var buildingId = 4;
        var expectedClassroom = _testData.LearningSpaceMultipleEntryData[1];  // Classroom
        var filteredSpaces = _testData.LearningSpaceMultipleEntryData
            .Where(ls => ls.BuildingId == buildingId && ls is Classroom).ToList();

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.ListLearningSpacesByBuildingIdAsync(buildingId))
            .ReturnsAsync(filteredSpaces);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingId);

        // Assert
        result.Should().Contain(expectedClassroom, because: "should return the classroom for the specified building ID");
        result.OfType<Classroom>().Should().NotBeEmpty(because: "should contain classroom learning spaces");
    }

    [Fact]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenNoLearningSpacesExistForBuildingId_ReturnsEmptyCollection()
    {
        // Arrange
        var buildingId = 999;
        var emptyResult = new List<LearningSpace>();

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.ListLearningSpacesByBuildingIdAsync(buildingId))
            .ReturnsAsync(emptyResult);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ListLearningSpacesByBuildingIdAsync(buildingId);

        // Assert
        result.Should().BeEmpty(because: "should return empty collection when no learning spaces exist for the building ID");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-999)]
    public async Task ListLearningSpacesByBuildingIdAsync_WhenBuildingIdIsInvalid_ThrowsValidationException(int invalidBuildingId)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.ListLearningSpacesByBuildingIdAsync(invalidBuildingId))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "building ID must be a positive number")
            .WithMessage("Building ID must be a positive number.", because: "the exception message should indicate the validation error");
    }
}