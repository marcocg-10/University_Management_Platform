using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Value Object representing a texture filename used for building rendering.
/// Must be a non-empty string ending in ".png" and no longer than 50 characters.
/// </summary>
public partial class BuildingTexture : ValueObject
{
    /// <summary>
    /// The internal string value of the texture filename.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private constructor that assigns the validated texture value.
    /// </summary>
    /// <param name="value">A valid texture string.</param>
    private BuildingTexture(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="BuildingTexture"/> instance from the given string.
    /// </summary>
    /// <param name="value">The input string to validate and convert.</param>
    /// <param name="Value">The resulting <see cref="BuildingTexture"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(string value, out BuildingTexture? Value)
    {
        Value = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (value.Length > 50)
        {
            return false;
        }

        if (!TextureRegex().IsMatch(value))
        {
            return false;
        }

        Value = new BuildingTexture(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="BuildingTexture"/> instance from the given string.
    /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate and convert.</param>
    /// <returns>A valid <see cref="BuildingTexture"/> instance.</returns>
    /// <exception cref="BuildingDataException">
    /// Thrown when the input is null, empty, exceeds 50 characters, or does not end in ".png".
    /// </exception>
    public static BuildingTexture Create(string input)
    {
        var result = TryCreate(input, out var Value);
        if (!result || Value is null)
        {
            throw new BuildingDataException(
                $"Texture '{input}' is invalid. It must be a non-empty string, less than or equal to 50 characters, and end with '.png'.");
        }

        return Value;
    }

    /// <summary>
    /// Provides the components used to compare equality between value objects.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Regular expression used to validate texture filenames.
    /// Must end in ".png".
    /// </summary>
    [GeneratedRegex(@"^.+\.png$", RegexOptions.None, "en-US")]
    private static partial Regex TextureRegex();
}
