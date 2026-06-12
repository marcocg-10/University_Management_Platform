using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents a validated identifier for a plate in the theme park.
/// Must match a specific format (exactly 6 digits).
/// </summary>
public partial class PlateId : ValueObject
{
    /// <summary>
    /// Gets the normalized plate identifier value.
    /// Stored in uppercase invariant form.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlateId"/> class.
    /// Validates and normalizes the provided value.
    /// </summary>
    /// <param name="value">The plate ID string to validate.</param>
    /// <exception cref="InvalidPlateIdException">
    /// Thrown if the value is null, empty, whitespace, or does not match the expected format.
    /// </exception>
    public PlateId(string value)
    {
        ValidatePlateId(value);
        Value = value.ToUpperInvariant();
    }

    /// <summary>
    /// Returns the string representation of the plate ID.
    /// </summary>
    /// <returns>The normalized plate ID string.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Returns the components of this value object for equality comparisons.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Validates the format of the plate ID.
    /// </summary>
    /// <param name="value">The plate ID value to validate.</param>
    /// <exception cref="InvalidPlateIdException">
    /// Thrown if the value is null, empty, whitespace, or does not match the expected format.
    /// </exception>
    private static void ValidatePlateId(string value)
    {
        if (value is null)
            throw new InvalidPlateIdException("Plate ID cannot be null.");

        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPlateIdException("Plate ID cannot be empty or whitespace.");

        if (!PlateIdRegex().IsMatch(value))
            throw new InvalidPlateIdException(
                $"Plate ID format is invalid. Expected format: 6 digits (e.g., '123456'). Given: '{value}'."
            );
    }

    /// <summary>
    /// Returns the compiled regular expression for validating a plate ID.
    /// </summary>
    /// <returns>A <see cref="Regex"/> that matches exactly six digits.</returns>
    [GeneratedRegex(@"^\d{6}$", RegexOptions.IgnoreCase, "en-us")]
    private static partial Regex PlateIdRegex();
}
