using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents a set of pixel dimensions (width, height) in a validated value object.
/// Ensures each dimension is a valid finite number, positive, and within allowed bounds.
/// Note: This type of dimensions should not be confused with the Dimensions of a Interactive Component,
/// in a learning space.
/// </summary>

public class Resolution : ValueObject
{
    /// <summary>
    /// Gets the width (pixels) dimension.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height (pixels) dimension.
    /// </summary> 
    public int Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Resolution"/> class
    /// with specified width, height.
    /// </summary>
    /// <param name="width">The width value.</param>
    /// <param name="height">The height value.</param>
    /// <exception cref="InvalidResolutionException">
    /// Thrown if any Resolution is not positive, not integer, or exceeds the maximum allowed value.
    /// </exception>
    public Resolution(int width, int height)
    {
        ValidateResolution(width, height);
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Returns the components of this value object for equality comparisons.
    /// </summary>
    /// <returns>An enumerable of equality components</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
    }


    /// <summary>
    /// Validates the specified resolution dimensions to ensure they fall within acceptable bounds.
    /// </summary>
    /// <param name="width">The width of the resolution, in pixels. Must be greater than zero and not exceed 15,360 pixels.</param>
    /// <param name="height">The height of the resolution, in pixels. Must be greater than zero and not exceed 8,640 pixels.</param>
    /// <exception cref="InvalidResolutionException">Thrown if <paramref name="width"/> is less than or equal to zero, exceeds 15,360 pixels, or if 
    /// <paramref name="height"/> is less than or equal to zero, or exceeds 8,640 pixels.</exception>
    private static void ValidateResolution(int width, int height)
    {
        if (width <= 0)
            throw new InvalidResolutionException($"Resolution width must be greater than zero.");

        if (height <= 0)
            throw new InvalidResolutionException("Resolution height must be greater than zero.");

        const int maxWidth = 15360;

        if (width > maxWidth)
            throw new InvalidResolutionException(
                $"Resolution width exceeds maximum allowed value ({maxWidth} pixels)."
            );

        const int maxHeight = 8640;

        if (height > maxHeight)
            throw new InvalidResolutionException(
                $"Resolution height exceeds maximum allowed value ({maxHeight} pixels)."
            );
    }
}
