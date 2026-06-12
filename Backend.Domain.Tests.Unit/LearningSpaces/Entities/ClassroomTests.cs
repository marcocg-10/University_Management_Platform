using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.Entities;

/// <summary>
/// Unit tests for the Classroom entity.
/// </summary>
public class ClassroomTests
{
    private readonly int _inputId;
    private readonly int _inputBuildingId;
    private readonly int _inputFloorLevel;
    private readonly string _inputRoomId;
    private readonly LearningSpaceColor _inputColor;
    private readonly LearningSpaceTexture _inputTexture;
    private readonly LearningSpaceDimensions _inputDimensions;
    private readonly LearningSpaceCoordinates _inputCoordinates;

    private readonly int? _inputNullBuildingId;
    private readonly int? _inputNullFloorLevel;

    /// <summary>
    /// Constructor for the ClassroomTests class.
    /// </summary>
    /// <exception cref="Exception">Invalid value objects.</exception>
    public ClassroomTests()
    {
        // Common valid input values for tests
        _inputId = 1;
        _inputBuildingId = 1;
        _inputFloorLevel = 1;
        _inputRoomId = "0104-IF";

        // Adding null values
        _inputNullBuildingId = null;
        _inputNullFloorLevel = null;

        var colorCreated = LearningSpaceColor.TryCreate("#FFFFFF", out LearningSpaceColor? color);
        if (!colorCreated || color is null)
        {
            throw new Exception("Failed to construct input dimensions.");
        }
        _inputColor = color;

        var textureCreated = LearningSpaceTexture.TryCreate("Outdoor_Wall_T15_Ambient_occlusion.png", out LearningSpaceTexture? texture);
        if (!textureCreated || texture is null)
        {
            throw new Exception("Failed to construct input texture.");
        }
        _inputTexture = texture;

        // Valid dimensions
        var coordinatesCreated = LearningSpaceCoordinates.TryCreate(1f, 2f, 10f, out LearningSpaceCoordinates? coordinates);
        if (!coordinatesCreated || coordinates is null)
        {
            throw new Exception("Failed to construct input coordinates.");
        }

        // Valid coordinates
        _inputCoordinates = coordinates;
        var dimensionsCreated = LearningSpaceDimensions.TryCreate(3f, 5f, 8f, out LearningSpaceDimensions? dimensions);
        if (!dimensionsCreated || dimensions is null)
        {
            throw new Exception("Failed to construct input dimensions.");
        }
        _inputDimensions = dimensions;
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Id property when given valid values.
    /// This test passes if the Id property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Id.Should().Be(
            _inputId,
            because: "ctor should correctly set the Id passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the BuildingId property when given valid values.
    /// This test passes if the BuildingId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomBuildingIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.BuildingId.Should().Be(
            _inputBuildingId,
            because: "ctor should correctly set the BuildingId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the FloorLevel property when given valid values.
    /// This test passes if the FloorLevel property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomFloorLevelProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.FloorLevel.Should().Be(
            _inputFloorLevel,
            because: "ctor should correctly set the FloorLevel passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the RoomId property when given valid values.
    /// This test passes if the RoomId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomRoomIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.RoomId.Should().Be(
            _inputRoomId,
            because: "ctor should correctly set the RoomId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Color property when given valid values.
    /// This test passes if the Color property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomColorProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Color.Should().Be(
            _inputColor,
            because: "ctor should correctly set the Color passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Dimensions property when given valid values.
    /// This test passes if the Dimensions property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomDimensionsProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Dimensions.Should().Be(
            _inputDimensions,
            because: "ctor should correctly set the Dimensions passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Coordinates property when given valid values.
    /// This test passes if the Coordinates property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsClassroomCoordinatesProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Coordinates.Should().Be(
            _inputCoordinates,
            because: "ctor should correctly set the Coordinates passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the BuildingId property when given null values.
    /// This test passes if the BuildingId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenNullBuildingId_CorrectlySetsClassroomBuildingIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputNullBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.BuildingId.Should().Be(
            _inputNullBuildingId,
            because: "ctor should correctly set the null BuildingId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the Classroom entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the FloorLevel property when given null values.
    /// This test passes if the FloorLevel property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenNullFloorLevel_CorrectlySetsClassroomFloorLevelProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputId,
            _inputBuildingId,
            _inputNullFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.FloorLevel.Should().Be(
            _inputNullFloorLevel,
            because: "ctor should correctly set the null FloorLevel passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsClassroomIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.BuildingId.Should().Be(
            _inputBuildingId,
            because: "ctor should correctly set the BuildingId passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsClassroomFloorLevelProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.FloorLevel.Should().Be(
            _inputFloorLevel,
            because: "ctor should correctly set the FloorLevel passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsClassroomRoomIdProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.RoomId.Should().Be(
            _inputRoomId,
            because: "ctor should correctly set the RoomId passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsClassroomDimensionsProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Dimensions.Should().Be(
            _inputDimensions,
            because: "ctor should correctly set the Dimensions passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsClassroomCoordinatesProperty()
    {
        // Arrange

        // Act
        var classroom = new Classroom(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        classroom.Coordinates.Should().Be(
            _inputCoordinates,
            because: "ctor should correctly set the Coordinates passed as parameter");
    }
}
