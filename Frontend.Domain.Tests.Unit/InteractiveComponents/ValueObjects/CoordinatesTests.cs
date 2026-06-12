using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;

/// <summary>
/// Contains unit tests for the <see cref="Coordinates"/> value object.
/// Verifies construction rules, validation constraints, equality behavior,
/// and proper exception handling for invalid inputs.
/// </summary>
public class CoordinatesTests
{
    /// <summary>
    /// Ensures that a <see cref="Coordinates"/> instance is created successfully
    /// when all values (X, Y, Z) are positive and finite.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateValidCoordinates_WhenAllValuesArePositive()
    {
        // Arrange
        double x = 5.0;
        double y = 10.0;
        double z = 2.5;

        // Act
        var coordinates = new Coordinates(x, y, z);

        // Assert
        coordinates.X.Should().Be(x);
        coordinates.Y.Should().Be(y);
        coordinates.Z.Should().Be(z);
    }

    /// <summary>
    /// Verifies that the constructor throws an <see cref="InvalidCoordinatesException"/>
    /// when any coordinate (X, Y, or Z) is NaN, positive infinity, or negative infinity.
    /// </summary>
    [Theory]
    [InlineData(double.NaN, 1, 1)]
    [InlineData(double.PositiveInfinity, 1, 1)]
    [InlineData(double.NegativeInfinity, 1, 1)]
    [InlineData(1, double.NaN, 1)]
    [InlineData(1, double.PositiveInfinity, 1)]
    [InlineData(1, double.NegativeInfinity, 1)]
    [InlineData(1, 1, double.NaN)]
    [InlineData(1, 1, double.PositiveInfinity)]
    [InlineData(1, 1, double.NegativeInfinity)]
    public void Constructor_ShouldThrowInvalidCoordinatesException_WhenAnyValueIsNaNOrInfinity(
        double x, double y, double z)
    {
        // Act
        Action act = () => new Coordinates(x, y, z);

        // Assert
        act.Should()
            .Throw<InvalidCoordinatesException>()
            .WithMessage("*valid finite number*");
    }

    /// <summary>
    /// Verifies that the constructor throws an exception when any coordinate exceeds
    /// the maximum allowed value (e.g., 10,000 units).
    /// </summary>
    [Theory]
    [InlineData(10001, 0, 0)]
    [InlineData(0, -10001, 0)]
    [InlineData(0, 0, 10001)]
    public void Constructor_ShouldThrowInvalidCoordinatesException_WhenAnyCoordinateExceedsLimit(
        double x, double y, double z)
    {
        // Act
        Action act = () => new Coordinates(x, y, z);

        // Assert
        act.Should()
            .Throw<InvalidCoordinatesException>()
            .WithMessage("*is out of valid range*");
    }

    /// <summary>
    /// Confirms that two <see cref="Coordinates"/> instances with identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_ShouldReturnTrue_WhenCoordinatesHaveSameValues()
    {
        // Arrange
        var c1 = new Coordinates(5, 10, 2);
        var c2 = new Coordinates(5, 10, 2);

        // Act & Assert
        c1.Should().Be(c2);
    }

    /// <summary>
    /// Confirms that two <see cref="Coordinates"/> instances with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_ShouldReturnFalse_WhenCoordinatesHaveDifferentValues()
    {
        // Arrange
        var c1 = new Coordinates(5, 10, 2);
        var c2 = new Coordinates(6, 10, 2);

        // Act & Assert
        c1.Should().NotBe(c2);
    }

    /// <summary>
    /// Ensures that equal <see cref="Coordinates"/> instances produce identical hash codes.
    /// </summary>
    [Fact]
    public void GetHashCode_ShouldBeEqual_ForEqualCoordinates()
    {
        // Arrange
        var c1 = new Coordinates(5, 10, 2);
        var c2 = new Coordinates(5, 10, 2);

        // Act & Assert
        c1.GetHashCode().Should().Be(c2.GetHashCode());
    }
}
