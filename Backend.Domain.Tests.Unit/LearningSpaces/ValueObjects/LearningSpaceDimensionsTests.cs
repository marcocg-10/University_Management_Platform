using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.ValueObjects;

public class LearningSpaceDimensionsTests
{
    /// <summary>
    ///     Contains unit tests for the <see cref="LearningSpaceDimensions"/> value object,
    ///     verifying its creation logic with valid, zero, and negative dimension values.
    /// </summary>

    // Test cases for valid inputs
    [Theory]
    [InlineData(4.5f, 6.0f, 2.6f)]
    [InlineData(8.0f, 12.0f, 3.5f)]
    [InlineData(10.0f, 18.0f, 5.0f)]
    [InlineData(0.0001f, 209.7892f, 100.52f)]
    public void TryCreate_GivenValidValues_ReturnsTrue(float w, float l, float h)
    {

        // Act
        var success = LearningSpaceDimensions.TryCreate(w, l, h, out var result);

        // Assert
        success.Should().BeTrue(because: "all dimensions are positive values");
        result.Should().NotBeNull(because: "valid inputs should produce a non-null value object");

        result!.Width.Should().Be(w, because: "the width must match the input value");
        result.Length.Should().Be(l, because: "the length must match the input value");
        result.Height.Should().Be(h, because: "the height must match the input value");
    }

    [Theory]
    [InlineData(0f, 10f, 5f)]   // width = 0
    [InlineData(10f, 0f, 5f)]   // length = 0
    [InlineData(10f, 10f, 0f)]  // height = 0
    public void TryCreate_GivenZeroValues_ReturnsFalse(float w, float l, float h)
    {
        // Act
        var success = LearningSpaceDimensions.TryCreate(w, l, h, out var result);

        // Assert
        success.Should().BeFalse(because: "dimensions must be greater than zero");
        result.Should().BeNull(because: "invalid input should not create a value object");
    }

    [Theory]
    [InlineData(-1f, 10f, 5f)]   // width < 0
    [InlineData(10f, -2f, 5f)]   // length < 0
    [InlineData(10f, 10f, -3f)]  // height < 0
    public void TryCreate_GivenNegativeValues_ReturnsFalse(float w, float l, float h)
    {
        // Act
        var success = LearningSpaceDimensions.TryCreate(w, l, h, out var result);

        // Assert
        success.Should().BeFalse(because: "dimensions cannot be negative values");
        result.Should().BeNull(because: "invalid input should not create a value object");
    }

    [Theory]
    [InlineData(float.NaN, 10f, 5f)]    // width = NaN
    [InlineData(10f, float.NaN, 5f)]    // length = NaN
    [InlineData(10f, 10f, float.NaN)]   // height = NaN
    public void TryCreate_GivenNaNValues_ReturnsFalse(float w, float l, float h)
    {
        // Act
        var success = LearningSpaceDimensions.TryCreate(w, l, h, out var result);

        // Assert
        success.Should().BeFalse(because: "dimensions cannot be NaN");
        result.Should().BeNull(because: "invalid input should not create a value object");
    }

    [Theory]
    [InlineData(float.PositiveInfinity, 10f, 5f)]   // width = +∞
    [InlineData(float.NegativeInfinity, 10f, 5f)]   // width = -∞
    [InlineData(10f, float.NegativeInfinity, 5f)]   // length = -∞
    [InlineData(10f, float.PositiveInfinity, 5f)]   // length = +∞
    [InlineData(10f, 10f, float.PositiveInfinity)]  // height = +∞
    [InlineData(10f, 10f, float.NegativeInfinity)]  // height = -∞
    public void TryCreate_GivenInfiniteFloatValues_ReturnsFalse(float w, float l, float h)
    {
        // Act
        var success = LearningSpaceDimensions.TryCreate(w, l, h, out var result);

        // Assert
        success.Should().BeFalse(because: "dimensions cannot be infinite");
        result.Should().BeNull(because: "invalid input should not create a value object");
    }
}
