using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="Depth"/> value object.
/// Validates creation logic, boundary conditions, and exception handling.
/// </summary>
public class DepthTests
{
    /// <summary>
    /// Should return true for valid depth values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidDepthValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = Depth.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid depth values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid depth values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidDepthValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = Depth.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "Depth values must be between 0 < Depth ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid depth values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidDepthValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        Depth.TryCreate(input, out var depth);

        // Assert
        (depth is not null).Should().BeTrue(because: "valid depth values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidDepthValues_SetsCorrectValue(decimal input)
    {
        // Act
        Depth.TryCreate(input, out var depth);

        // Assert
        depth!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidDepthValues_ReturnsInstance(decimal input)
    {
        // Act
        var depth = Depth.Create(input);

        // Assert
        depth.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid depth values.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void Create_WithInvalidDepthValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => Depth.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid depth values should trigger domain validation");
    }
}
