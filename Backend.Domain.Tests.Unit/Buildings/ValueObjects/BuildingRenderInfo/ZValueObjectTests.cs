using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="Z"/> value object.
/// Validates creation logic, boundary constraints, and exception handling for Z coordinate values.
/// </summary>
public class ZValueObjectTests
{
    /// <summary>
    /// Should return true for valid Z values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidZValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = Z.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid Z values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid Z values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidZValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = Z.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "Z values must be between -2000000 ≤ Z ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid Z values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidZValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        Z.TryCreate(input, out var z);

        // Assert
        (z is not null).Should().BeTrue(because: "valid Z values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidZValues_SetsCorrectValue(decimal input)
    {
        // Act
        Z.TryCreate(input, out var z);

        // Assert
        z!.ZValue.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidZValues_ReturnsInstance(decimal input)
    {
        // Act
        var z = Z.Create(input);

        // Assert
        z.ZValue.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid Z values.
    /// </summary>
    [Theory]
    [InlineData(-2000001)]
    [InlineData(2000001)]
    public void Create_WithInvalidZValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => Z.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid Z values should trigger domain validation");
    }
}
