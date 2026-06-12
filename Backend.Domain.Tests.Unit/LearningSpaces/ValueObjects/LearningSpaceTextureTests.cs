using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.LearningSpaces.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="LearningSpaceTexture"/> value object.
/// Validates texture format enforcement, input boundaries, and exception handling.
/// </summary>
public class LearningSpaceTextureTests
{
    /// <summary>
    /// Should return true for valid texture formats.
    /// </summary>
    [Theory]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusion.png")]
    [InlineData("Outdoor_Wall_T14_Base_Color.png")]
    [InlineData("Outdoor_Wall_T14_Height.png")]
    [InlineData("Outdoor_Wall_T14_MaskMap.png")]
    public void TryCreate_WithValidTextureFormats_ReturnsTrue(string input)
    {
        // Act
        var result = LearningSpaceTexture.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "valid texture formats should pass validation");
    }

    /// <summary>
    /// Should return false for invalid texture formats.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusionOutdoor_Wall_T15_Ambient_occlusion.png")]
    [InlineData("Outdoor_Wall_T14_Height.pngOutdoor_Wall_T14_Height.png")]
    public void TryCreate_WithInvalidTextureFormats_ReturnsFalse(string input)
    {
        // Act
        var result = LearningSpaceTexture.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "invalid texture formats should fail validation");
    }

    /// <summary>
    /// Should produce a non-null instance for valid texture formats.
    /// </summary>
    [Theory]
    [InlineData("Outdoor_Wall_T14_Height.png")]
    [InlineData("Outdoor_Wall_T14_MaskMap.png")]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusion.png")]
    public void TryCreate_WithValidTextureFormats_ReturnsNonNullInstance(string input)
    {
        // Act
        LearningSpaceTexture.TryCreate(input, out var texture);

        // Assert
        (texture is not null).Should().BeTrue(because: "valid texture formats should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData("Outdoor_Wall_T14_Height.png")]
    [InlineData("Outdoor_Wall_T14_MaskMap.png")]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusion.png")]
    public void TryCreate_WithValidTextureFormats_SetsCorrectValue(string input)
    {
        // Act
        LearningSpaceTexture.TryCreate(input, out var texture);

        // Assert
        texture!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid input.
    /// </summary>
    [Theory]
    [InlineData("Outdoor_Wall_T14_Height.png")]
    [InlineData("Outdoor_Wall_T14_MaskMap.png")]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusion.png")]
    public void Create_WithValidTextureFormats_ReturnsInstance(string input)
    {
        // Act
        var texture = LearningSpaceTexture.Create(input);

        // Assert
        texture.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw LearningSpaceDataException for invalid texture formats.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("Outdoor_Wall_T15_Ambient_occlusionOutdoor_Wall_T15_Ambient_occlusion.png")]
    [InlineData("Outdoor_Wall_T14_Height.pngOutdoor_Wall_T14_Height.png")]
    public void Create_WithInvalidTextureFormats_ThrowsException(string input)
    {
        // Act
        var act = () => LearningSpaceTexture.Create(input);

        // Assert
        act.Should().Throw<LearningSpaceDataException>(because: "invalid texture formats should trigger domain validation");
    }
}