using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

/// <summary>
/// Value Object representing the depth of a building element in the rendering system.
/// Ensures the depth is positive, non-zero, and does not exceed 2,000,000.
/// </summary>
public partial class Depth : ValueObject
{
    /// <summary>
    /// The internal decimal value representing the depth.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Private constructor that assigns the validated depth value.
    /// </summary>
    /// <param name="value">A valid depth value.</param>
    private Depth(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="Depth"/> instance from the given decimal value.
    /// </summary>
    /// <param name="value">The input decimal value to validate and convert.</param>
    /// <param name="Value">The resulting <see cref="Depth"/> instance if valid; otherwise null.</param>
    /// <returns>True if the input is valid and an instance is created; false otherwise.</returns>
    public static bool TryCreate(decimal value, out Depth? Value)
    {
        Value = null;

        // Reject values greater than 2,000,000
        if (value > 2000000)
        {
            return false;
        }

        // Reject values less than 0
        if (value <= 0)
        {
            return false;
        }

        Value = new Depth(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="Depth"/> instance from the given decimal value.
    /// Throws a <see cref="BuildingDataException"/> if the input is invalid.
    /// </summary>
    /// <param name="input">The input decimal value to validate and convert.</param>
    /// <returns>A valid <see cref="Depth"/> instance.</returns>
    /// <exception cref="BuildingDataException">
    /// Thrown when the input is zero, negative, or exceeds the maximum allowed depth.
    /// </exception>
    public static Depth Create(decimal input)
    {
        var result = Depth.TryCreate(input, out var Value);
        if (!result || Value is null)
        {
            throw new BuildingDataException(string.Format("Depth {0} is invalid", input));
        }

        return Value;
    }

    /// <summary>
    /// Provides the components used to compare equality between value objects.
    /// </summary>
    /// <returns>An enumerable containing the depth value.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
