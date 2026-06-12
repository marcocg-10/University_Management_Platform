using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

/// <summary>
/// LearningSpaceService tests.
/// </summary>
public class LearningSpaceServiceTests
{
    /// <summary>
    /// Tests that <see cref="LearningSpaceService.ListLaboratoriesAsync"/> returns an empty collection when the
    /// repository provides no laboratory data.
    /// </summary>
    /// <remarks>This test verifies that the service correctly handles the scenario where the repository
    /// returns  an empty collection, ensuring that no null values or unexpected results are returned.</remarks>
    /// <returns>A simple Task.</returns>
    [Fact]
    public async Task ListLaboratoriesAsync_WhenRepositoryGivesEmptyData_ReturnsEmptyData()
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(repository => repository.ListLaboratoriesAsync())
            .ReturnsAsync(Array.Empty<Laboratory>());


        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var laboratories = await sut.ListLaboratoriesAsync();

        // Assert
        laboratories.Should().BeEmpty(because: "repository returned empty data");
    }

    /// <summary>
    /// Verifies that the <see cref="LearningSpaceService.ListLaboratoriesAsync"/> method returns the same data as
    /// provided by the <see cref="ILearningSpaceRepository.ListLaboratoriesAsync"/> implementation.
    /// </summary>
    /// <remarks>This test ensures that the service correctly retrieves and returns laboratory data from the
    /// repository without modification. The test uses mocked repository data to validate the behavior.</remarks>
    /// <returns>A simple Task.</returns>
    [Fact]
    public async Task ListLaboratoriesAsync_WhenRepositoryGivesData_ReturnsSameData()
    {
        // Arrange
        var expectedLaboratories = new[]
        {
            new Laboratory(1, 1, 1, "Example1", LearningSpaceColor.Create("#FFFFFF"), LearningSpaceTexture.Create("Brick_Wall_T03_Ambient_occlusion.png"), LearningSpaceDimensions.Create(1.1f, 2.2f, 3.3f), LearningSpaceCoordinates.Create(777.69f, 420.69f, 69.69f)),
            new Laboratory(2, null, null, "Example2", LearningSpaceColor.Create("#000000"), LearningSpaceTexture.Create("Brick_Wall_T03_Ambient_occlusion.png"), LearningSpaceDimensions.Create(0.5f, 3f, 5f), LearningSpaceCoordinates.Create(5.3f, 9.1f, 7.8f))
        };

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(repository => repository.ListLaboratoriesAsync())
            .ReturnsAsync(expectedLaboratories);


        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var laboratories = await sut.ListLaboratoriesAsync();

        // Assert
        laboratories.Should().BeEquivalentTo(expectedLaboratories, because: "repository returned data");
    }

    /// <summary>
    /// Tests that DeleteLaboratoryAsync throws UnauthorizedAccessException when the user is not an admin.
    /// </summary>
    [Fact]
    public async Task DeleteLaboratoryAsync_WhenUserIsNotAdmin_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);
        int laboratoryId = 1;

        // Act
        Func<Task> act = async () => await sut.DeleteLaboratoryAsync(laboratoryId, isAdmin: false);

        // Assert
        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Only administrators can delete laboratories.");
    }

    /// <summary>
    /// Tests that DeleteLaboratoryAsync calls DeleteLearningSpaceAsync on the repository when the user is an admin.
    /// </summary>
    [Fact]
    public async Task DeleteLaboratoryAsync_WhenUserIsAdmin_CallsRepositoryDelete()
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);
        int laboratoryId = 42;

        repositoryMock
            .Setup(repo => repo.DeleteLearningSpaceAsync(laboratoryId))
            .Returns(Task.CompletedTask);

        // Act
        await sut.DeleteLaboratoryAsync(laboratoryId, isAdmin: true);

        // Assert
        repositoryMock.Verify(
            repo => repo.DeleteLearningSpaceAsync(laboratoryId),
            Times.Once,
            "service should call repository deletion once when user is admin");
    }

    /// <summary>
    /// Tests that DeleteLaboratoryAsync propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task DeleteLaboratoryAsync_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);
        int laboratoryId = 99;

        var expectedException = new Exception("Database error");
        repositoryMock
            .Setup(repo => repo.DeleteLearningSpaceAsync(laboratoryId))
            .ThrowsAsync(expectedException);

        // Act
        Func<Task> act = async () => await sut.DeleteLaboratoryAsync(laboratoryId, isAdmin: true);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns a non-null laboratory when given a valid ID.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsNonNullLaboratory()
    {
        // Arrange
        int laboratoryId = 1;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Should().NotBeNull(because: "a valid laboratory should be returned");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync handles complete laboratory configurations correctly.
    /// </summary>
    [Theory]
    [InlineData(1, 1, 1, "Lab-A1", "#FFFFFF", 5.0f, 6.0f, 3.0f, 10.0f, 20.0f, 0.0f)]
    [InlineData(2, 2, 3, "Lab-B2", "#FFFFFF", 7.5f, 8.5f, 4.0f, 15.0f, 25.0f, 1.0f)]
    [InlineData(3, null, null, "Standalone", "#FFFFFF", 10.0f, 12.0f, 5.0f, 0.0f, 0.0f, 0.0f)]
    [InlineData(4, 5, -1, "Basement-Lab", "#FFFFFF", 4.0f, 5.0f, 2.8f, 30.0f, 40.0f, 1.0f)]
    public async Task ReadLaboratoryByIdAsync_WhenGivenCompleteConfigurations_ShouldReturnCorrectLaboratory(
        int laboratoryId, int? buildingId, int? floorLevel, string roomId, string color,
        float width, float length, float height, float x, float y, float z)
    {
        // Arrange
        var expectedLaboratory = new Laboratory(
            laboratoryId, buildingId, floorLevel, roomId,
            LearningSpaceColor.Create(color),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(width, length, height),
            LearningSpaceCoordinates.Create(x, y, z));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Id.Should().Be(laboratoryId, because: "the laboratory ID should match");
        result.RoomId.Should().Be(roomId, because: "the room ID should match");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns correct laboratory ID for different valid IDs.
    /// </summary>
    /// <param name="laboratoryId">Valid laboratory ID to test</param>
    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidIds_ReturnsCorrectLaboratoryId(int laboratoryId)
    {
        // Arrange
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Id.Should().Be(laboratoryId, because: "the laboratory ID should match the requested ID");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct building ID.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectBuildingId()
    {
        // Arrange
        int laboratoryId = 1;
        int? expectedBuildingId = 1;
        var expectedLaboratory = new Laboratory(
            laboratoryId, expectedBuildingId, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.BuildingId.Should().Be(expectedBuildingId, because: "the BuildingId should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct floor level.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectFloorLevel()
    {
        // Arrange
        int laboratoryId = 1;
        int? expectedFloorLevel = 2;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, expectedFloorLevel, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.FloorLevel.Should().Be(expectedFloorLevel, because: "the FloorLevel should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct room ID.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectRoomId()
    {
        // Arrange
        int laboratoryId = 1;
        string expectedRoomId = "Lab101";
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, expectedRoomId,
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.RoomId.Should().Be(expectedRoomId, because: "the RoomId should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct color.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectColor()
    {
        // Arrange
        int laboratoryId = 1;
        string expectedColor = "#FF5733";
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create(expectedColor),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Color.Value.Should().Be(expectedColor, because: "the Color should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct width dimension.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectWidth()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedWidth = 5.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(expectedWidth, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Dimensions.Width.Should().Be(expectedWidth, because: "the Width should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct length dimension.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectLength()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedLength = 6.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, expectedLength, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Dimensions.Length.Should().Be(expectedLength, because: "the Length should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct height dimension.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectHeight()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedHeight = 3.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, expectedHeight),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Dimensions.Height.Should().Be(expectedHeight, because: "the Height should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct X coordinate.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectXCoordinate()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedXCoordinate = 1.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(expectedXCoordinate, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Coordinates.XCoordinate.Should().Be(expectedXCoordinate, because: "the X Coordinate should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct Y coordinate.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectYCoordinate()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedYCoordinate = 2.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, expectedYCoordinate, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Coordinates.YCoordinate.Should().Be(expectedYCoordinate, because: "the Y Coordinate should be correctly read");
    }

    /// <summary>
    /// Tests that ReadLaboratoryByIdAsync returns the correct Z coordinate.
    /// </summary>
    [Fact]
    public async Task ReadLaboratoryByIdAsync_WhenGivenValidId_ReturnsCorrectZCoordinate()
    {
        // Arrange
        int laboratoryId = 1;
        float expectedZCoordinate = 0.0f;
        var expectedLaboratory = new Laboratory(
            laboratoryId, 1, 2, "Lab101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, expectedZCoordinate));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(expectedLaboratory);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadLaboratoryByIdAsync(laboratoryId);

        // Assert
        result.Coordinates.ZCoordinate.Should().Be(expectedZCoordinate, because: "the Z Coordinate should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns a non-null classroom when given a valid ID.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsNonNullClassroom()
    {
        // Arrange
        int classroomId = 1;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Should().NotBeNull(because: "a valid classroom should be returned");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync handles complete classroom configurations correctly.
    /// </summary>
    [Theory]
    [InlineData(1, 1, 1, "Class-A1", "#FFFFFF", 5.0f, 6.0f, 3.0f, 10.0f, 20.0f, 0.0f)]
    [InlineData(2, 2, 3, "Class-B2", "#FFFFFF", 7.5f, 8.5f, 4.0f, 15.0f, 25.0f, 1.0f)]
    [InlineData(3, null, null, "Standalone", "#FFFFFF", 10.0f, 12.0f, 5.0f, 0.0f, 0.0f, 0.0f)]
    [InlineData(4, 5, -1, "Basement-Class", "#FFFFFF", 4.0f, 5.0f, 2.8f, 30.0f, 40.0f, 1.0f)]
    public async Task ReadClassroomByIdAsync_WhenGivenCompleteConfigurations_ShouldReturnCorrectClassroom(
        int classroomId, int? buildingId, int? floorLevel, string roomId, string color,
        float width, float length, float height, float x, float y, float z)
    {
        // Arrange
        var expectedClassroom = new Classroom(
            classroomId, buildingId, floorLevel, roomId,
            LearningSpaceColor.Create(color),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(width, length, height),
            LearningSpaceCoordinates.Create(x, y, z));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Id.Should().Be(classroomId, because: "the classroom ID should match");
        result.RoomId.Should().Be(roomId, because: "the room ID should match");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns correct classroom ID for different valid IDs.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(999)]
    [InlineData(1000)]
    public async Task ReadClassroomByIdAsync_WhenGivenValidIds_ReturnsCorrectClassroomId(int classroomId)
    {
        // Arrange
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Id.Should().Be(classroomId, because: "the classroom ID should match the requested ID");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct building ID.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectBuildingId()
    {
        // Arrange
        int classroomId = 1;
        int? expectedBuildingId = 1;
        var expectedClassroom = new Classroom(
            classroomId, expectedBuildingId, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.BuildingId.Should().Be(expectedBuildingId, because: "the BuildingId should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct floor level.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectFloorLevel()
    {
        // Arrange
        int classroomId = 1;
        int? expectedFloorLevel = 2;
        var expectedClassroom = new Classroom(
            classroomId, 1, expectedFloorLevel, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.FloorLevel.Should().Be(expectedFloorLevel, because: "the FloorLevel should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct room ID.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectRoomId()
    {
        // Arrange
        int classroomId = 1;
        string expectedRoomId = "Class101";
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, expectedRoomId,
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.RoomId.Should().Be(expectedRoomId, because: "the RoomId should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct color.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectColor()
    {
        // Arrange
        int classroomId = 1;
        string expectedColor = "#00FF00";
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create(expectedColor),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Color.Value.Should().Be(expectedColor, because: "the Color should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct width dimension.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectWidth()
    {
        // Arrange
        int classroomId = 1;
        float expectedWidth = 5.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(expectedWidth, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Dimensions.Width.Should().Be(expectedWidth, because: "the Width should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct length dimension.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectLength()
    {
        // Arrange
        int classroomId = 1;
        float expectedLength = 6.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, expectedLength, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Dimensions.Length.Should().Be(expectedLength, because: "the Length should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct height dimension.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectHeight()
    {
        // Arrange
        int classroomId = 1;
        float expectedHeight = 3.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, expectedHeight),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Dimensions.Height.Should().Be(expectedHeight, because: "the Height should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct X coordinate.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectXCoordinate()
    {
        // Arrange
        int classroomId = 1;
        float expectedXCoordinate = 1.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(expectedXCoordinate, 2.0f, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Coordinates.XCoordinate.Should().Be(expectedXCoordinate, because: "the X Coordinate should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct Y coordinate.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectYCoordinate()
    {
        // Arrange
        int classroomId = 1;
        float expectedYCoordinate = 2.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, expectedYCoordinate, 0.0f));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Coordinates.YCoordinate.Should().Be(expectedYCoordinate, because: "the Y Coordinate should be correctly read");
    }

    /// <summary>
    /// Tests that ReadClassroomByIdAsync returns the correct Z coordinate.
    /// </summary>
    [Fact]
    public async Task ReadClassroomByIdAsync_WhenGivenValidId_ReturnsCorrectZCoordinate()
    {
        // Arrange
        int classroomId = 1;
        float expectedZCoordinate = 0.0f;
        var expectedClassroom = new Classroom(
            classroomId, 1, 2, "Class101",
            LearningSpaceColor.Create("#FFFFFF"),
            LearningSpaceTexture.Create("Outdoor_Wall_T15_Ambient_occlusion.png"),
            LearningSpaceDimensions.Create(5.0f, 6.0f, 3.0f),
            LearningSpaceCoordinates.Create(1.0f, 2.0f, expectedZCoordinate));

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(expectedClassroom);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var result = await sut.ReadClassroomByIdAsync(classroomId);

        // Assert
        result.Coordinates.ZCoordinate.Should().Be(expectedZCoordinate, because: "the Z Coordinate should be correctly read");
    }

    /// <summary>
    /// Unit tests for <see cref="LearningSpaceService.ListClassroomsPagedAsync"/>
    /// </summary>
    public class LearningSpaceServiceTests_ListClassroomsPagedAsync
    {
        private readonly Mock<ILearningSpaceRepository> _repositoryMock;
        private readonly LearningSpaceService _service;

        /// <summary>
        /// Initializes mocks and the service under test.
        /// </summary>
        public LearningSpaceServiceTests_ListClassroomsPagedAsync()
        {
            _repositoryMock = new Mock<ILearningSpaceRepository>();
            _service = new LearningSpaceService(_repositoryMock.Object);
        }

        private static Classroom CreateClassroom(int id)
        {
            return new Classroom(
                id,
                1,
                1,
                $"L-{id:D3}",
                LearningSpaceColor.Create("#FFFFFF"),
                LearningSpaceTexture.Create("default.png"),
                LearningSpaceDimensions.Create(10, 10, 3),
                LearningSpaceCoordinates.Create(id, id, 0));
        }

        /// <summary>
        /// Ensures that the service returns the correct page of classrooms and total count.
        /// </summary>
        [Fact]
        public async Task ListClassroomsPagedAsync_Should_Return_Correct_Page_And_TotalCount()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 10;
            var expectedClassrooms = Enumerable.Range(11, 10)
                .Select(CreateClassroom)
                .ToList();
            var expectedTotalCount = 25;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
                .ReturnsAsync((expectedClassrooms, expectedTotalCount));

            // Act
            var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

            // Assert
            classrooms.Should().NotBeNull();
            classrooms.Should().HaveCount(10);
            classrooms.Should().BeEquivalentTo(expectedClassrooms);
            totalCount.Should().Be(expectedTotalCount);

            _repositoryMock.Verify(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null), Times.Once);
        }

        /// <summary>
        /// Ensures that the service returns the first page of classrooms correctly.
        /// </summary>
        [Fact]
        public async Task ListClassroomsPagedAsync_Should_Return_First_Page_Correctly()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var expectedClassrooms = Enumerable.Range(1, 10)
                .Select(CreateClassroom)
                .ToList();
            var expectedTotalCount = 15;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
                .ReturnsAsync((expectedClassrooms, expectedTotalCount));

            // Act
            var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

            // Assert
            classrooms.Should().HaveCount(10);
            classrooms.First().Id.Should().Be(1);
            classrooms.Last().Id.Should().Be(10);
            totalCount.Should().Be(15);
        }

        /// <summary>
        /// Ensures that the service returns the last page of classrooms with remaining items.
        /// </summary>
        [Fact]
        public async Task ListClassroomsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
        {
            // Arrange
            var pageNumber = 3;
            var pageSize = 10;
            var expectedClassrooms = Enumerable.Range(21, 5)
                .Select(CreateClassroom)
                .ToList();
            var expectedTotalCount = 25;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
                .ReturnsAsync((expectedClassrooms, expectedTotalCount));

            // Act
            var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

            // Assert
            classrooms.Should().HaveCount(5);
            classrooms.First().Id.Should().Be(21);
            classrooms.Last().Id.Should().Be(25);
            totalCount.Should().Be(25);
        }

        /// <summary>
        /// Ensures that the service returns an empty list when requesting a page beyond the total count.
        /// </summary>
        [Fact]
        public async Task ListClassroomsPagedAsync_Should_Return_Empty_List_When_Page_Exceeds_Total()
        {
            // Arrange
            var pageNumber = 10;
            var pageSize = 10;
            var expectedClassrooms = new List<Classroom>();
            var expectedTotalCount = 15;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
                .ReturnsAsync((expectedClassrooms, expectedTotalCount));

            // Act
            var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

            // Assert
            classrooms.Should().BeEmpty();
            totalCount.Should().Be(15);

            _repositoryMock.Verify(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null), Times.Once);
        }

        /// <summary>
        /// Ensures that the service correctly handles different page sizes.
        /// </summary>
        [Theory]
        [InlineData(5, 5)]
        [InlineData(10, 10)]
        [InlineData(20, 20)]
        [InlineData(50, 25)] // Only 25 total items
        public async Task ListClassroomsPagedAsync_Should_Handle_Different_Page_Sizes(int pageSize, int expectedCount)
        {
            // Arrange
            var pageNumber = 1;
            var totalClassrooms = 25;
            var expectedClassrooms = Enumerable.Range(1, Math.Min(pageSize, totalClassrooms))
                .Select(CreateClassroom)
                .ToList();

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, pageSize, null))
                .ReturnsAsync((expectedClassrooms, totalClassrooms));

            // Act
            var (classrooms, totalCount) = await _service.ListClassroomsPagedAsync(pageNumber, pageSize);

            // Assert
            classrooms.Should().HaveCount(expectedCount);
            totalCount.Should().Be(totalClassrooms);
        }

        /// <summary>
        /// Ensures that the service propagates ValidationException for invalid page number.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task ListClassroomsPagedAsync_Should_Throw_Exception_For_Invalid_PageNumber(int invalidPageNumber)
        {
            // Arrange
            var pageSize = 10;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(invalidPageNumber, pageSize, null))
                .ThrowsAsync(new ValidationException("Page number must be greater than 0."));

            // Act
            Func<Task> act = () => _service.ListClassroomsPagedAsync(invalidPageNumber, pageSize);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Page number must be greater than 0*");
        }

        /// <summary>
        /// Ensures that the service propagates ValidationException for invalid page size.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task ListClassroomsPagedAsync_Should_Throw_Exception_For_Invalid_PageSize(int invalidPageSize)
        {
            // Arrange
            var pageNumber = 1;

            _repositoryMock
                .Setup(r => r.ListClassroomsPagedAsync(pageNumber, invalidPageSize, null))
                .ThrowsAsync(new ValidationException("Page size must be greater than 0."));

            // Act
            Func<Task> act = () => _service.ListClassroomsPagedAsync(pageNumber, invalidPageSize);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Page size must be greater than 0*");
        }
    }
}
