using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Value Object representing the Z coordinate of a building element in the rendering system.
/// Ensures the coordinate is positive, non-zero, and does not exceed 2,000,000.
/// </summary>
public partial class Z : ValueObject
{
    /// <summary>
    /// The internal decimal value representing the Z coordinate.
    /// </summary>
    public decimal ZValue { get; }

    /// <summary>
    /// Private constructor that assigns the validated Z coordinate value.
    /// </summary>
    /// <param name="value">A valid Z coordinate value.</param>
    private Z(decimal value)
    {
        ZValue = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="Z"/> instance from the given decimal value.
    /// </summary>
    /// <param name="value">The input decimal value to validate and convert.</param>
    /// <param name="ZValue">The resulting <see cref="Z"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(decimal value, out Z? ZValue)
    {
        ZValue = null;

        // Reject values greater than 2,000,000
        if (value > 2000000)
        {
            return false;
        }
        // Reject values less than 2,000,000
        if (value < -2000000)
        {
            return false;
        }

        ZValue = new Z(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="Z"/> instance from the given decimal value.
    /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input decimal value to validate and convert.</param>
    /// <returns>A valid <see cref="Z"/> instance.</returns>
    /// <exception cref="BuildingDataException">
    /// Thrown when the input is zero, negative, or exceeds the maximum allowed Z coordinate.
    /// </exception>
    public static Z Create(decimal input)
    {
        var result = Z.TryCreate(input, out var ZValue);
        if (!result || ZValue is null)
        {
            throw new BuildingDataException(string.Format("Z Coordinate {0} is invalid", input));
        }

        return ZValue;
    }

    /// <summary>
    /// Provides the components used to compare equality between value objects.
    /// </summary>
    /// <returns>An enumerable containing the Z coordinate value.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ZValue;
    }
}
