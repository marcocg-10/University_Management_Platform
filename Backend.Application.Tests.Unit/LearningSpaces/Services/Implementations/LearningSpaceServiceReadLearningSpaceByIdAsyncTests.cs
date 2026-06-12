using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

/// <summary>
/// Contains unit tests for the ReadLearningSpaceByIdAsync method.
/// </summary>
public class LearningSpaceServiceReadLearningSpaceByIdAsyncTests
    : IClassFixture<LearningSpaceServiceTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceServiceTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceServiceReadLearningSpaceByIdAsyncTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceServiceReadLearningSpaceByIdAsyncTests(
        LearningSpaceServiceTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task ReadLearningSpaceByIdAsync_WhenLearningSpaceExists_ReturnsLearningSpace()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceSingleEntryData[0];

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(expectedLearningSpace.Id))
            .ReturnsAsync(expectedLearningSpace);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Fact]
    public async Task ReadLearningSpaceByIdAsync_WhenMultipleLearningSpacesExist_ReturnsLaboratory()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceMultipleEntryData[0];  // Laboratory

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(expectedLearningSpace.Id))
            .ReturnsAsync(expectedLearningSpace);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Fact]
    public async Task ReadLearningSpaceByIdAsync_WhenMultipleLearningSpacesExist_ReturnsClassroom()
    {
        // Arrange
        var expectedLearningSpace = _testData.LearningSpaceMultipleEntryData[1];  // Classroom

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(expectedLearningSpace.Id))
            .ReturnsAsync(expectedLearningSpace);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLearningSpaceByIdAsync(expectedLearningSpace.Id);

        // Assert
        result.Should().Be(expectedLearningSpace, because: "should return the correct learning space");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(777)]
    public async Task ReadLearningSpaceByIdAsync__WhenLearningSpaceDoesNotExist_ThrowsLearningSpaceNotFoundException(int idThatDoesNotExist)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(idThatDoesNotExist))
            .ThrowsAsync(new LearningSpaceNotFoundException(idThatDoesNotExist));

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        await FluentActions
            .Awaiting(() => sut.ReadLearningSpaceByIdAsync(idThatDoesNotExist))
            .Should()
            .ThrowExactlyAsync<LearningSpaceNotFoundException>(because: "the learning space does not exist")
            .WithMessage($"*{idThatDoesNotExist}*", because: "the exception message should include the invalid id");
    }
}
