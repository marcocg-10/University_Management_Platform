using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="LearningSpaceColor"/> value object.
/// Validates hexadecimal format enforcement, input boundaries, and exception handling.
/// </summary>
public class LearningSpaceColorTests
{
    /// <summary>
    /// Should return true for valid hexadecimal color formats.
    /// </summary>
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#ABCDEF")]
    [InlineData("#123456")]
    [InlineData("#FFF")]
    [InlineData("#ABC")]
    public void TryCreate_WithValidColorFormats_ReturnsTrue(string input)
    {
        // Act
        var result = LearningSpaceColor.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid hexadecimal formats should pass validation");
    }

    /// <summary>
    /// Should return false for invalid hexadecimal color formats.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("123456")]
    [InlineData("#ZZZZZZZ")]
    [InlineData("#FF")]
    [InlineData("#FFFFFFFF")]
    [InlineData("blue")]
    [InlineData("#12345")]
    public void TryCreate_WithInvalidColorFormats_ReturnsFalse(string input)
    {
        // Act
        var result = LearningSpaceColor.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "invalid formats should fail validation");
    }

    /// <summary>
    /// Should produce a non-null instance for valid color formats.
    /// </summary>
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#ABCDEF")]
    [InlineData("#FFF")]
    public void TryCreate_WithValidColorFormats_ReturnsNonNullInstance(string input)
    {
        // Act
        LearningSpaceColor.TryCreate(input, out var color);

        // Assert
        (color is not null).Should().BeTrue(because: "valid formats should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#ABCDEF")]
    [InlineData("#FFF")]
    public void TryCreate_WithValidColorFormats_SetsCorrectValue(string input)
    {
        // Act
        LearningSpaceColor.TryCreate(input, out var color);

        // Assert
        color!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData("#FFFFFF")]
    [InlineData("#ABCDEF")]
    [InlineData("#FFF")]
    public void Create_WithValidColorFormats_ReturnsInstance(string input)
    {
        // Act
        var color = LearningSpaceColor.Create(input);

        // Assert
        color.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw ValidationException for invalid color formats.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("blue")]
    [InlineData("#12345")]
    [InlineData("#FFFFFFFF")]
    public void Create_WithInvalidColorFormats_ThrowsException(string input)
    {
        // Act
        var act = () => LearningSpaceColor.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "invalid formats should trigger domain validation");
    }
}