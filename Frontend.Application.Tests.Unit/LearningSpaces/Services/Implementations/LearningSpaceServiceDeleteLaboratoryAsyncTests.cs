using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceDeleteLaboratoryAsyncTests
{
    private static int ValidId() => 1;

    [Fact]
    public async Task DeleteLaboratoryAsync_WithValidId_CallsRepositoryOnce()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        // Act
        await sut.DeleteLaboratoryAsync(validId);

        // Assert
        repoMock.Verify(
            r => r.DeleteLaboratoryAsync(validId),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteLaboratoryAsync_WhenRepositoryThrowsDomainException_PropagatesDomainException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        var domainException = new DomainException("Laboratory not found");

        repoMock
            .Setup(r => r.DeleteLaboratoryAsync(It.IsAny<int>()))
            .ThrowsAsync(domainException);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteLaboratoryAsync(validId))
            .Should()
            .ThrowExactlyAsync<DomainException>(because: "the laboratory was not found or a conflict occurred");
    }

    [Fact]
    public async Task DeleteLaboratoryAsync_WhenRepositoryThrowsArgumentException_PropagatesArgumentException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        var argumentException = new ArgumentException("Invalid laboratory ID");

        repoMock
            .Setup(r => r.DeleteLaboratoryAsync(It.IsAny<int>()))
            .ThrowsAsync(argumentException);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteLaboratoryAsync(validId))
            .Should()
            .ThrowExactlyAsync<ArgumentException>(because: "the repository detected an invalid argument");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task DeleteLaboratoryAsync_WhenIdIsInvalid_ThrowsValidationException_AndRepositoryIsNotCalled(int invalidId)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteLaboratoryAsync(invalidId))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the laboratory ID is invalid");

        repoMock.Verify(r => r.DeleteLaboratoryAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteLaboratoryAsync_WithValidId_CompletesSuccessfully()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        // Setup repository to complete successfully (no exceptions)
        repoMock
            .Setup(r => r.DeleteLaboratoryAsync(validId))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteLaboratoryAsync(validId))
            .Should()
            .NotThrowAsync(because: "the operation should complete successfully with a valid ID");

        repoMock.Verify(
            r => r.DeleteLaboratoryAsync(validId),
            Times.Once);
    }
}