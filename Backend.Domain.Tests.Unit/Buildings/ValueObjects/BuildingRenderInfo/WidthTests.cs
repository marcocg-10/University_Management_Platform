using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="Width"/> value object.
/// Validates creation logic, boundary conditions, and exception handling.
/// </summary>
public class WidthTests
{
    /// <summary>
    /// Should return true for valid width values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidWidthValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = Width.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid width values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid width values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidWidthValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = Width.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "Width values must be between 0 < Width ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid width values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidWidthValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        Width.TryCreate(input, out var width);

        // Assert
        (width is not null).Should().BeTrue(because: "valid width values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidWidthValues_SetsCorrectValue(decimal input)
    {
        // Act
        Width.TryCreate(input, out var width);

        // Assert
        width!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidWidthValues_ReturnsInstance(decimal input)
    {
        // Act
        var width = Width.Create(input);

        // Assert
        width.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid width values.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void Create_WithInvalidWidthValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => Width.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid width values should trigger domain validation");
    }
}
