using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.LearningSpaces.ValueObjects;

/// <summary>
/// Unit tests for the LearningSpaceCoordinates value object.
/// </summary>
public class LearningSpaceCoordinatesTests
{
    /// <summary>
    /// Fact unit test for the LearningSpaceCoordinates value object.
    /// </summary>
    /// <remarks>
    /// Tests whether a new LearningSpaceCoordinates instance is created when given valid axis
    /// coordinate values. This test passes if a new instance is created using the given values.
    /// </remarks>
    [Fact]
    public void TryCreate_GivenValidCoordinates_ReturnsTrue()
    {
        // Arrange
        float xAxis = 10f, yAxis = 2f, zAxis = 3f;

        // Act
        var objectCreated = LearningSpaceCoordinates.TryCreate(xAxis, yAxis, zAxis, out var result);

        // Assert
        objectCreated.Should().BeTrue(because: "all axis coordinates are valid.");
        result.Should().NotBeNull(
            because: "valid axis coordinates should produce a ",
            "LearningSpaceCoordinates value object");
        result.XCoordinate.Should().Be(xAxis, because: "the x axis value must match the x input value");
        result.YCoordinate.Should().Be(yAxis, because: "the y axis value must match the y input value");
        result.ZCoordinate.Should().Be(zAxis, because: "the z axis value must match the z input value");
    }

    /// <summary>
    /// Theory unit test for the LearningSpaceCoordinates value objects (negatives accepted).
    /// </summary>
    /// <remarks>
    /// Tests whether a new LearningSpaceCoordinates instance is not created when given invalid axis
    /// coordinate values. These tests pass if for every test case, a new instance is not created.
    /// </remarks>
    /// <param name="xAxis">x-axis coordinate</param>
    /// <param name="yAxis">y-axis coordinate</param>
    /// <param name="zAxis">z-axis coordinate</param>
    [Theory]
    [InlineData(-1f, 2f, 10f)]   // x-axis = negative
    [InlineData(10f, -1f, 20f)]  // y-axis = negative
    [InlineData(10f, 0f, -1f)]  // z-axis = negative
    [InlineData(-1f, -1f, 10f)]  // x-axis and y-axis = negative
    [InlineData(10f, -1f, -1f)]  // y-axis and z-axis = negative
    [InlineData(-9.2f, 10f, -10f)]  // x-axis and z-axis = negative
    [InlineData(-1f, -10f, -4.5f)]  // all = negative
    public void TryCreate_GivenNegativeCoordinates_ReturnsTrue(float xAxis, float yAxis, float zAxis)
    {
        // Act
        var objectCreated = LearningSpaceCoordinates.TryCreate(xAxis, yAxis, zAxis, out var result);

        // Assert
        objectCreated.Should().BeTrue(because: "all axis coordinates are valid.");
        result.Should().NotBeNull(
            because: "valid axis coordinates should produce a ",
            "LearningSpaceCoordinates value object");
        result.XCoordinate.Should().Be(xAxis, because: "the x axis value must match the x input value");
        result.YCoordinate.Should().Be(yAxis, because: "the y axis value must match the y input value");
        result.ZCoordinate.Should().Be(zAxis, because: "the z axis value must match the z input value");
    }

    /// <summary>
    /// Theory unit test for the LearningSpaceCoordinates value object (no nulls).
    /// </summary>
    /// <param name="xAxis">x-axis coordinate</param>
    /// <param name="yAxis">y-axis coordinate</param>
    /// <param name="zAxis">z-axis coordinate</param>
    [Theory]
    [InlineData(float.NaN, 2f, 3f)]   // x-axis = NaN
    [InlineData(10f, float.NaN, 3f)]  // y-axis = NaN
    [InlineData(10f, 2f, float.NaN)]  // z-axis = NaN
    [InlineData(float.NaN, float.NaN, 3f)]  // x-axis and y-axis = NaN
    [InlineData(10f, float.NaN, float.NaN)]  // y-axis and z-axis = NaN
    [InlineData(float.NaN, 2f, float.NaN)]  // x-axis and z-axis = NaN
    [InlineData(float.NaN, float.NaN, float.NaN)]  // all = NaN
    public void TryCreate_GivenNaNCoordinates_ReturnsFalse(float xAxis, float yAxis, float zAxis)
    {
        // Act
        var objectCreated = LearningSpaceCoordinates.TryCreate(xAxis, yAxis, zAxis, out var result);
       
        // Assert
        objectCreated.Should().BeFalse(because: "all coordinates must not be null");
        result.Should().BeNull(
            because: "null coordinates should not be used to create a ",
            "LearningSpaceCoordinates value object");
    }

    [Theory]
    [InlineData(float.PositiveInfinity, 2f, 3f)]   // x-axis = +infinity
    [InlineData(10f, float.PositiveInfinity, 3f)]  // y-axis = +infinity
    [InlineData(10f, 2f, float.PositiveInfinity)]  // z-axis = +infinity
    [InlineData(float.PositiveInfinity, float.PositiveInfinity, 3f)]  // x-axis and y-axis = +infinity
    [InlineData(10f, float.PositiveInfinity, float.PositiveInfinity)]  // y-axis and z-axis = +infinity
    [InlineData(float.PositiveInfinity, 2f, float.PositiveInfinity)]  // x-axis and z-axis = +infinity
    [InlineData(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)]  // all = +infinity
    public void Try_Create_GivenInfiniteCoordinates_ReturnsFalse(float xAxis, float yAxis, float zAxis)
    {         
        // Act
        var objectCreated = LearningSpaceCoordinates.TryCreate(xAxis, yAxis, zAxis, out var result);
        
        // Assert
        objectCreated.Should().BeFalse(because: "all coordinates must be finite values");
        result.Should().BeNull(
            because: "infinite coordinates should not be used to create a ",
            "LearningSpaceCoordinates value object");
    }
}
