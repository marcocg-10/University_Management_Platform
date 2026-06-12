using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents a set of 3D coordinates in a validated value object.
/// Ensures that each coordinate is a valid, and finite number within allowed bounds.
/// </summary>
public class Coordinates : ValueObject
{
    /// <summary>
    /// Gets the X coordinate value.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y coordinate value.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the Z coordinate value.
    /// </summary>
    public double Z { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinates"/> class
    /// with specified X, Y, and Z values.
    /// </summary>
    /// <param name="x">The X coordinate value.</param>
    /// <param name="y">The Y coordinate value.</param>
    /// <param name="z">The Z coordinate value.</param>
    /// <exception cref="InvalidCoordinatesException">
    /// Thrown if any coordinate is NaN, infinite, or exceeds the maximum allowed value.
    /// </exception>
    public Coordinates(double x, double y, double z)
    {
        ValidateCoordinates(x, y, z);

        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Returns the components of this value object for equality comparisons.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }

    /// <summary>
    /// Validates the provided coordinates.
    /// </summary>
    /// <param name="x">X coordinate value.</param>
    /// <param name="y">Y coordinate value.</param>
    /// <param name="z">Z coordinate value.</param>
    /// <exception cref="InvalidCoordinatesException">
    /// Thrown if any coordinate is NaN, infinite, or exceeds the maximum allowed value.
    /// </exception>
    private static void ValidateCoordinates(double x, double y, double z)
    {
        if (double.IsNaN(x) || double.IsInfinity(x))
            throw new InvalidCoordinatesException("X coordinate must be a valid finite number.");

        if (double.IsNaN(y) || double.IsInfinity(y))
            throw new InvalidCoordinatesException("Y coordinate must be a valid finite number.");

        if (double.IsNaN(z) || double.IsInfinity(z))
            throw new InvalidCoordinatesException("Z coordinate must be a valid finite number.");

        const double maxCoordinate = 10000.0;
        const double minCoordinate = -10000.0;

        if (x > maxCoordinate || x < minCoordinate)
            throw new InvalidCoordinatesException(
                $"X coordinate is out of valid range [{minCoordinate}, {maxCoordinate}] meters."
            );

        if (y > maxCoordinate || y < minCoordinate)
            throw new InvalidCoordinatesException(
                $"Y coordinate is out of valid range [{minCoordinate}, {maxCoordinate}] meters."
            );

        if (z > maxCoordinate || z < minCoordinate)
            throw new InvalidCoordinatesException(
                $"Z coordinate is out of valid range [{minCoordinate}, {maxCoordinate}] meters."
            );
    }
}
