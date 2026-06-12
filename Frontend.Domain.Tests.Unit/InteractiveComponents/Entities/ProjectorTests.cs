using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.InteractiveComponents.Entities;


/// <summary>
/// Unit tests for the <see cref="Projector"/> entity.
/// These tests ensure that the Projector constructor behaves correctly
/// when given valid and invalid parameters.
/// </summary>
public class ProjectorTests
{
    /// <summary>
    /// Tests that creating a Projector with an invalid color value
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
        Action act = () => new Projector(
            new Color(invalidHex),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(1920, 1080),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );

        // Assert
        act.Should().Throw<InvalidColorException>();

    }

    /// <summary>
    /// Tests that creating a Projector with a color string in an invalid format
    /// throws an <see cref="InvalidColorException"/>.
    /// Example: missing '#' in a hex color code.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowInvalidColorException_WhenColorHasInvalidFormat()
    {
        // Act
        Action act = () => new Projector(
            new Color("123456"),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(1920, 1080),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );
        // Assert
        act.Should().Throw<InvalidColorException>();
    }

    /// <summary>
    /// Tests that creating a Projector with an invalid PlateId format
    /// throws an <see cref="InvalidPlateIdException"/>.
    /// </summary>
    /// <param name="invalidId">Invalid Plate ID value</param>
    [Theory]
    [InlineData("1234567")] // Too long
    [InlineData("12345")]   // Too short
    public void Constructor_ShouldThrowInvalidPlateIdException_WhenPlateIdIsInvalid(string invalidId)
    {
        // Act
        Action act = () => new Projector(
            new Color("#FFFFFF"),
            "Smooth",
            100,
            new PlateId(invalidId),
            new Resolution(1920, 1080),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );
        // Assert
        act.Should().Throw<InvalidPlateIdException>();
    }

    /// <summary> 
    /// Tests that creating a Projector with an invalid resolution (zero width or height, negative width 
    /// or height or values that are too big)
    /// </summary>
    /// <param name="width">Width of the resolution</param>
    /// <param name="height">Height of the resolution</param>
    [Theory]
    [InlineData(0, 1080)]        // Zero width
    [InlineData(1920, 0)]        // Zero height
    [InlineData(-1920, 1080)]    // Negative width
    [InlineData(1920, -1080)]    // Negative height
    [InlineData(20000, 1080)]    // Width too big
    [InlineData(1920, 20000)]    // Height too big
    public void Constructor_ShouldThrowInvalidResolutionException_WhenResolutionIsInvalid(int width, int height)
    {
        // Act
        Action act = () => new Projector(
            new Color("#FFFFFF"),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(width, height),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );
        // Assert
        act.Should().Throw<InvalidResolutionException>();
    }

    /// <summary>
    /// Tests that creating Projector with invalid coordinates (negative values)
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
        Action act = () => new Projector(
            new Color("#FFFFFF"),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(1920, 1080),
            new Coordinates(x, y, z),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1
        );
        // Assert
        act.Should().Throw<InvalidCoordinatesException>();
    }

    /// <summary>
    /// Tests that creating a Projector with invalid dimensions (zero or negative values)
    /// throws an <see cref="InvalidDimensionsException"/>.
    /// </summary>
    /// <param name="width">Width dimension</param>
    /// <param name="height">Height of the projector</param>
    /// <param name="depth">Depth of the projector</param>
    [Theory]
    [InlineData(0, 1, 1)]    // Zero width
    [InlineData(1, 0, 1)]    // Zero height
    [InlineData(1, 1, 0)]    // Zero depth
    public void Constructor_ShouldThrowInvalidDimensionsException_WhenDimensionsAreInvalid(double width, double height, double depth)
    {
        // Act
        Action act = () => new Projector(
            new Color("#FFFFFF"),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(1920, 1080),
            new Coordinates(0, 0, 0),
            new Dimensions(width, height, depth),
            new Rotations(0, 0, 0),
            1
        );
        // Assert
        act.Should().Throw<InvalidDimensionsException>();
    }

    /// <summary>
    /// Tests that creating a Projector with all valid parameters
    /// successfully constructs the Projector instance with the expected property values.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateProjector_WhenAllParametersAreValid()
    {
        // Arrange
        var color = new Color("#FFFFFF");
        var texture = "Smooth";
        var brightness = 100;
        var plateId = new PlateId("123456");
        var resolution = new Resolution(1920, 1080);
        var coordinates = new Coordinates(0, 0, 0);
        var dimensions = new Dimensions(1, 1, 1);
        var rotations = new Rotations(0, 0, 0);

        // Act
        var projector = new Projector(
            color,
            texture,
            brightness,
            plateId,
            resolution,
            coordinates,
            dimensions,
            rotations,
            1
        );

        // Assert
        projector.Should().NotBeNull();
        projector.Color.Should().Be(color);
        projector.Texture.Should().Be(texture);
        projector.Brightness.Should().Be(brightness);
        projector.PlateId.Should().Be(plateId);
        projector.ProjectionResolution.Should().Be(resolution);
        projector.Coordinates.Should().Be(coordinates);
        projector.Dimensions.Should().Be(dimensions);
        projector.Rotations.Should().Be(rotations);
        projector.LearningSpaceId.Should().Be(1);
    }
}

