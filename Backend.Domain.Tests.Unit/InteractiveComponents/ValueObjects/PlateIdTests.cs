using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.InteractiveComponents.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="PlateId"/> value object.
/// Ensures proper validation, equality behavior, and formatting rules.
/// </summary>
public class PlateIdTests
{
    /// <summary>
    /// Verifies that a valid Plate ID creates successfully
    /// and retains its value in uppercase format.
    /// </summary>
    [Fact]
    public void Create_ValidPlateId_ShouldCreateSuccessfully()
    {
        // Arrange
        var validValue = "123456";

        // Act
        var plateId = new PlateId(validValue);

        // Assert
        plateId.Value.Should().Be("123456");
        plateId.ToString().Should().Be("123456");
    }

    /// <summary>
    /// Ensures that Plate IDs are automatically stored in uppercase.
    /// </summary>
    [Fact]
    public void Create_ValidPlateId_ShouldBeStoredInUppercase()
    {
        // Arrange
        var lowerCaseValue = "001234";

        // Act
        var plateId = new PlateId(lowerCaseValue);

        // Assert
        plateId.Value.Should().Be("001234");
    }

    /// <summary>
    /// Validates that two Plate IDs with the same value are considered equal.
    /// </summary>
    [Fact]
    public void PlateIds_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var plate1 = new PlateId("654321");
        var plate2 = new PlateId("654321");

        // Assert
        plate1.Should().Be(plate2);
        plate1.GetHashCode().Should().Be(plate2.GetHashCode());
    }

    /// <summary>
    /// Verifies that two Plate IDs with different values are not equal.
    /// </summary>
    [Fact]
    public void PlateIds_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var plate1 = new PlateId("123456");
        var plate2 = new PlateId("654321");

        // Assert
        plate1.Should().NotBe(plate2);
    }

    /// <summary>
    /// Ensures that attempting to create a Plate ID with a null value throws an exception.
    /// </summary>
    [Fact]
    public void Create_NullPlateId_ShouldThrow()
    {
        // Act
        Action act = () => new PlateId(null!);

        // Assert
        act.Should()
            .Throw<InvalidPlateIdException>()
            .WithMessage("Plate ID cannot be null.");
    }

    /// <summary>
    /// Ensures that creating a Plate ID with an empty or whitespace-only value throws an exception.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Create_EmptyOrWhitespacePlateId_ShouldThrow(string value)
    {
        // Act
        Action act = () => new PlateId(value);

        // Assert
        act.Should()
            .Throw<InvalidPlateIdException>()
            .WithMessage("Plate ID cannot be empty or whitespace.");
    }

    /// <summary>
    /// Verifies that invalid formats (letters, symbols, or incorrect patterns)
    /// trigger a <see cref="InvalidPlateIdException"/>.
    /// </summary>
    [Theory]
    [InlineData("12345")]     // too short
    [InlineData("1234567")]   // too long
    [InlineData("12A456")]    // contains letter
    [InlineData("12-456")]    // contains symbol
    [InlineData("12 456")]    // contains space
    [InlineData("ABCDEF")]    // letters only
    [InlineData("!@#$%^")]    // special characters
    public void Create_InvalidFormat_ShouldThrow(string invalidValue)
    {
        // Act
        Action act = () => new PlateId(invalidValue);

        // Assert
        act.Should()
            .Throw<InvalidPlateIdException>()
            .WithMessage(
                $"Plate ID format is invalid. Expected format: 6 digits (e.g., '123456'). Given: '{invalidValue}'.");
    }
}
