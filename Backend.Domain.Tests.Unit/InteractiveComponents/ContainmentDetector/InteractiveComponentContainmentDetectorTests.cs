using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.InteractiveComponents.ContainmentDetector;

public class InteractiveComponentContainmentDetectorTests
{
    private readonly IInteractiveComponentContainmentDetector _detector;

    private readonly int _inputId;
    private readonly int _inputBuildingId;
    private readonly int _inputFloorLevel;
    private readonly string _inputRoomId;
    private readonly LearningSpaceColor _inputColor;
    private readonly LearningSpaceTexture _inputTexture;
    private readonly LearningSpaceDimensions _inputDimensions;
    private readonly LearningSpaceCoordinates _inputCoordinates;

    public InteractiveComponentContainmentDetectorTests()
    {
        _detector = new InteractiveComponentContainmentDetector();
        // Common valid input values for tests
        _inputId = 1;
        _inputBuildingId = 1;
        _inputFloorLevel = 1;
        _inputRoomId = "0104-IF";

        var colorCreated = LearningSpaceColor.TryCreate("#FFFFFF", out LearningSpaceColor? color);
        if (!colorCreated || color is null)
        {
            throw new Exception("Failed to construct input dimensions.");
        }
        _inputColor = color;

        // Valid coordinates
        var coordinatesCreated = LearningSpaceCoordinates.TryCreate(0f, 0f, 0f, out LearningSpaceCoordinates? coordinates);
        if (!coordinatesCreated || coordinates is null)
        {
            throw new Exception("Failed to construct input coordinates.");
        }
        _inputCoordinates = coordinates;

        // Valid texture
        var textureCreated = LearningSpaceTexture.TryCreate("Outdoor_Wall_T14_Base_Color.png", out LearningSpaceTexture? texture);
        if (!textureCreated || texture is null)
        {
            throw new Exception("Failed to construct input texture.");
        }
        _inputTexture = texture;

        // Valid dimensions
        var dimensionsCreated = LearningSpaceDimensions.TryCreate(50, 50, 50, out LearningSpaceDimensions? dimensions);
        if (!dimensionsCreated || dimensions is null)
        {
            throw new Exception("Failed to construct input dimensions.");
        }
        _inputDimensions = dimensions;
    }

    /// <summary>
    /// Verifies that an interactive component is correctly identified as being fully contained within a learning space.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="_detector.IsContained"/> method returns <see
    /// langword="true"/> when the component is entirely within the boundaries of the specified learning
    /// space.</remarks>
    [Fact]
    public void InteractiveComponent_ContainedInside_LearningSpace_ShouldReturnTrue()
    {
        // Arrange
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(10, 2, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var laboratory = new Laboratory(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        bool isContained = _detector.IsContained(board, laboratory);

        // Assert
        isContained.Should().BeTrue(because: "Component is fully inside a Learning Space");
    }

    /// <summary>
    /// Verifies that an interactive component is not contained within a specified learning space.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="_detector.IsContained"/> method correctly identifies
    /// when a component, such as a board, is not located within the boundaries of a learning space, such as a
    /// laboratory.</remarks>
    [Fact]
    public void InteractiveComponent_NotContainedInside_LearningSpace_ShouldReturnFalse()
    {
        // Arrange
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(30, 30, 30),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var laboratory = new Laboratory(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        bool isContained = _detector.IsContained(board, laboratory);

        // Assert
        isContained.Should().BeFalse(because: "Component not contained inside a Learning Space");
    }

    /// <summary>
    /// Verifies that the <see cref="Board"/> is not considered fully contained within the <see cref="Laboratory"/> when
    /// it is only partially contained.
    /// </summary>
    /// <remarks>This test ensures that the containment detection logic correctly identifies when a component
    /// is not fully enclosed within a learning space, returning <see langword="false"/> in such cases.</remarks>
    [Fact]
    public void InteractiveComponent_PartiallyContainedInside_LearningSpace_ShouldReturnFalse()
    {
        // Arrange
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(100, 1, 1),
            new Rotations(0, 0, 0),
            1);
        var laboratory = new Laboratory(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        bool isContained = _detector.IsContained(board, laboratory);

        // Assert
        isContained.Should().BeFalse(because: "Component not fully contained inside a Learning Space");
    }

    /// <summary>
    /// Verifies that a rotated interactive component, which is only partially contained within a learning space, is
    /// correctly identified as not fully contained.
    /// </summary>
    /// <remarks>This test ensures that the containment detection logic correctly handles cases where a
    /// rotated component overlaps with, but is not entirely within, the boundaries of a learning space. The test uses a
    /// specific board configuration and laboratory setup to validate this behavior.</remarks>
    [Fact]
    public void RotatedInteractiveComponent_PartiallyContainedInside_LearningSpace_ShouldReturnFalse()
    {
        // Arrange
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("111111"),
            new Coordinates(0, 0, 0),
            new Dimensions(45, 30, 30),
            new Rotations(0, 60, 0),
            1);
        var laboratory = new Laboratory(
            _inputId,
            _inputBuildingId,
            _inputFloorLevel,
            _inputRoomId,
            _inputColor,
            _inputTexture,
            _inputDimensions,
            _inputCoordinates);

        // Act
        bool isContained = _detector.IsContained(board, laboratory);

        // Assert
        isContained.Should().BeFalse(because: "Rotated component not fully contained inside a Learning Space");
    }

}
