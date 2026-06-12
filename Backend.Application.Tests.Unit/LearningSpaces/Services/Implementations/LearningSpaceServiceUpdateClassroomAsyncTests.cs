using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

/// <summary>
/// Contains unit tests for the UpdateClassroomAsync method.
/// </summary>
public class LearningSpaceServiceUpdateClassroomAsyncTests
    : IClassFixture<LearningSpaceServiceTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceServiceTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceServiceUpdateClassroomAsyncTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceServiceUpdateClassroomAsyncTests(
        LearningSpaceServiceTestData testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync successfully updates a classroom when given valid parameters.
    /// </summary>
    [Fact]
    public async Task UpdateClassroomAsync_WhenGivenValidParameters_ShouldUpdateClassroom()
    {
        // Arrange
        var existingClassroom = _testData.ClassroomSingleEntryData[0];

        int classroomId = existingClassroom.Id;
        int? buildingId = 2;
        int? floorLevel = 3;
        string roomId = "Class102-Updated";
        string color = "#00FF00";
        string texture = "Brick_Wall_T03_Ambient_occlusion.png";
        float width = 8.0f;
        float length = 10.0f;
        float height = 4.0f;
        float xCoordinate = 5.0f;
        float yCoordinate = 7.0f;
        float zCoordinate = 1.0f;

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(existingClassroom);

        repositoryMock
            .Setup(r => r.UpdateClassroomAsync(It.IsAny<Classroom>()))
            .Returns(Task.CompletedTask);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var updatedClassroom = await sut.UpdateClassroomAsync(
            classroomId,
            buildingId,
            floorLevel,
            roomId,
            color,
            texture,
            width,
            length,
            height,
            xCoordinate,
            yCoordinate,
            zCoordinate);

        // Assert
        // These asserts purpose is to verify
        // that a classroom is updated with the correct properties and not that each property
        // is set right, as that is already checked in the domain-layer tests.
        updatedClassroom.Should().NotBeNull(because: "a valid updated classroom should be returned");
        updatedClassroom.Id.Should().Be(classroomId, because: "the classroom ID should remain the same");
        updatedClassroom.BuildingId.Should().Be(buildingId, because: "the BuildingId should be updated");
        updatedClassroom.FloorLevel.Should().Be(floorLevel, because: "the FloorLevel should be updated");
        updatedClassroom.RoomId.Should().Be(roomId, because: "the RoomId should be updated");
        updatedClassroom.Color.Value.Should().Be(color, because: "the Color should be updated");
        updatedClassroom.Texture.Value.Should().Be(texture, because: "the Texture should be updated");
        updatedClassroom.Dimensions.Width.Should().Be(width, because: "the Width should be updated");
        updatedClassroom.Dimensions.Length.Should().Be(length, because: "the Length should be updated");
        updatedClassroom.Dimensions.Height.Should().Be(height, because: "the Height should be updated");
        updatedClassroom.Coordinates.XCoordinate.Should().Be(xCoordinate, because: "the X Coordinate should be updated");
        updatedClassroom.Coordinates.YCoordinate.Should().Be(yCoordinate, because: "the Y Coordinate should be updated");
        updatedClassroom.Coordinates.ZCoordinate.Should().Be(zCoordinate, because: "the Z Coordinate should be updated");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(classroomId),
            Times.Once,
            "repository should be called to get the existing classroom");

        repositoryMock.Verify(
            r => r.UpdateClassroomAsync(It.IsAny<Classroom>()),
            Times.Once,
            "repository should be called to update the classroom");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync updates classroom with null building ID and floor level.
    /// </summary>
    [Fact]
    public async Task UpdateClassroomAsync_WhenBuildingIdAndFloorLevelAreNull_UpdatesSuccessfully()
    {
        // Arrange
        var existingClassroom = _testData.ClassroomSingleEntryData[0];

        int classroomId = existingClassroom.Id;
        int? buildingId = null;
        int? floorLevel = null;
        string roomId = "Standalone-Classroom";
        string color = "#00FF00";
        string texture = "Outdoor_Wall_T15_Ambient_occlusion.png";
        float width = 5.0f;
        float length = 6.0f;
        float height = 3.0f;
        float xCoordinate = 1.0f;
        float yCoordinate = 2.0f;
        float zCoordinate = 0.0f;

        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(classroomId))
            .ReturnsAsync(existingClassroom);

        repositoryMock
            .Setup(r => r.UpdateClassroomAsync(It.IsAny<Classroom>()))
            .Returns(Task.CompletedTask);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var updatedClassroom = await sut.UpdateClassroomAsync(
            classroomId,
            buildingId,
            floorLevel,
            roomId,
            color,
            texture,
            width,
            length,
            height,
            xCoordinate,
            yCoordinate,
            zCoordinate);

        // Assert
        updatedClassroom.Should().NotBeNull(because: "a valid updated classroom should be returned");
        updatedClassroom.BuildingId.Should().BeNull(because: "the BuildingId should be null");
        updatedClassroom.FloorLevel.Should().BeNull(because: "the FloorLevel should be null");
        updatedClassroom.RoomId.Should().Be(roomId, because: "the RoomId should be updated");

        repositoryMock.Verify(
            r => r.UpdateClassroomAsync(It.IsAny<Classroom>()),
            Times.Once,
            "repository should be called to update the classroom");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws ValidationException when classroom ID is zero or negative.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task UpdateClassroomAsync_WhenClassroomIdIsInvalid_ThrowsValidationException(int invalidClassroomId)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                invalidClassroomId,
                1,
                2,
                "Class101",
                "#FFFFFF",
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                5.0f,
                6.0f,
                3.0f,
                1.0f,
                2.0f,
                0.0f))
            .Should()
            .ThrowExactlyAsync<ValidationException>(
                because: "the classroom ID must be a positive number")
            .WithMessage("Classroom ID must be a positive number.",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when classroom ID is invalid");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws LearningSpaceNotFoundException when classroom does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateClassroomAsync_WhenClassroomDoesNotExist_ThrowsLearningSpaceNotFoundException()
    {
        // Arrange
        int nonExistentClassroomId = 999;
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetClassroomByIdAsync(nonExistentClassroomId))
            .ReturnsAsync((Classroom?)null);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                nonExistentClassroomId,
                1,
                2,
                "Class999",
                "#FFFFFF",
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                5.0f,
                6.0f,
                3.0f,
                1.0f,
                2.0f,
                0.0f))
            .Should()
            .ThrowExactlyAsync<LearningSpaceNotFoundException>(
                because: "the classroom does not exist in the database")
            .WithMessage($"*{nonExistentClassroomId}*",
                because: "the exception message should include the classroom ID");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(nonExistentClassroomId),
            Times.Once,
            "repository should be called to check if classroom exists");

        repositoryMock.Verify(
            r => r.UpdateClassroomAsync(It.IsAny<Classroom>()),
            Times.Never,
            "repository should not be called to update when classroom doesn't exist");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws ValidationException when room ID is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateClassroomAsync_WhenRoomIdIsInvalid_ThrowsValidationException(string invalidRoomId)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                1,
                1,
                2,
                invalidRoomId,
                "#FFFFFF",
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                5.0f,
                6.0f,
                3.0f,
                1.0f,
                2.0f,
                0.0f))
            .Should()
            .ThrowExactlyAsync<ValidationException>(
                because: "room ID is required and cannot be empty")
            .WithMessage("Room ID is required and cannot be empty.",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when room ID is invalid");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws ValidationException when dimensions are invalid.
    /// </summary>
    [Theory]
    [InlineData(-1.0f, 6.0f, 3.0f)] // Invalid width
    [InlineData(5.0f, -1.0f, 3.0f)] // Invalid length
    [InlineData(5.0f, 6.0f, -1.0f)] // Invalid height
    [InlineData(0.0f, 6.0f, 3.0f)]  // Zero width
    [InlineData(5.0f, 0.0f, 3.0f)]  // Zero length
    [InlineData(5.0f, 6.0f, 0.0f)]  // Zero height
    public async Task UpdateClassroomAsync_WhenDimensionsAreInvalid_ThrowsValidationException(
        float width, float length, float height)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                1,
                1,
                2,
                "Class101",
                "#FFFFFF",
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                width,
                length,
                height,
                1.0f,
                2.0f,
                0.0f))
            .Should()
            .ThrowExactlyAsync<ValidationException>(
                because: "dimensions must be positive numbers")
            .WithMessage("Invalid dimensions provided. Width, length, and height must be positive numbers.",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when dimensions are invalid");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws ValidationException when coordinates are invalid.
    /// </summary>
    [Theory]
    [InlineData(float.NaN, 2.0f, 0.0f)]           // NaN X coordinate
    [InlineData(1.0f, float.NaN, 0.0f)]           // NaN Y coordinate
    [InlineData(1.0f, 2.0f, float.NaN)]           // NaN Z coordinate
    [InlineData(float.PositiveInfinity, 2.0f, 0.0f)]  // Infinity X coordinate
    [InlineData(1.0f, float.NegativeInfinity, 0.0f)]  // Negative Infinity Y coordinate
    public async Task UpdateClassroomAsync_WhenCoordinatesAreInvalid_ThrowsValidationException(
        float xCoordinate, float yCoordinate, float zCoordinate)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                1,
                1,
                2,
                "Class101",
                "#FFFFFF",
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                5.0f,
                6.0f,
                3.0f,
                xCoordinate,
                yCoordinate,
                zCoordinate))
            .Should()
            .ThrowExactlyAsync<ValidationException>(
                because: "coordinates must be valid numbers")
            .WithMessage("Invalid coordinates provided. X, Y, and Z coordinates must be valid numbers.",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when coordinates are invalid");
    }

    /// <summary>
    /// Tests that UpdateClassroomAsync throws ValidationException when color is invalid.
    /// </summary>
    [Theory]
    [InlineData("   ")]                   // Whitespace color
    [InlineData("#GGGGGG")]               // Invalid hex characters
    [InlineData("#FF")]                   // Too short
    [InlineData("#FFFFFFF")]              // Too long (8 characters)
    [InlineData("FF5733")]                // No # prefix
    [InlineData("#ZZZ")]                  // Invalid characters in 3-digit format
    [InlineData("red")]                   // Named color (not hex)
    [InlineData("#")]                     // Only # symbol
    public async Task UpdateClassroomAsync_WhenColorIsInvalid_ThrowsValidationException(string invalidColor)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateClassroomAsync(
                1,
                1,
                2,
                "Class101",
                invalidColor,
                "Outdoor_Wall_T15_Ambient_occlusion.png",
                5.0f,
                6.0f,
                3.0f,
                1.0f,
                2.0f,
                0.0f))
            .Should()
            .ThrowExactlyAsync<ValidationException>(
                because: "color must be in valid hexadecimal format")
            .WithMessage("Invalid color format. Color must be in hexadecimal format (e.g., #FFFFFF or #FFF).",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetClassroomByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when color is invalid");
    }
}