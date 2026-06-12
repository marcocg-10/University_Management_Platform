using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="Resolution"/> value object.
/// Verifies construction rules, validation constraints, equality behavior,
/// and proper exception handling for invalid inputs.
/// </summary>
public class ResolutionTests
{
    /// <summary>
    /// Ensures that a <see cref="Resolution"/> instance is created successfully
    /// when all values (Width, Height) are positive.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateValidResolution_WhenAllValuesArePositive()
    {
        // Arrange
        int width = 1920;
        int height = 1080;

        // Act
        var resolution = new Resolution(width, height);

        // Assert
        resolution.Width.Should().Be(width);
        resolution.Height.Should().Be(height);
    }

    /// <summary>
    /// Ensures that the constructor throws an <see cref="InvalidResolutionException"/>
    /// when any dimension (width or height) is zero or negative.
    /// </summary>
    [Theory]
    [InlineData(0, 1080)]    // Zero width
    [InlineData(1920, 0)]    // Zero height
    [InlineData(-1920, 1080)] // Negative width
    [InlineData(1920, -1080)] // Negative height
    public void Constructor_ShouldThrowInvalidResolutionException_WhenAnyDimensionIsZeroOrNegative(int width, int height)
    {
        // Act
        Action act = () => new Resolution(width, height);

        // Assert
        act.Should()
            .Throw<InvalidResolutionException>()
            .WithMessage("*greater than zero*");
    }

    /// <summary>
    /// Ensures that dimensions exceeding the maximum allowed value
    /// (e.g., 20000) throw an <see cref="InvalidResolutionException"/>.
    /// </summary>
    [Theory]
    [InlineData(20000, 1080)] // Width too big
    [InlineData(1920, 20000)] // Height too big
    public void Constructor_ShouldThrowInvalidResolutionException_WhenAnyDimensionExceedsMaximum(int width, int height)
    {
        // Act
        Action act = () => new Resolution(width, height);
        // Assert
        act.Should()
            .Throw<InvalidResolutionException>()
            .WithMessage("*exceeds maximum allowed value*");
    }

    /// <summary>
    /// Verifies that two <see cref="Resolution"/> instances with the same values are equal.
    /// </summary>
    public void Equals_ShouldReturnTrue_WhenResolutionsHaveSameValues()
    {
        // Arrange
        var r1 = new Resolution(1660, 980);
        var r2 = new Resolution(1660, 980);

        // Act & Assert
        r1.Should().Be(r2);
    }

    /// <summary>
    /// Verifies that two <see cref="Resolution"/> instances with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_ShouldReturnFalse_WhenResolutionsHaveDifferentValues()
    {
        // Arrange
        var r1 = new Resolution(1920, 1080);
        var r2 = new Resolution(1280, 720);

        // Act & Assert
        r1.Should().NotBe(r2);
    }

    /// <summary>
    /// Ensures that equal <see cref="Resolution"/> instances produce the same hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_ShouldBeEqual_ForEqualResolutions()
    {
        // Arrange
        var r1 = new Resolution(2560, 1440);
        var r2 = new Resolution(2560, 1440);

        // Act & Assert
        r1.GetHashCode().Should().Be(r2.GetHashCode());
    }
}
