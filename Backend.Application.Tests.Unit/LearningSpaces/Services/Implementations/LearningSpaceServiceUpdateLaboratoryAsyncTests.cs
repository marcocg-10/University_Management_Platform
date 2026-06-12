using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.LearningSpaces.Services.Implementations;

/// <summary>
/// Contains unit tests for the UpdateLaboratoryAsync method.
/// </summary>
public class LearningSpaceServiceUpdateLaboratoryAsyncTests
    : IClassFixture<LearningSpaceServiceTestData>
{
    /// <summary>
    /// Test data used for the unit tests.
    /// </summary>
    private readonly LearningSpaceServiceTestData _testData;

    /// <summary>
    /// Constructs a LearningSpaceServiceUpdateLaboratoryAsyncTests instance.
    /// </summary>
    /// <param name="testData">Test data that will be used for the unit tests.</param>
    public LearningSpaceServiceUpdateLaboratoryAsyncTests(
        LearningSpaceServiceTestData testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync successfully updates a laboratory when given valid parameters.
    /// </summary>
    [Fact]
    public async Task UpdateLaboratoryAsync_WhenGivenValidParameters_ShouldUpdateLaboratory()
    {
        // Arrange
        var existingLaboratory = _testData.LaboratorySingleEntryData[0];

        int laboratoryId = existingLaboratory.Id;
        int? buildingId = 2;
        int? floorLevel = 3;
        string roomId = "Lab102-Updated";
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
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(existingLaboratory);

        repositoryMock
            .Setup(r => r.UpdateLaboratoryAsync(It.IsAny<Laboratory>()))
            .Returns(Task.CompletedTask);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var updatedLaboratory = await sut.UpdateLaboratoryAsync(
            laboratoryId,
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
        // that a laboratory is updated with the correct properties and not that each property
        // is set right, as that is already checked in the domain-layer tests.
        updatedLaboratory.Should().NotBeNull(because: "a valid updated laboratory should be returned");
        updatedLaboratory.Id.Should().Be(laboratoryId, because: "the laboratory ID should remain the same");
        updatedLaboratory.BuildingId.Should().Be(buildingId, because: "the BuildingId should be updated");
        updatedLaboratory.FloorLevel.Should().Be(floorLevel, because: "the FloorLevel should be updated");
        updatedLaboratory.RoomId.Should().Be(roomId, because: "the RoomId should be updated");
        updatedLaboratory.Color.Value.Should().Be(color, because: "the Color should be updated");
        updatedLaboratory.Texture.Value.Should().Be(texture, because: "the Texture should be updated");
        updatedLaboratory.Dimensions.Width.Should().Be(width, because: "the Width should be updated");
        updatedLaboratory.Dimensions.Length.Should().Be(length, because: "the Length should be updated");
        updatedLaboratory.Dimensions.Height.Should().Be(height, because: "the Height should be updated");
        updatedLaboratory.Coordinates.XCoordinate.Should().Be(xCoordinate, because: "the X Coordinate should be updated");
        updatedLaboratory.Coordinates.YCoordinate.Should().Be(yCoordinate, because: "the Y Coordinate should be updated");
        updatedLaboratory.Coordinates.ZCoordinate.Should().Be(zCoordinate, because: "the Z Coordinate should be updated");

        repositoryMock.Verify(
            r => r.GetLaboratoryByIdAsync(laboratoryId),
            Times.Once,
            "repository should be called to get the existing laboratory");

        repositoryMock.Verify(
            r => r.UpdateLaboratoryAsync(It.IsAny<Laboratory>()),
            Times.Once,
            "repository should be called to update the laboratory");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync updates laboratory with null building ID and floor level.
    /// </summary>
    [Fact]
    public async Task UpdateLaboratoryAsync_WhenBuildingIdAndFloorLevelAreNull_UpdatesSuccessfully()
    {
        // Arrange
        var existingLaboratory = _testData.LaboratorySingleEntryData[0];

        int laboratoryId = existingLaboratory.Id;
        int? buildingId = null;
        int? floorLevel = null;
        string roomId = "Standalone-Lab";
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
            .Setup(r => r.GetLaboratoryByIdAsync(laboratoryId))
            .ReturnsAsync(existingLaboratory);

        repositoryMock
            .Setup(r => r.UpdateLaboratoryAsync(It.IsAny<Laboratory>()))
            .Returns(Task.CompletedTask);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act
        var updatedLaboratory = await sut.UpdateLaboratoryAsync(
            laboratoryId,
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
        updatedLaboratory.Should().NotBeNull(because: "a valid updated laboratory should be returned");
        updatedLaboratory.BuildingId.Should().BeNull(because: "the BuildingId should be null");
        updatedLaboratory.FloorLevel.Should().BeNull(because: "the FloorLevel should be null");
        updatedLaboratory.RoomId.Should().Be(roomId, because: "the RoomId should be updated");

        repositoryMock.Verify(
            r => r.UpdateLaboratoryAsync(It.IsAny<Laboratory>()),
            Times.Once,
            "repository should be called to update the laboratory");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws ValidationException when laboratory ID is zero or negative.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task UpdateLaboratoryAsync_WhenLaboratoryIdIsInvalid_ThrowsValidationException(int invalidLaboratoryId)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
                invalidLaboratoryId,
                1,
                2,
                "Lab101",
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
                because: "the laboratory ID must be a positive number")
            .WithMessage("Laboratory ID must be a positive number.",
                because: "the exception message should clearly state the validation error");

        repositoryMock.Verify(
            r => r.GetLaboratoryByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when laboratory ID is invalid");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws LearningSpaceNotFoundException when laboratory does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateLaboratoryAsync_WhenLaboratoryDoesNotExist_ThrowsLearningSpaceNotFoundException()
    {
        // Arrange
        int nonExistentLaboratoryId = 999;
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        repositoryMock
            .Setup(r => r.GetLaboratoryByIdAsync(nonExistentLaboratoryId))
            .ReturnsAsync((Laboratory?)null);

        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
                nonExistentLaboratoryId,
                1,
                2,
                "Lab999",
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
                because: "the laboratory does not exist in the database")
            .WithMessage($"*{nonExistentLaboratoryId}*",
                because: "the exception message should include the laboratory ID");

        repositoryMock.Verify(
            r => r.GetLaboratoryByIdAsync(nonExistentLaboratoryId),
            Times.Once,
            "repository should be called to check if laboratory exists");

        repositoryMock.Verify(
            r => r.UpdateLaboratoryAsync(It.IsAny<Laboratory>()),
            Times.Never,
            "repository should not be called to update when laboratory doesn't exist");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws ValidationException when room ID is null or empty.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UpdateLaboratoryAsync_WhenRoomIdIsInvalid_ThrowsValidationException(string invalidRoomId)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
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
            r => r.GetLaboratoryByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when room ID is invalid");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws ValidationException when dimensions are invalid.
    /// </summary>
    [Theory]
    [InlineData(-1.0f, 6.0f, 3.0f)] // Invalid width
    [InlineData(5.0f, -1.0f, 3.0f)] // Invalid length
    [InlineData(5.0f, 6.0f, -1.0f)] // Invalid height
    [InlineData(0.0f, 6.0f, 3.0f)]  // Zero width
    [InlineData(5.0f, 0.0f, 3.0f)]  // Zero length
    [InlineData(5.0f, 6.0f, 0.0f)]  // Zero height
    public async Task UpdateLaboratoryAsync_WhenDimensionsAreInvalid_ThrowsValidationException(
        float width, float length, float height)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
                1,
                1,
                2,
                "Lab101",
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
            r => r.GetLaboratoryByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when dimensions are invalid");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws ValidationException when coordinates are invalid.
    /// </summary>
    [Theory]
    [InlineData(float.NaN, 2.0f, 0.0f)]           // NaN X coordinate
    [InlineData(1.0f, float.NaN, 0.0f)]           // NaN Y coordinate
    [InlineData(1.0f, 2.0f, float.NaN)]           // NaN Z coordinate
    [InlineData(float.PositiveInfinity, 2.0f, 0.0f)]  // Infinity X coordinate
    [InlineData(1.0f, float.NegativeInfinity, 0.0f)]  // Negative Infinity Y coordinate
    public async Task UpdateLaboratoryAsync_WhenCoordinatesAreInvalid_ThrowsValidationException(
        float xCoordinate, float yCoordinate, float zCoordinate)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
                1,
                1,
                2,
                "Lab101",
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
            r => r.GetLaboratoryByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when coordinates are invalid");
    }

    /// <summary>
    /// Tests that UpdateLaboratoryAsync throws ValidationException when color is invalid.
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
    public async Task UpdateLaboratoryAsync_WhenColorIsInvalid_ThrowsValidationException(string invalidColor)
    {
        // Arrange
        var repositoryMock = new Mock<ILearningSpaceRepository>();
        var sut = new LearningSpaceService(repositoryMock.Object);

        // Act & Assert
        await FluentActions
            .Awaiting(() => sut.UpdateLaboratoryAsync(
                1,
                1,
                2,
                "Lab101",
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
            r => r.GetLaboratoryByIdAsync(It.IsAny<int>()),
            Times.Never,
            "repository should not be called when color is invalid");
    }
}
