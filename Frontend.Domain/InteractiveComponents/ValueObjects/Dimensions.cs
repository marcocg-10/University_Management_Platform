using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents a set of dimensions (width, height, depth) in a validated value object.
/// Ensures each dimension is a valid finite number, positive, and within allowed bounds.
/// </summary>
public class Dimensions : ValueObject
{
    /// <summary>
    /// Gets the width dimension.
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Gets the height dimension.
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Gets the depth dimension.
    /// </summary>
    public double Depth { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dimensions"/> class
    /// with specified width, height, and optional depth.
    /// </summary>
    /// <param name="width">The width value.</param>
    /// <param name="height">The height value.</param>
    /// <param name="depth">The depth value.</param>
    /// <exception cref="InvalidDimensionsException">
    /// Thrown if any dimension is NaN, infinite, not positive, or exceeds the maximum allowed value.
    /// </exception>
    public Dimensions(double width, double height, double depth)
    {
        ValidateDimensions(width, height, depth);

        Width = width;
        Height = height;
        Depth = depth;
    }

    /// <summary>
    /// Returns the components of this value object for equality comparisons.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
        yield return Depth;
    }

    /// <summary>
    /// Validates the provided dimensions.
    /// </summary>
    /// <param name="width">Width value.</param>
    /// <param name="height">Height value.</param>
    /// <param name="depth">Depth value.</param>
    /// <exception cref="InvalidDimensionsException">
    /// Thrown if any dimension is NaN, infinite, not positive, or exceeds the maximum allowed value.
    /// </exception>
    private static void ValidateDimensions(double width, double height, double depth)
    {
        if (double.IsNaN(width) || double.IsInfinity(width))
            throw new InvalidDimensionsException("Width must be a valid finite number.");

        if (double.IsNaN(height) || double.IsInfinity(height))
            throw new InvalidDimensionsException("Height must be a valid finite number.");

        if (double.IsNaN(depth) || double.IsInfinity(depth))
            throw new InvalidDimensionsException("Depth must be a valid finite number.");

        if (width <= 0)
            throw new InvalidDimensionsException("Width must be greater than zero.");

        if (height <= 0)
            throw new InvalidDimensionsException("Height must be greater than zero.");

        if (depth <= 0)
            throw new InvalidDimensionsException("Depth must be greater than zero.");

        const double maxDimension = 1000.0;

        if (width > maxDimension)
            throw new InvalidDimensionsException(
                $"Width exceeds maximum allowed value ({maxDimension} meters)."
            );

        if (height > maxDimension)
            throw new InvalidDimensionsException(
                $"Height exceeds maximum allowed value ({maxDimension} meters)."
            );

        if (depth > maxDimension)
            throw new InvalidDimensionsException(
                $"Depth exceeds maximum allowed value ({maxDimension} meters)."
            );
    }
}
