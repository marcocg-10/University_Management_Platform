using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.Building;

/// <summary>
/// Unit tests for the <see cref="BuildingName"/> value object.
/// Validates creation logic, boundary conditions, and exception handling.
/// </summary>
public class BuildingNameTests
{
    /// <summary>
    /// Should return true for valid building name values.
    /// </summary>
    [Theory]
    [InlineData("Main Hall")]
    [InlineData("Entrance")]
    [InlineData("A12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void TryCreate_WithValidNameValues_ReturnsTrue(string input)
    {
        // Act
        var result = BuildingName.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid building names should pass validation");
    }

    /// <summary>
    /// Should return false for invalid building name values (empty, whitespace, or null).
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TryCreate_WithEmptyOrNullName_ReturnsFalse(string? input)
    {
        // Act
        var result = BuildingName.TryCreate(input!, out var _);

        // Assert
        result.Should().BeFalse(because: "building name must be non-empty and non-null");
    }

    /// <summary>
    /// Should return false for building name values longer than 200 characters.
    /// </summary>
    [Fact]
    public void TryCreate_WithTooLongName_ReturnsFalse()
    {
        // Arrange
        var longName = new string('a', 201);

        // Act
        var result = BuildingName.TryCreate(longName, out var _);

        // Assert
        result.Should().BeFalse(because: "building name must not exceed 200 characters");
    }

    /// <summary>
    /// Should produce a non-null instance for valid building name values.
    /// </summary>
    [Theory]
    [InlineData("Main Hall")]
    [InlineData("Entrance")]
    public void TryCreate_WithValidNameValues_ReturnsNonNullInstance(string input)
    {
        // Act
        BuildingName.TryCreate(input, out var name);

        // Assert
        (name is not null).Should().BeTrue(because: "valid building names should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData("Main Hall")]
    [InlineData("Entrance")]
    public void TryCreate_WithValidNameValues_SetsCorrectValue(string input)
    {
        // Act
        BuildingName.TryCreate(input, out var name);

        // Assert
        name!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData("Main Hall")]
    [InlineData("Entrance")]
    public void Create_WithValidNameValues_ReturnsInstance(string input)
    {
        // Act
        var name = BuildingName.Create(input);

        // Assert
        name.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid building name values.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyOrNullName_ThrowsException(string? input)
    {
        // Act
        var act = () => BuildingName.Create(input!);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "empty or null building names should trigger domain validation");
    }

    /// <summary>
    /// Should throw BuildingDataException for building name values longer than 200 characters.
    /// </summary>
    [Fact]
    public void Create_WithTooLongName_ThrowsException()
    {
        // Arrange
        var longName = new string('a', 201);

        // Act
        var act = () => BuildingName.Create(longName);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "building names longer than 200 characters should be considered invalid");
    }
}
