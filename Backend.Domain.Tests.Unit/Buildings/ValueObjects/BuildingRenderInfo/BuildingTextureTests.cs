using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Unit tests for the <see cref="BuildingTexture"/> value object.
/// Validates strict .png enforcement (lowercase), filename length boundaries, and exception handling.
/// </summary>
public class BuildingTextureTests
{
    /// <summary>
    /// Should return true for valid .png texture filenames within length limits.
    /// </summary>
    [Theory]
    [InlineData("brick_texture.png")]
    [InlineData("wall01.png")]
    [InlineData("ambient_occlusion_01.png")]
    public void TryCreate_WithValidPngTextureNames_ReturnsTrue(string input)
    {
        var result = BuildingTexture.TryCreate(input, out var _);
        result.Should().BeTrue(because: "valid .png texture names should pass validation");
    }

    /// <summary>
    /// Should return false for invalid texture names (wrong extension, null, empty, or too long).
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("texture.jpg")]
    [InlineData("texture")]
    [InlineData("wall01.PNG")]
    [InlineData("ambient_occlusion_01.PnG")]
    [InlineData("this_texture_name_is_way_too_long_to_be_valid_because_it_exceeds_fifty_characters.png")]
    public void TryCreate_WithInvalidTextureNames_ReturnsFalse(string input)
    {
        var result = BuildingTexture.TryCreate(input, out var _);
        result.Should().BeFalse(because: "invalid texture names should fail validation");
    }

    /// <summary>
    /// Should produce a non-null instance for valid .png texture names.
    /// </summary>
    [Theory]
    [InlineData("brick_texture.png")]
    [InlineData("wall01.png")]
    public void TryCreate_WithValidTextureNames_ReturnsNonNullInstance(string input)
    {
        BuildingTexture.TryCreate(input, out var texture);
        (texture is not null).Should().BeTrue(because: "valid .png texture names should produce a non-null instance");
    }

    /// <summary>
    /// Should match the input value when created successfully.
    /// </summary>
    [Theory]
    [InlineData("brick_texture.png")]
    [InlineData("wall01.png")]
    public void TryCreate_WithValidTextureNames_SetsCorrectValue(string input)
    {
        BuildingTexture.TryCreate(input, out var texture);
        texture!.Value.Should().Be(input, because: "the internal value should match the input");
    }

    /// <summary>
    /// Should return a valid instance from Create with valid .png input.
    /// </summary>
    [Theory]
    [InlineData("brick_texture.png")]
    [InlineData("wall01.png")]
    public void Create_WithValidTextureNames_ReturnsInstance(string input)
    {
        var texture = BuildingTexture.Create(input);
        texture.Value.Should().Be(input, because: "Create should preserve the input value");
    }

    /// <summary>
    /// Should throw BuildingDataException for invalid texture names.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("texture.jpg")]
    [InlineData("texture")]
    [InlineData("wall01.PNG")]
    [InlineData("ambient_occlusion_01.PnG")]
    [InlineData("this_texture_name_is_way_too_long_to_be_valid_because_it_exceeds_fifty_characters.png")]
    public void Create_WithInvalidTextureNames_ThrowsException(string input)
    {
        var act = () => BuildingTexture.Create(input);
        act.Should().Throw<BuildingDataException>(because: "invalid texture names should trigger domain validation");
    }
}
