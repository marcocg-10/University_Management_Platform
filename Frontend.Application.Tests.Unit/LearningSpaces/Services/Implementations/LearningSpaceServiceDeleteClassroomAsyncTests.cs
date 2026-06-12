using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

public class LearningSpaceServiceDeleteClassroomAsyncTests
{
    private static int ValidId() => 1;

    [Fact]
    public async Task DeleteClassroomAsync_WithValidId_CallsRepositoryOnce()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        // Act
        await sut.DeleteClassroomAsync(validId);

        // Assert
        repoMock.Verify(
            r => r.DeleteClassroomAsync(validId),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteClassroomAsync_WhenRepositoryThrowsDomainException_PropagatesDomainException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        var domainException = new DomainException("Classroom not found");

        repoMock
            .Setup(r => r.DeleteClassroomAsync(It.IsAny<int>()))
            .ThrowsAsync(domainException);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteClassroomAsync(validId))
            .Should()
            .ThrowExactlyAsync<DomainException>(because: "the classroom was not found or a conflict occurred");
    }

    [Fact]
    public async Task DeleteClassroomAsync_WhenRepositoryThrowsArgumentException_PropagatesArgumentException()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        var argumentException = new ArgumentException("Invalid classroom ID");

        repoMock
            .Setup(r => r.DeleteClassroomAsync(It.IsAny<int>()))
            .ThrowsAsync(argumentException);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteClassroomAsync(validId))
            .Should()
            .ThrowExactlyAsync<ArgumentException>(because: "the repository detected an invalid argument");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task DeleteClassroomAsync_WhenIdIsInvalid_ThrowsValidationException_AndRepositoryIsNotCalled(int invalidId)
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteClassroomAsync(invalidId))
            .Should()
            .ThrowExactlyAsync<ValidationException>(because: "the classroom ID is invalid");

        repoMock.Verify(r => r.DeleteClassroomAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteClassroomAsync_WithValidId_CompletesSuccessfully()
    {
        // Arrange
        var repoMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repoMock.Object);
        var validId = ValidId();

        // Setup repository to complete successfully (no exceptions)
        repoMock
            .Setup(r => r.DeleteClassroomAsync(validId))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.DeleteClassroomAsync(validId))
            .Should()
            .NotThrowAsync(because: "the operation should complete successfully with a valid ID");

        repoMock.Verify(
            r => r.DeleteClassroomAsync(validId),
            Times.Once);
    }
}
