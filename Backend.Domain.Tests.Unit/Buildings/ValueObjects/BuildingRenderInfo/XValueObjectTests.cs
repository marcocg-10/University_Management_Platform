using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="X"/> value object.
/// Validates creation logic, boundary conditions, and exception handling for X coordinate values.
/// </summary>
public class XValueObjectTests
{
    /// <summary>
    /// Should return true for valid X values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidXValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = X.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid X values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid X values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidXValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = X.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "X values must be between -2000000 ≤ X ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid X values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidXValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        X.TryCreate(input, out var x);

        // Assert
        (x is not null).Should().BeTrue(because: "valid X values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidXValues_SetsCorrectValue(decimal input)
    {
        // Act
        X.TryCreate(input, out var x);

        // Assert
        x!.XValue.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidXValues_ReturnsInstance(decimal input)
    {
        // Act
        var x = X.Create(input);

        // Assert
        x.XValue.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid X values.
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void Create_WithInvalidXValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => X.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid X values should trigger domain validation");
    }
}
