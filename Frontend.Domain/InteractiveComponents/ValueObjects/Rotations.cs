
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Exceptions;


namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents a set of rotations (x, y, z) in a validated value object.
/// Ensures each rotation is a valid finite number within -360° to 360°, positive or negative, and within allowed bounds.
/// </summary>
public class Rotations : ValueObject
{
    /// <summary>
    /// Gets the X rotation.
    /// 
    /// X-axis rotation (Pitch):
    /// Represents rotation around the object's local X-axis. 
    /// This corresponds to tilting the object up or down.
    /// </summary>
    public double XAxisRotation { get; }

    /// <summary>
    /// Gets the Y rotation.
    /// 
    /// Y-axis rotation (Yaw):
    /// Represents rotation around the object's local Y-axis. 
    /// This corresponds to turning the object left or right.
    /// </summary>
    public double YAxisRotation { get; }

    /// <summary>
    /// Gets the Z rotation.
    /// 
    /// Z-axis rotation (Roll):
    /// Represents rotation around the object's local Z-axis. 
    /// This corresponds to rolling or banking the object.
    /// </summary>
    public double ZAxisRotation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotations"/> class with the specified rotation angles for
    /// the X, Y and Z axes.
    /// </summary>
    /// <remarks>The constructor validates the provided rotation angles to ensure they meet the required
    /// constraints.</remarks>
    /// <param name="x_axis">The rotation angle, in degrees, around the X-axis.</param>
    /// <param name="y_axis">The rotation angle, in degrees, around the Y-axis.</param>
    /// <param name="z_axis">The rotation angle, in degrees, around the Z-axis.</param>
    public Rotations(double x_axis, double y_axis, double z_axis)
    {
        ValidateRotations(x_axis, y_axis, z_axis);

        // Normalize rotations to be within -360 to 360 degrees.
        XAxisRotation = NormalizeAngle(x_axis);
        YAxisRotation = NormalizeAngle(y_axis);
        ZAxisRotation = NormalizeAngle(z_axis);
    }

    /// <summary>
    /// Returns the components of this value object for equality comparisons.
    /// </summary>
    /// <returns>An enumerable of equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return XAxisRotation;
        yield return YAxisRotation;
        yield return ZAxisRotation;
    }

    /// <summary>
    /// Validates the rotation values for the X, Y, and Z axes to ensure they are within the valid range and are finite
    /// numbers.
    /// </summary>
    /// <param name="x_axis">The rotation value for the X axis, in degrees. Must be a finite number within the range -360° to 360°.</param>
    /// <param name="y_axis">The rotation value for the Y axis, in degrees. Must be a finite number within the range -360° to 360°.</param>
    /// <param name="z_axis">The rotation value for the Z axis, in degrees. Must be a finite number within the range -360° to 360°.</param>
    /// <exception cref="InvalidRotationsException">Thrown if any of the rotation values are not finite numbers or fall outside the valid range of -360° to 360°.</exception>
    private static void ValidateRotations(double x_axis, double y_axis, double z_axis)
    {
        if (double.IsNaN(x_axis) || double.IsInfinity(x_axis))
            throw new InvalidRotationsException("X axis rotation must be a valid finite number.");

        if (double.IsNaN(y_axis) || double.IsInfinity(y_axis))
            throw new InvalidRotationsException("Y axis rotation must be a valid finite number.");

        if (double.IsNaN(z_axis) || double.IsInfinity(z_axis))
            throw new InvalidRotationsException("Z axis rotation must be a valid finite number.");

        const double minRotation = -360.0;
        const double maxRotation = 360.0;

        if (x_axis < minRotation || x_axis > maxRotation)
            throw new InvalidRotationsException(
                $"X axis rotation is out of valid range ({minRotation}° to {maxRotation}°).");

        if (y_axis < minRotation || y_axis > maxRotation)
            throw new InvalidRotationsException(
                $"Y axis rotation is out of valid range ({minRotation}° to {maxRotation}°).");

        if (z_axis < minRotation || z_axis > maxRotation)
            throw new InvalidRotationsException(
                $"Z axis rotation is out of valid range ({minRotation}° to {maxRotation}°).");
    }

    /// <summary>
    /// Normalizes the specified angle to the range [-360, 360].
    /// </summary>
    /// <param name="angle">The angle, in degrees, to normalize.</param>
    /// <returns>The normalized angle, constrained to the range [-360, 360].</returns>
    private static double NormalizeAngle(double angle)
    {
        // Modulo operation to wrap the angle within 360 degrees.
        angle %= 360.0;
        // Adjust to ensure the angle is within the desired range.
        if (angle > 360.0)
            angle -= 360.0;
        else if (angle < -360.0)
            angle += 360.0;

        return angle;
    }
}