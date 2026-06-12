using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Value Object representing a color in hexadecimal format used for building rendering.
/// Supports both short (#RGB) and full (#RRGGBB) formats.
/// </summary>
public partial class Color : ValueObject
{
    /// <summary>
    /// The internal string value of the color in hexadecimal format.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private constructor that assigns the validated color value.
    /// </summary>
    /// <param name="value">A valid hexadecimal color string.</param>
    private Color(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="Color"/> instance from the given string.
    /// </summary>
    /// <param name="value">The input string to validate and convert.</param>
    /// <param name="Value">The resulting <see cref="Color"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(string value, out Color? Value)
    {
        Value = null;

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        // Validates hexadecimal color format: #RGB or #RRGGBB
        if (!ColorRegex().IsMatch(value))
        {
            return false;
        }

        if (value.Length > 7)
        {
            return false;
        }

        Value = new Color(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="Color"/> instance from the given string.
    /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input string to validate and convert.</param>
    /// <returns>A valid <see cref="Color"/> instance.</returns>
    /// <exception cref="BuildingDataException">
    /// Thrown when the input is null, does not match the expected format, or exceeds 7 characters.
    /// </exception>
    public static Color Create(string input)
    {
        var result = Color.TryCreate(input, out var Value);
        if (!result || Value is null)
        {
            throw new BuildingDataException(string.Format("Color {0} is invalid, it must be less than or " +
                "equal to 7 characters in hexadecimal.", input));
        }

        return Value;
    }

    /// <summary>
    /// Provides the components used to compare equality between value objects.
    /// </summary>
    /// <returns>An enumerable containing the color value.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Regular expression used to validate hexadecimal color strings.
    /// Accepts formats #RGB and #RRGGBB.
    /// </summary>
    /// <returns>A compiled regex for color validation.</returns>
    [GeneratedRegex(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex ColorRegex();
}
