using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.Entities;

/// <summary>
/// Unit tests for the LearningSpace entity.
/// </summary>
public class LearningSpaceTests
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
    /// Constructor for the LearningSpaceTests class.
    /// </summary>
    /// <exception cref="Exception">Invalid value objects.</exception>
    public LearningSpaceTests()
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

        var textureCreated = LearningSpaceTexture.TryCreate("Outdoor_Wall_T16_Ambient_occlusion.png", out LearningSpaceTexture? texture);
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
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Id property when given valid values.
    /// This test passes if the Id property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Id.Should().Be(
            _inputId,
            because: "ctor should correctly set the Id passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the BuildingId property when given valid values.
    /// This test passes if the BuildingId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsBuildingIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.BuildingId.Should().Be(
            _inputBuildingId,
            because: "ctor should correctly set the BuildingId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the FloorLevel property when given valid values.
    /// This test passes if the FloorLevel property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsFloorLevelProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.FloorLevel.Should().Be(
            _inputFloorLevel,
            because: "ctor should correctly set the FloorLevel passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the RoomId property when given valid values.
    /// This test passes if the RoomId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsRoomIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.RoomId.Should().Be(
            _inputRoomId,
            because: "ctor should correctly set the RoomId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the Color property when given valid values.
    /// This test passes if the Color property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsColorProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Color.Should().Be(
            _inputColor,
            because: "ctor should correctly set the Color passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the RoomId property when given valid values.
    /// This test passes if the RoomId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsDimensionsProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Dimensions.Should().Be(
            _inputDimensions,
            because: "ctor should correctly set the Dimensions passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the RoomId property when given valid values.
    /// This test passes if the RoomId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenValidValues_CorrectlySetsCoordinatesProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Coordinates.Should().Be(
            _inputCoordinates,
            because: "ctor should correctly set the Coordinates passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the BuildingId property when given null values.
    /// This test passes if the BuildingId property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenNullBuildingId_CorrectlySetsBuildingIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputNullBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.BuildingId.Should().Be(
            _inputNullBuildingId,
            because: "ctor should correctly set the null BuildingId passed as parameter");
    }

    /// <summary>
    /// Fact unit test for the LearningSpace entity constructor.
    /// </summary>
    /// <remarks>
    /// Tests whether the constructor correctly sets the FloorLevel property when given null values.
    /// This test passes if the FloorLevel property matches the input value.
    /// </remarks>
    [Fact]
    public void Constructor_GivenNullFloorLevel_CorrectlySetsFloorLevelProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputNullFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.FloorLevel.Should().Be(
            _inputNullFloorLevel,
            because: "ctor should correctly set the null FloorLevel passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.BuildingId.Should().Be(
            _inputBuildingId,
            because: "ctor should correctly set the BuildingId passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsFloorLevelProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.FloorLevel.Should().Be(
            _inputFloorLevel,
            because: "ctor should correctly set the FloorLevel passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsRoomIdProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.RoomId.Should().Be(
            _inputRoomId,
            because: "ctor should correctly set the RoomId passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsColorProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Color.Should().Be(
            _inputColor,
            because: "ctor should correctly set the Color passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsDimensionsProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Dimensions.Should().Be(
            _inputDimensions,
            because: "ctor should correctly set the Dimensions passed as parameter");
    }

    [Fact]
    public void Constructor_GivenNoId_CorrectlySetsCoordinatesProperty()
    {
        // Arrange

        // Act
        var learningSpace = new LearningSpace(
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Assert
        learningSpace.Coordinates.Should().Be(
            _inputCoordinates,
            because: "ctor should correctly set the Coordinates passed as parameter");
    }

    /// <summary>
    /// Tests that the Update method only updates the RoomId when only that parameter is provided.
    /// </summary>
    [Fact]
    public void Update_GivenOnlyRoomId_UpdatesOnlyRoomIdProperty()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newRoomId = "Lab-303";

        // Act
        learningSpace.Update(roomId: newRoomId, updateBuildingId: false, updateFloorLevel: false);

        // Assert
        learningSpace.RoomId.Should().Be(newRoomId,
            because: "Update should set the new RoomId");
        learningSpace.BuildingId.Should().Be(_inputBuildingId,
            because: "Update should preserve the original BuildingId");
        learningSpace.FloorLevel.Should().Be(_inputFloorLevel,
            because: "Update should preserve the original FloorLevel");
        learningSpace.Dimensions.Should().Be(_inputDimensions,
            because: "Update should preserve the original Dimensions");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Update should preserve the original Coordinates");
    }

    /// <summary>
    /// Tests that the Update method only updates BuildingId and FloorLevel when only those parameters are provided.
    /// </summary>
    [Fact]
    public void Update_GivenOnlyBuildingIdAndFloorLevel_UpdatesOnlyThoseProperties()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newBuildingId = 7;
        var newFloorLevel = 2;

        // Act
        learningSpace.Update(buildingId: newBuildingId, floorLevel: newFloorLevel);

        // Assert
        learningSpace.BuildingId.Should().Be(newBuildingId,
            because: "Update should set the new BuildingId");
        learningSpace.FloorLevel.Should().Be(newFloorLevel,
            because: "Update should set the new FloorLevel");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Update should preserve the original RoomId");
        learningSpace.Dimensions.Should().Be(_inputDimensions,
            because: "Update should preserve the original Dimensions");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Update should preserve the original Coordinates");
    }

    /// <summary>
    /// Tests that the Update method only updates Color when only that parameter is provided.
    /// </summary>
    [Fact]
    public void Update_GivenOnlyColor_UpdatesOnlyColorProperty()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newColor = LearningSpaceColor.Create("#000000");

        // Act
        learningSpace.Update(color: newColor, updateBuildingId: false, updateFloorLevel: false);

        // Assert
        learningSpace.Color.Should().Be(newColor,
            because: "Update should set the new Color");
        learningSpace.BuildingId.Should().Be(_inputBuildingId,
            because: "Update should preserve the original BuildingId");
        learningSpace.FloorLevel.Should().Be(_inputFloorLevel,
            because: "Update should preserve the original FloorLevel");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Update should preserve the original RoomId");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Update should preserve the original Coordinates");
    }

    /// <summary>
    /// Tests that the Update method only updates Dimensions when only that parameter is provided.
    /// </summary>
    [Fact]
    public void Update_GivenOnlyDimensions_UpdatesOnlyDimensionsProperty()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newDimensions = LearningSpaceDimensions.Create(20f, 25f, 5f);

        // Act
        learningSpace.Update(dimensions: newDimensions, updateBuildingId: false, updateFloorLevel: false);

        // Assert
        learningSpace.Dimensions.Should().Be(newDimensions,
            because: "Update should set the new Dimensions");
        learningSpace.BuildingId.Should().Be(_inputBuildingId,
            because: "Update should preserve the original BuildingId");
        learningSpace.FloorLevel.Should().Be(_inputFloorLevel,
            because: "Update should preserve the original FloorLevel");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Update should preserve the original RoomId");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Update should preserve the original Coordinates");
    }

    /// <summary>
    /// Tests that the Update method only updates Coordinates when only that parameter is provided.
    /// </summary>
    [Fact]
    public void Update_GivenOnlyCoordinates_UpdatesOnlyCoordinatesProperty()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newCoordinates = LearningSpaceCoordinates.Create(15f, 5f, 20f);

        // Act
        learningSpace.Update(coordinates: newCoordinates, updateBuildingId: false, updateFloorLevel: false);

        // Assert
        learningSpace.Coordinates.Should().Be(newCoordinates,
            because: "Update should set the new Coordinates");
        learningSpace.BuildingId.Should().Be(_inputBuildingId,
            because: "Update should preserve the original BuildingId");
        learningSpace.FloorLevel.Should().Be(_inputFloorLevel,
            because: "Update should preserve the original FloorLevel");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Update should preserve the original RoomId");
        learningSpace.Dimensions.Should().Be(_inputDimensions,
            because: "Update should preserve the original Dimensions");
    }

    /// <summary>
    /// Tests that the Update method preserves all properties when called with updateBuildingId and updateFloorLevel set to false.
    /// </summary>
    [Fact]
    public void Update_GivenNoParametersAndUpdateFlagsDisabled_PreservesAllProperties()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        learningSpace.Update(updateBuildingId: false, updateFloorLevel: false);

        // Assert
        learningSpace.BuildingId.Should().Be(_inputBuildingId,
            because: "Update with updateBuildingId false should preserve BuildingId");
        learningSpace.FloorLevel.Should().Be(_inputFloorLevel,
            because: "Update with updateFloorLevel false should preserve FloorLevel");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Update with no parameters should preserve RoomId");
        learningSpace.Dimensions.Should().Be(_inputDimensions,
            because: "Update with no parameters should preserve Dimensions");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Update with no parameters should preserve Coordinates");
    }

    /// <summary>
    /// Tests that the Update method preserves the Id property regardless of parameters.
    /// </summary>
    [Fact]
    public void Update_GivenAnyParameters_PreservesIdProperty()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        var newDimensions = LearningSpaceDimensions.Create(50f, 60f, 10f);

        // Act
        learningSpace.Update(
            buildingId: 999,
            floorLevel: 10,
            roomId: "NewRoom",
            dimensions: newDimensions,
            coordinates: LearningSpaceCoordinates.Create(100f, 100f, 100f));

        // Assert
        learningSpace.Id.Should().Be(_inputId,
            because: "Update should never modify the Id property");
    }

    /// <summary>
    /// Tests that the Update method preserves original values when null is passed for roomId, dimensions, and coordinates.
    /// </summary>
    [Fact]
    public void Update_GivenNullForOptionalParameters_PreservesOriginalValues()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        learningSpace.Update(
            buildingId: 5,
            floorLevel: 3,
            roomId: null,
            dimensions: null,
            coordinates: null);

        // Assert
        learningSpace.BuildingId.Should().Be(5,
            because: "BuildingId should be updated to the new value");
        learningSpace.FloorLevel.Should().Be(3,
            because: "FloorLevel should be updated to the new value");
        learningSpace.RoomId.Should().Be(_inputRoomId,
            because: "Null roomId parameter should preserve original value");
        learningSpace.Dimensions.Should().Be(_inputDimensions,
            because: "Null dimensions parameter should preserve original value");
        learningSpace.Coordinates.Should().Be(_inputCoordinates,
            because: "Null coordinates parameter should preserve original value");
    }

    /// <summary>
    /// Tests that the Update method sets buildingId and floorLevel to null when explicitly passed null values.
    /// </summary>
    [Fact]
    public void Update_GivenNullBuildingIdAndFloorLevel_SetsToNull()
    {
        // Arrange
        var learningSpace = new LearningSpace(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        learningSpace.Update(buildingId: null, floorLevel: null);

        // Assert
        learningSpace.BuildingId.Should().BeNull(
            because: "Update with null buildingId should set BuildingId to null");
        learningSpace.FloorLevel.Should().BeNull(
            because: "Update with null buildingId should also clear FloorLevel due to business rule");
    }

}
