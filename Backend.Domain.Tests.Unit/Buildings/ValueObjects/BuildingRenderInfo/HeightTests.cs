using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="Heigth"/> value object.
/// Validates creation logic, boundary conditions, and exception handling.
/// </summary>
public class HeightTests
{
    /// <summary>
    /// Should return true for valid height values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    [InlineData(1999999.99)]
    public void TryCreate_WithValidHeigthValues_ReturnsTrue(decimal input)
    {
        // Act
        var result = Heigth.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid height values should pass validation");
    }

    /// <summary>
    /// Should return false for invalid height values (too small or too large).
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void TryCreate_WithInvalidHeigthValues_ReturnsFalse(decimal input)
    {
        // Act
        var result = Heigth.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "Height values must be between 0 < Height ≤ 2,000,000");
    }

    /// <summary>
    /// Should produce a non-null instance for valid height values.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidHeigthValues_ReturnsNonNullInstance(decimal input)
    {
        // Act
        Heigth.TryCreate(input, out var heigth);

        // Assert
        (heigth is not null).Should().BeTrue(because: "valid height values should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void TryCreate_WithValidHeigthValues_SetsCorrectValue(decimal input)
    {
        // Act
        Heigth.TryCreate(input, out var heigth);

        // Assert
        heigth!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(12.34)]
    public void Create_WithValidHeigthValues_ReturnsInstance(decimal input)
    {
        // Act
        var heigth = Heigth.Create(input);

        // Assert
        heigth.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid height values.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(2000001)]
    public void Create_WithInvalidHeigthValues_ThrowsException(decimal input)
    {
        // Act
        var act = () => Heigth.Create(input);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "invalid height values should trigger domain validation");
    }
}
