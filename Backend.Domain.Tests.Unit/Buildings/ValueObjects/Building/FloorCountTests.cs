using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.Building;

/// <summary>
/// Unit tests for the <see cref="FloorCount"/> value object.
/// </summary>
public class FloorCountTests
{
    /// <summary>
    /// Should return true for valid floor count values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void TryCreate_WithValidFloorCountValues_ReturnsTrue(int input)
    {
        // Act
        var result = FloorCount.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid floor counts should pass validation");
    }

    /// <summary>
    /// Should return false for invalid floor count values (zero or negative).
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TryCreate_WithInvalidFloorCountValues_ReturnsFalse(int input)
    {
        // Act
        var result = FloorCount.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "floor count must be a positive integer");
    }

    /// <summary>
    /// Should produce a non-null instance for valid floor count values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void TryCreate_WithValidFloorCountValues_ReturnsNonNullInstance(int input)
    {
        // Act
        FloorCount.TryCreate(input, out var floorCount);

        // Assert
        (floorCount is not null).Should().BeTrue(because: "valid floor counts should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void TryCreate_WithValidFloorCountValues_SetsCorrectValue(int input)
    {
        // Act
        FloorCount.TryCreate(input, out var floorCount);

        // Assert
        floorCount!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void Create_WithValidFloorCountValues_ReturnsInstance(int input)
    {
        // Act
        var floorCount = FloorCount.Create(input);

        // Assert
        floorCount.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid floor count values.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    public void Create_WithInvalidFloorCountValues_ThrowsException(int input)
    {
        // Act
        var act = () => FloorCount.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid floor counts should trigger domain validation");
    }

    /// <summary>
    /// Should compare equality based on value.
    /// </summary>
    [Fact]
    public void FloorCount_Equality_IsBasedOnValue()
    {
        // Arrange
        var floorCount1 = FloorCount.Create(4);
        var floorCount2 = FloorCount.Create(4);
        var floorCount3 = FloorCount.Create(5);

        // Assert
        floorCount1.Should().Be(floorCount2, because: "FloorCounts with the same value should be equal");
        floorCount1.Should().NotBe(floorCount3, because: "FloorCounts with different values should not be equal");
    }
}
