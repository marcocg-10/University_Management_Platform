using System.Numerics;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Represents an oriented bounding box (OBB) in 3D space, defined by a center point, half-extents, and a rotation
/// matrix.
/// </summary>
/// <remarks>An oriented bounding box is a rectangular box that can be rotated arbitrarily in 3D space. It
/// is commonly used in collision detection, spatial queries, and other geometric computations. The box is defined
/// by its center point, dimensions (half-extents), and orientation (rotation matrix).</remarks>
public class OrientedBoundingBox
{
    /// <summary>
    /// Gets the center point of the object in 3D space.
    /// </summary>
    public Vector3 Center { get; }
    /// <summary>
    /// Gets the half extents of the bounding box, representing half the dimensions of the box along each axis.
    /// </summary>
    public Vector3 HalfExtents { get; }
    /// <summary>
    /// Gets the rotation matrix representing the orientation of the object.
    /// </summary>
    public Matrix4x4 Rotation { get; }

    /// <summary>
    /// Gets the X-axis of the rotation matrix as a <see cref="Vector3"/>.
    /// </summary>
    public Vector3 AxisX => new Vector3(Rotation.M11, Rotation.M12, Rotation.M13);
    /// <summary>
    /// Gets the Y-axis of the rotation matrix as a <see cref="Vector3"/>.
    /// </summary>
    public Vector3 AxisY => new Vector3(Rotation.M21, Rotation.M22, Rotation.M23);
    /// <summary>
    /// Gets the Z-axis of the rotation matrix as a <see cref="Vector3"/>.
    /// </summary>
    public Vector3 AxisZ => new Vector3(Rotation.M31, Rotation.M32, Rotation.M33);

    /// <summary>
    /// Initializes a new instance of the <see cref="OrientedBoundingBox"/> class with the specified center,
    /// half-extents, and rotation.
    /// </summary>
    /// <param name="center">The center point of the oriented bounding box in 3D space.</param>
    /// <param name="halfExtents">The half-lengths of the box along each axis, representing its size in each dimension.</param>
    /// <param name="rotation">The rotation matrix that defines the orientation of the box in 3D space.</param>
    public OrientedBoundingBox(Vector3 center, Vector3 halfExtents, Matrix4x4 rotation)
    {
        Center = center;
        HalfExtents = halfExtents;
        Rotation = rotation;
    }
}