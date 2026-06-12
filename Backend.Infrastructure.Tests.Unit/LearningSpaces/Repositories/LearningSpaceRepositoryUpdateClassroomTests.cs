using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

/// <summary>
/// Contains unit tests for the UpdateClassroomAsync method.
/// </summary>
public class LearningSpaceRepositoryUpdateClassroomTests
    : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryUpdateClassroomTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryUpdateClassroomTests(
        LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task UpdateClassroomAsync_WithValidClassroom_UpdatesAndSavesChanges()
    {
        // Arrange
        var existingClassroom = _testData.ClassroomSingleEntryData[0];
        var classroomsDbSetMock = _testData.ClassroomSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedClassroom = new Classroom(
            id: existingClassroom.Id,
            buildingId: 2,
            floorLevel: 3,
            roomId: "Class-Updated",
            color: LearningSpaceColor.Create("#FF0000"),
            texture: LearningSpaceTexture.Create("NewTexture.png"),
            dimensions: LearningSpaceDimensions.Create(10, 15, 4),
            coordinates: LearningSpaceCoordinates.Create(5, 5, 5));

        // Act
        await sut.UpdateClassroomAsync(updatedClassroom);

        // Assert
        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should call SaveChangesAsync after updating the classroom");
    }

    [Fact]
    public async Task UpdateClassroomAsync_WhenClassroomDoesNotExist_ThrowsLearningSpaceNotFoundException()
    {
        // Arrange
        var classroomsDbSetMock = _testData.ClassroomEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var nonExistentClassroom = new Classroom(
            id: 999,
            buildingId: 1,
            floorLevel: 1,
            roomId: "Class-NonExistent",
            color: LearningSpaceColor.Create("#FFFFFF"),
            texture: LearningSpaceTexture.Create("Texture.png"),
            dimensions: LearningSpaceDimensions.Create(5, 5, 3),
            coordinates: LearningSpaceCoordinates.Create(0, 0, 0));

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(nonExistentClassroom))
            .Should()
            .ThrowExactlyAsync<LearningSpaceNotFoundException>(
                because: "the classroom does not exist in the database")
            .WithMessage("*999*",
                because: "the exception message should include the classroom ID");

        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "repository should not call SaveChangesAsync when classroom doesn't exist");
    }

    [Fact]
    public async Task UpdateClassroomAsync_WhenSaveThrowsDuplicate_PropagatesDuplicateValueInEntityException()
    {
        // Arrange
        var existingClassroom = _testData.ClassroomSingleEntryData[0];
        var classroomsDbSetMock = _testData.ClassroomSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var expected = new DuplicateValueInEntityException(
            entityName: "Classroom",
            propertyName: "UNIQUE_Room_Building",
            duplicateValue: "Class-1, 1");

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedClassroom = new Classroom(
            id: existingClassroom.Id,
            buildingId: existingClassroom.BuildingId,
            floorLevel: existingClassroom.FloorLevel,
            roomId: "Class-Duplicate",
            color: existingClassroom.Color,
            texture: existingClassroom.Texture,
            dimensions: existingClassroom.Dimensions,
            coordinates: existingClassroom.Coordinates);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(updatedClassroom))
            .Should()
            .ThrowExactlyAsync<DuplicateValueInEntityException>(
                because: "a classroom with the same room ID and building ID already exists")
            .WithMessage("*UNIQUE_Room_Building*",
                because: "the exception message should include the constraint name");
    }

    [Fact]
    public async Task UpdateClassroomAsync_WhenSaveThrowsForeignKey_PropagatesForeignKeyException()
    {
        // Arrange
        var existingClassroom = _testData.ClassroomSingleEntryData[0];
        var classroomsDbSetMock = _testData.ClassroomSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var expected = new ForeignKeyException(
            constraintName: "FK_LearningSpace_Building",
            tableName: "Building");

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedClassroom = new Classroom(
            id: existingClassroom.Id,
            buildingId: 999,  // Non-existent building
            floorLevel: existingClassroom.FloorLevel,
            roomId: existingClassroom.RoomId,
            color: existingClassroom.Color,
            texture: existingClassroom.Texture,
            dimensions: existingClassroom.Dimensions,
            coordinates: existingClassroom.Coordinates);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(updatedClassroom))
            .Should()
            .ThrowExactlyAsync<ForeignKeyException>(
                because: "the building reference does not exist in the database")
            .WithMessage("*FK_LearningSpace_Building*",
                because: "the exception message should include the constraint name");
    }
}