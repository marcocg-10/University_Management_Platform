using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.LearningSpaces.Repositories;

/// <summary>
/// Contains unit tests for the learning space repository.
/// </summary>
public class LearningSpaceRepositoryTests : IClassFixture<LearningSpaceRepositoryTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceRepositoryTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceRepositoryTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceRepositoryTests(LearningSpaceRepositoryTestData testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving no data.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListLaboratoriesAsync_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var laboratoriesDbSetMock = _testData.LaboratoryEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var laboratories = await sut.ListLaboratoriesAsync();

        // Assert
        laboratories.Should().BeEmpty(because: "there are no laboratories in the database");
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving one laboratory.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListLaboratoriesAsync_WhenGivenSingleEntryData_ReturnsData()
    {
        // Arrange
        var laboratoriesDbSetMock = _testData.LaboratorySingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var laboratories = await sut.ListLaboratoriesAsync();

        // Assert
        laboratories.Should().BeEquivalentTo(_testData.LaboratorySingleEntryData,
            because: "should return data given by database");
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving multiple laboratories.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListLaboratoriesAsync_WhenGivenMultipleEntryData_ReturnsData()
    {
        // Arrange
        var laboratoriesDbSetMock = _testData.LaboratoryMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var laboratories = await sut.ListLaboratoriesAsync();

        // Assert
        laboratories.Should().BeEquivalentTo(_testData.LaboratoryMultipleEntryData,
            because: "should return data given by database");
    }

    /// <summary>
    /// Unit tests for getting a laboratory by ID through the repository.
    /// </summary>

    [Fact]
    public async Task GetLaboratoryByIdAsync_WhenLaboratoryExists_ReturnsLaboratory()
    {
        // Arrange
        var expectedLaboratory = _testData.LaboratorySingleEntryData.First();
        var laboratoriesDbSetMock = _testData.LaboratorySingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLaboratoryByIdAsync(expectedLaboratory.Id);

        // Assert
        result.Should().NotBeNull(because: "the laboratory exists in the database");
        result.Should().BeEquivalentTo(expectedLaboratory, because: "should return the correct laboratory");
    }

    [Fact]
    public async Task GetLaboratoryByIdAsync_WhenLaboratoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var laboratoriesDbSetMock = _testData.LaboratoryEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Laboratories)
            .Returns(laboratoriesDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetLaboratoryByIdAsync(999);

        // Assert
        result.Should().BeNull(because: "the laboratory does not exist in the database");
    }

    [Fact]
    public async Task GetClassroomByIdAsync_WhenClassroomExists_ReturnsClassroom()
    {
        // Arrange
        var expectedClassroom = _testData.ClassroomSingleEntryData.First();
        var classroomsDbSetMock = _testData.ClassroomSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetClassroomByIdAsync(expectedClassroom.Id);

        // Assert
        result.Should().NotBeNull(because: "the classroom exists in the database");
        result.Should().BeEquivalentTo(expectedClassroom, because: "should return the correct classroom");
    }

    [Fact]
    public async Task GetClassroomByIdAsync_WhenClassroomDoesNotExist_ReturnsNull()
    {
        // Arrange
        var classroomsDbSetMock = _testData.ClassroomEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var result = await sut.GetClassroomByIdAsync(999);

        // Assert
        result.Should().BeNull(because: "the classroom does not exist in the database");
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving no data.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListClassroomsAsync_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var classroomsDbSetMock = _testData.ClassroomEmptyData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var classrooms = await sut.ListClassroomsAsync();

        // Assert
        classrooms.Should().BeEmpty(because: "there are no classrooms in the database");
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving one classroom.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListClassroomsAsync_WhenGivenSingleEntryData_ReturnsData()
    {
        // Arrange
        var classroomsDbSetMock = _testData.ClassroomSingleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var classrooms = await sut.ListClassroomsAsync();

        // Assert
        classrooms.Should().BeEquivalentTo(_testData.ClassroomSingleEntryData,
            because: "should return data given by database");
    }

    /// <summary>
    /// Tests whether the LearningSpaceRepository behaves correctly when receiving multiple classrooms.
    /// </summary>
    /// <returns>Task object that represents the current test.</returns>
    [Fact]
    public async Task ListClassroomsAsync_WhenGivenMultipleEntryData_ReturnsData()
    {
        // Arrange
        var classroomsDbSetMock = _testData.ClassroomMultipleEntryData.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Classrooms)
            .Returns(classroomsDbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        var classrooms = await sut.ListClassroomsAsync();

        // Assert
        classrooms.Should().BeEquivalentTo(_testData.ClassroomMultipleEntryData,
            because: "should return data given by database");
    }

    /// <summary>
    /// Tests that DeleteLearningSpaceAsync removes the learning space from the DbSet and saves changes.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenLearningSpaceExists_RemovesAndSavesChanges()
    {
        // Arrange
        var learningSpaceId = 1;
        var learningSpace = new Laboratory(
            buildingId: 1,
            floorLevel: 1,
            roomId: "Lab-1",
            color: LearningSpaceColor.Create("#FFFFFF"),
            texture: LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            dimensions: LearningSpaceDimensions.Create(5, 5, 3),
            coordinates: LearningSpaceCoordinates.Create(1, 1, 1));

        typeof(LearningSpace)
            .GetProperty(nameof(LearningSpace.Id))!
            .SetValue(learningSpace, learningSpaceId);

        var learningSpaces = new List<LearningSpace> { learningSpace };
        var dbSetMock = learningSpaces.BuildMockDbSet();

        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(c => c.LearningSpaces).Returns(dbSetMock.Object);
        dbContextMock
            .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        await sut.DeleteLearningSpaceAsync(learningSpaceId);

        // Assert
        dbSetMock.Verify(d => d.Remove(It.Is<LearningSpace>(ls => ls.Id == learningSpaceId)),
            Times.Once,
            "repository should remove the learning space from the DbSet");

        dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once,
            "repository should call SaveChangesAsync after deleting");
    }

    /// <summary>
    /// Tests that DeleteLearningSpaceAsync throws LearningSpaceNotFoundException when the learning space does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenLearningSpaceDoesNotExist_ThrowsLearningSpaceNotFoundException()
    {
        // Arrange
        var dbSetMock = new List<LearningSpace>().BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock.Setup(c => c.LearningSpaces).Returns(dbSetMock.Object);

        var sut = new LearningSpaceRepository(dbContextMock.Object);

        // Act
        Func<Task> act = async () => await sut.DeleteLearningSpaceAsync(999);

        // Assert
        await act.Should().ThrowAsync<UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions.LearningSpaceNotFoundException>();
    }
}
