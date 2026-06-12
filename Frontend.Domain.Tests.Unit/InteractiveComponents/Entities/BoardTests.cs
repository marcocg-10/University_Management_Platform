using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.InteractiveComponents.Entities;

/// <summary>
/// Unit tests for the <see cref="Board"/> entity.
/// These tests ensure that the Board constructor behaves correctly
/// when given valid and invalid parameters.
/// </summary>
public class BoardTests
{
    /// <summary>
    /// Tests that creating a Board with an invalid color value
    /// (null, empty string, or whitespace) throws an <see cref="InvalidColorException"/>.
    /// </summary>
    /// <param name="invalidHex">Invalid color value</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowInvalidColorException_WhenColorIsInvalid(string invalidHex)
    {
        // Act
        Action act = () => new Board(
            new Color(invalidHex),
            new Color(invalidHex),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidColorException>();
    }

    /// <summary>
    /// Tests that creating a Board with a color string in an invalid format
    /// throws an <see cref="InvalidColorException"/>.
    /// Example: missing '#' in a hex color code.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowInvalidColorException_WhenColorHasInvalidFormat()
    {
        // Act
        Action act = () => new Board(
            new Color("123456"),
            new Color("123456"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidColorException>();
    }

    /// <summary>
    /// Tests that creating a Board with an invalid PlateId format
    /// throws an <see cref="InvalidPlateIdException"/>.
    /// </summary>
    /// <param name="invalidId">Invalid Plate ID value</param>
    [Theory]
    [InlineData("1234567")] // Too long
    [InlineData("13546")]   // Too short
    public void Constructor_ShouldThrowInvalidPlateIdException_WhenPlateIdIsInvalid(string invalidId)
    {
        // Act
        Action act = () => new Board(
            new Color("#FFFFFF"),
            new Color("#000000"),
            "Smooth",
            new PlateId(invalidId),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidPlateIdException>();
    }

    /// <summary>
    /// Tests that creating a Board with invalid coordinates (negative values)
    /// throws an <see cref="InvalidCoordinatesException"/>.
    /// </summary>
    /// <param name="x">X coordinate value</param>
    /// <param name="y">Y coordinate value</param>
    /// <param name="z">Z coordinate value</param>
    [Theory]
    [InlineData(-10001, 0, 0)]
    [InlineData(0, 10001, 0)]
    [InlineData(0, 0, -12000)]
    public void Constructor_ShouldThrowInvalidCoordinatesException_WhenCoordinatesAreInvalid(int x, int y, int z)
    {
        // Act
        Action act = () => new Board(
            new Color("#FFFFFF"),
            new Color("#000000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(x, y, z),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidCoordinatesException>();
    }

    /// <summary>
    /// Tests that creating a Board with invalid dimensions
    /// (zero or negative width, height, or depth) throws an <see cref="InvalidDimensionsException"/>.
    /// </summary>
    /// <param name="width">Width of the board</param>
    /// <param name="height">Height of the board</param>
    /// <param name="depth">Depth of the board</param>
    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 0)]
    public void Constructor_ShouldThrowInvalidDimensionsException_WhenDimensionsAreInvalid(double width, double height, double depth)
    {
        // Act
        Action act = () => new Board(
            new Color("#FFFFFF"),
            new Color("#000000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(0, 0, 0),
            new Dimensions(width, height, depth),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidDimensionsException>();
    }

    /// <summary>
    /// Tests that creating a Board with all valid parameters
    /// successfully constructs a Board instance with the expected properties.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateBoard_WhenAllAttributesAreValid()
    {
        // Arrange
        var color = new Color("#FFFFFF");
        var markerColor = new Color("#000000");
        var texture = "Smooth";
        var plateId = new PlateId("123456");
        var coordinates = new Coordinates(0, 0, 0);
        var dimensions = new Dimensions(1, 1, 1);
        var rotations = new Rotations(0, 0, 0);

        // Act
        var board = new Board(
            color,
            markerColor,
            texture,
            plateId,
            coordinates,
            dimensions,
            rotations,
            1
        );

        // Assert
        board.Should().NotBeNull();
        board.Color.Should().Be(color);
        board.MarkerColor.Should().Be(markerColor);
        board.Texture.Should().Be(texture);
        board.PlateId.Should().Be(plateId);
        board.Coordinates.Should().Be(coordinates);
        board.Dimensions.Should().Be(dimensions);
        board.Rotations.Should().Be(rotations);
    }
}
