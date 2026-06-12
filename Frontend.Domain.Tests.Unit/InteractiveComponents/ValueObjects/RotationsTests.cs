
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;


public class RotationsTests
{

    /// <summary>
    /// Ensures that the constructor creates a valid <see cref="Rotations"/> instance
    /// when all rotation values are within the acceptable range.
    /// </summary>
    [Fact]
    public void Constructor_ShouldCreateValidRotations_WhenAllValuesInRange()
    {
        // Arrange
        double xRotation = 10.0;
        double yRotation = 20.0;
        double zRotation = 5.0;

        // Act
        var rotations = new Rotations(xRotation, yRotation, zRotation);

        // Assert
        rotations.XAxisRotation.Should().Be(xRotation);
        rotations.YAxisRotation.Should().Be(yRotation);
        rotations.ZAxisRotation.Should().Be(zRotation);
    }


    /// <summary>
    /// Ensures that the constructor throws an <see cref="InvalidDimensionsException"/>
    /// when any rotation (X, Y, or Z) is NaN, positive infinity, or negative infinity.
    /// </summary>
    /// <param name="xRotation"></param>
    /// <param name="yRotation"></param>
    /// <param name="zRotation"></param>
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
    public void Constructor_ShouldThrowInvalidRotationsException_WhenAnyValueIsNaNOrInfinity(
        double xRotation, double yRotation, double zRotation)
    {
        // Act
        Action act = () => new Rotations(xRotation, yRotation, zRotation);

        // Assert
        act.Should()
            .Throw<InvalidRotationsException>()
            .WithMessage("*axis rotation must be a valid finite number*");
    }

    /// <summary>
    /// Ensures that the constructor throws an <see cref="InvalidDimensionsException"/>
    /// when any rotation (X, Y, or Z) exceeds the maximum allowed limit (e.g., 360 degrees).
    /// </summary>
    /// <param name="xRotation"></param>
    /// <param name="yRotation"></param>
    /// <param name="zRotation"></param>
    [Theory]
    [InlineData(1001, 10, 10)]
    [InlineData(10, 1001, 10)]
    [InlineData(10, 10, 1001)]
    public void Constructor_ShouldThrowInvalidRotationsException_WhenAnyRotationExceedsLimit(
        double xRotation, double yRotation, double zRotation)
    {
        // Act
        Action act = () => new Rotations(xRotation, yRotation, zRotation);

        // Assert
        act.Should()
            .Throw<InvalidRotationsException>()
            .WithMessage("*is out of valid range*");
    }
}
