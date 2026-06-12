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
/// Contains unit tests for the UpdateLaboratoryAsync method.
/// </summary>
public class LearningSpaceRepositoryUpdateLaboratoryTests
    : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryUpdateLaboratoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryUpdateLaboratoryTests(
        LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    [Fact]
    public async Task UpdateLaboratoryAsync_WithValidLaboratory_UpdatesAndSavesChanges()
    {
        // Arrange
        var existingLaboratory = _testData.LaboratorySingleEntryData[0];
        var laboratoriesDbSetMock = _testData.LaboratorySingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedLaboratory = new Laboratory(
            id: existingLaboratory.Id,
            buildingId: 2,
            floorLevel: 3,
            roomId: "Lab-Updated",
            color: LearningSpaceColor.Create("#FF0000"),
            texture: LearningSpaceTexture.Create("NewTexture.png"),
            dimensions: LearningSpaceDimensions.Create(10, 15, 4),
            coordinates: LearningSpaceCoordinates.Create(5, 5, 5));

        // Act
        await sut.UpdateLaboratoryAsync(updatedLaboratory);

        // Assert

        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should call SaveChangesAsync after updating the laboratory");
    }

    [Fact]
    public async Task UpdateLaboratoryAsync_WhenLaboratoryDoesNotExist_ThrowsLearningSpaceNotFoundException()
    {
        // Arrange
        var laboratoriesDbSetMock = _testData.LaboratoryEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var nonExistentLaboratory = new Laboratory(
            id: 999,
            buildingId: 1,
            floorLevel: 1,
            roomId: "Lab-NonExistent",
            color: LearningSpaceColor.Create("#FFFFFF"),
            texture: LearningSpaceTexture.Create("Texture.png"),
            dimensions: LearningSpaceDimensions.Create(5, 5, 3),
            coordinates: LearningSpaceCoordinates.Create(0, 0, 0));

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(nonExistentLaboratory))
            .Should()
            .ThrowExactlyAsync<LearningSpaceNotFoundException>(
                because: "the laboratory does not exist in the database")
            .WithMessage("*999*",
                because: "the exception message should include the laboratory ID");

        dbContextMock.Verify(
            c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never,
            "repository should not call SaveChangesAsync when laboratory doesn't exist");
    }

    [Fact]
    public async Task UpdateLaboratoryAsync_WhenSaveThrowsDuplicate_PropagatesDuplicateValueInEntityException()
    {
        // Arrange
        var existingLaboratory = _testData.LaboratorySingleEntryData[0];
        var laboratoriesDbSetMock = _testData.LaboratorySingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var expected = new DuplicateValueInEntityException(
            entityName: "Laboratory",
            propertyName: "UNIQUE_Room_Building",
            duplicateValue: "Lab-1, 1");

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedLaboratory = new Laboratory(
            id: existingLaboratory.Id,
            buildingId: existingLaboratory.BuildingId,
            floorLevel: existingLaboratory.FloorLevel,
            roomId: "Lab-Duplicate",
            color: existingLaboratory.Color,
            texture: existingLaboratory.Texture,
            dimensions: existingLaboratory.Dimensions,
            coordinates: existingLaboratory.Coordinates);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(updatedLaboratory))
            .Should()
            .ThrowExactlyAsync<DuplicateValueInEntityException>(
                because: "a laboratory with the same room ID and building ID already exists")
            .WithMessage("*UNIQUE_Room_Building*",
                because: "the exception message should include the constraint name");
    }

    [Fact]
    public async Task UpdateLaboratoryAsync_WhenSaveThrowsForeignKey_PropagatesForeignKeyException()
    {
        // Arrange
        var existingLaboratory = _testData.LaboratorySingleEntryData[0];
        var laboratoriesDbSetMock = _testData.LaboratorySingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(c => c.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var expected = new ForeignKeyException(
            constraintName: "FK_LearningSpace_Building",
            tableName: "Building");

        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expected);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        var updatedLaboratory = new Laboratory(
            id: existingLaboratory.Id,
            buildingId: 999,  // Non-existent building
            floorLevel: existingLaboratory.FloorLevel,
            roomId: existingLaboratory.RoomId,
            color: existingLaboratory.Color,
            texture: existingLaboratory.Texture,
            dimensions: existingLaboratory.Dimensions,
            coordinates: existingLaboratory.Coordinates);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(updatedLaboratory))
            .Should()
            .ThrowExactlyAsync<ForeignKeyException>(
                because: "the building reference does not exist in the database")
            .WithMessage("*FK_LearningSpace_Building*",
                because: "the exception message should include the constraint name");
    }
}
