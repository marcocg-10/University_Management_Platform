using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Dimensions"/> value object.
/// Validates construction rules, input constraints, equality logic,
/// and string representation to ensure domain integrity.
/// </summary>
public class DimensionsTests
{

    /// <summary>
    /// Ensures that a <see cref="Dimensions"/> instance is created successfully
    /// when all provided values (width, height, depth) are positive.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateValidDimensions_WhenAllValuesArePositive()
    {
        // Arrange
        double width = 10.0;
        double height = 20.0;
        double depth = 5.0;

        // Act
        var dimensions = new Dimensions(width, height, depth);

        // Assert
        dimensions.Width.Should().Be(width);
        dimensions.Height.Should().Be(height);
        dimensions.Depth.Should().Be(depth);
    }

    /// <summary>
    /// Ensures that the constructor throws an <see cref="InvalidDimensionsException"/>
    /// when any dimension (width, height, or depth) is NaN or an infinite value.
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
    public void Constructor_ShouldThrowInvalidDimensionsException_WhenAnyValueIsNaNOrInfinity(
        double width, double height, double depth)
    {
        // Act
        Action act = () => new Dimensions(width, height, depth);

        // Assert
        act.Should()
            .Throw<InvalidDimensionsException>()
            .WithMessage("*valid finite number*");
    }

    /// <summary>
    /// Ensures that an exception is thrown when any dimension
    /// is zero or negative, enforcing strictly positive domain rules.
    /// </summary>
    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(-1, 1, 1)]
    [InlineData(1, -1, 1)]
    [InlineData(1, 1, -1)]
    public void Constructor_ShouldThrowInvalidDimensionsException_WhenAnyDimensionIsZeroOrNegative(
        double width, double height, double depth)
    {
        // Act
        Action act = () => new Dimensions(width, height, depth);

        // Assert
        act.Should()
            .Throw<InvalidDimensionsException>()
            .WithMessage("*greater than zero*");
    }

    /// <summary>
    /// Ensures that dimensions exceeding the maximum allowed value
    /// (e.g., 1000 units) trigger a validation exception.
    /// </summary>
    [Theory]
    [InlineData(1001, 10, 10)]
    [InlineData(10, 1001, 10)]
    [InlineData(10, 10, 1001)]
    public void Constructor_ShouldThrowInvalidDimensionsException_WhenAnyDimensionExceedsLimit(
        double width, double height, double depth)
    {
        // Act
        Action act = () => new Dimensions(width, height, depth);

        // Assert
        act.Should()
            .Throw<InvalidDimensionsException>()
            .WithMessage("*exceeds maximum allowed value*");
    }

    /// <summary>
    /// Verifies that two <see cref="Dimensions"/> instances with identical values are equal.
    /// </summary>
    [Fact]
    public void Equals_ShouldReturnTrue_WhenDimensionsHaveSameValues()
    {
        // Arrange
        var d1 = new Dimensions(10, 20, 5);
        var d2 = new Dimensions(10, 20, 5);

        // Act & Assert
        d1.Should().Be(d2);
    }

    /// <summary>
    /// Verifies that two <see cref="Dimensions"/> instances with different values are not equal.
    /// </summary>
    [Fact]
    public void Equals_ShouldReturnFalse_WhenDimensionsHaveDifferentValues()
    {
        // Arrange
        var d1 = new Dimensions(10, 20, 5);
        var d2 = new Dimensions(11, 20, 5);

        // Act & Assert
        d1.Should().NotBe(d2);
    }

    /// <summary>
    /// Ensures that equal <see cref="Dimensions"/> instances produce the same hash code.
    /// </summary>
    [Fact]
    public void GetHashCode_ShouldBeEqual_ForEqualDimensions()
    {
        // Arrange
        var d1 = new Dimensions(10, 20, 5);
        var d2 = new Dimensions(10, 20, 5);

        // Act & Assert
        d1.GetHashCode().Should().Be(d2.GetHashCode());
    }
}
