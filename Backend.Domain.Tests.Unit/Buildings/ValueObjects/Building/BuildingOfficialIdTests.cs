using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.Building;

/// <summary>
/// Unit tests for the <see cref="BuildingOfficialId"/> value object.
/// Validates creation logic, input boundaries, and exception handling.
/// </summary>
public class BuildingOfficialIdTests
{
    /// <summary>
    /// Should return true for valid official ID values.
    /// </summary>
    [Theory]
    [InlineData("EDCI2023")]
    [InlineData("BLDG001")]
    [InlineData("B12345678901234567890123456789")]
    public void TryCreate_WithValidIdValues_ReturnsTrue(string input)
    {
        // Act
        var result = BuildingOfficialId.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid official IDs should pass validation");
    }

    /// <summary>
    /// Should return false for invalid official ID values (empty, whitespace, or null).
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TryCreate_WithEmptyOrNullId_ReturnsFalse(string? input)
    {
        // Act
        var result = BuildingOfficialId.TryCreate(input!, out var _);

        // Assert
        result.Should().BeFalse(because: "official ID must be non-empty and non-null");
    }

    /// <summary>
    /// Should return false for official ID values longer than 30 characters.
    /// </summary>
    [Fact]
    public void TryCreate_WithTooLongId_ReturnsFalse()
    {
        // Arrange
        var longId = new string('a', 31);

        // Act
        var result = BuildingOfficialId.TryCreate(longId, out var _);

        // Assert
        result.Should().BeFalse(because: "official ID must not exceed 30 characters");
    }

    /// <summary>
    /// Should produce a non-null instance for valid official ID values.
    /// </summary>
    [Theory]
    [InlineData("EDCI2023")]
    [InlineData("BLDG001")]
    public void TryCreate_WithValidIdValues_ReturnsNonNullInstance(string input)
    {
        // Act
        BuildingOfficialId.TryCreate(input, out var id);

        // Assert
        (id is not null).Should().BeTrue(because: "valid official IDs should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData("EDCI2023")]
    [InlineData("BLDG001")]
    public void TryCreate_WithValidIdValues_SetsCorrectValue(string input)
    {
        // Act
        BuildingOfficialId.TryCreate(input, out var id);

        // Assert
        id!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData("EDCI2023")]
    [InlineData("BLDG001")]
    public void Create_WithValidIdValues_ReturnsInstance(string input)
    {
        // Act
        var id = BuildingOfficialId.Create(input);

        // Assert
        id.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid official ID values.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyOrNullId_ThrowsException(string? input)
    {
        // Act
        var act = () => BuildingOfficialId.Create(input!);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "empty or null official IDs should trigger domain validation");
    }

    /// <summary>
    /// Should throw BuildingDataException for official ID values longer than 30 characters.
    /// </summary>
    [Fact]
    public void Create_WithTooLongId_ThrowsException()
    {
        // Arrange
        var longId = new string('a', 31);

        // Act
        var act = () => BuildingOfficialId.Create(longId);

        // Assert
        act.Should().Throw<BuildingDataException>(because: "official IDs longer than 30 characters should be considered invalid");
    }
}
