using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="Y"/> value object.
/// Validates creation logic, boundary conditions, and exception handling for Y coordinate values.
/// </summary>
public class YValueObjectTests
{
    /// <summary>
    /// Should return true for valid Y values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidYValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = Y.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid Y values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid Y values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidYValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = Y.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "Y values must be between -2000000 ≤ Y ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid Y values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidYValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        Y.TryCreate(input, out var y);

        // Assert
        (y is not null).Should().BeTrue(because: "valid Y values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidYValues_SetsCorrectValue(decimal input)
    {
        // Act
        Y.TryCreate(input, out var y);

        // Assert
        y!.YValue.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidYValues_ReturnsInstance(decimal input)
    {
        // Act
        var y = Y.Create(input);

        // Assert
        y.YValue.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid Y values.
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void Create_WithInvalidYValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => Y.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid Y values should trigger domain validation");
    }
}
