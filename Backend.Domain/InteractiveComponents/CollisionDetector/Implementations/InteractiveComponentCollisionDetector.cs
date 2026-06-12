namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector.Implementations;

using System.Numerics;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

/// <summary>
/// Interactive component collision detector implementation.
/// </summary>
internal class InteractiveComponentCollisionDetector : IInteractiveComponentCollisionDetector
{
    /// <summary>
    /// Detect if component has a collision with every interactive component placed in the same learning space.
    /// </summary>
    /// <param name="actualInteractiveComponent"> Actual interactive component to check for collisions </param>
    /// <param name="placedInteractiveComponents"> A list of every interactive component in the learning space </param>
    /// <returns> True if a collision is detected; otherwise, false. </returns>
    public bool DetectCollision(InteractiveComponent actualInteractiveComponent, IEnumerable<InteractiveComponent>  placedInteractiveComponents)
    {
        if (actualInteractiveComponent == null || placedInteractiveComponents == null)
            throw new InteractiveComponentNotFoundException("Interactive component could not be found.");

        return placedInteractiveComponents.Any(placedComponent => HasCollision(actualInteractiveComponent, placedComponent));
    }

    /// <summary>
    /// Determines whether two axis-aligned bounding boxes (AABBs) collide in 3D space.
    /// </summary>
    /// <remarks>This method calculates the minimum and maximum bounds of each component's bounding box based
    /// on their dimensions and coordinates, and checks for overlap along the X, Y, and Z axes. It assumes that the
    /// components are represented as axis-aligned bounding boxes.</remarks>
    /// <param name="componentA">The first <see cref="InteractiveComponent"/> to check for collision.</param>
    /// <param name="componentB">The second <see cref="InteractiveComponent"/> to check for collision.</param>
    /// <returns><see langword="true"/> if the two components' bounding boxes overlap along all three axes; otherwise, <see
    /// langword="false"/>.</returns>
    private bool CheckAABBCollision(InteractiveComponent componentA, InteractiveComponent componentB)
    {
        // Get half sizes for easier math
        double halfWidthA = componentA.Dimensions.Width / 2f;
        double halfHeightA = componentA.Dimensions.Height / 2f;
        double halfDepthA = componentA.Dimensions.Depth / 2f;

        double halfWidthB = componentB.Dimensions.Width / 2f;
        double halfHeightB = componentB.Dimensions.Height / 2f;
        double halfDepthB = componentB.Dimensions.Depth / 2f;

        // Calculate min and max bounds for each component
        double aMinX = componentA.Coordinates.X - halfWidthA;
        double aMaxX = componentA.Coordinates.X + halfWidthA;
        double aMinY = componentA.Coordinates.Y - halfHeightA;
        double aMaxY = componentA.Coordinates.Y + halfHeightA;
        double aMinZ = componentA.Coordinates.Z - halfDepthA;
        double aMaxZ = componentA.Coordinates.Z + halfDepthA;

        double bMinX = componentB.Coordinates.X - halfWidthB;
        double bMaxX = componentB.Coordinates.X + halfWidthB;
        double bMinY = componentB.Coordinates.Y - halfHeightB;
        double bMaxY = componentB.Coordinates.Y + halfHeightB;
        double bMinZ = componentB.Coordinates.Z - halfDepthB;
        double bMaxZ = componentB.Coordinates.Z + halfDepthB;

        // Check for overlap along each axis
        bool overlapX = aMinX < bMaxX && aMaxX > bMinX;
        bool overlapY = aMinY < bMaxY && aMaxY > bMinY;
        bool overlapZ = aMinZ < bMaxZ && aMaxZ > bMinZ;

        return overlapX && overlapY && overlapZ;
    }

    /// <summary>
    /// Determines whether two interactive components are colliding using Oriented Bounding Box (OBB) collision
    /// detection.
    /// </summary>
    /// <remarks>This method uses the Separating Axis Theorem (SAT) to evaluate collision between the OBBs of
    /// the specified components.</remarks>
    /// <param name="componentA">The first interactive component to check for collision.</param>
    /// <param name="componentB">The second interactive component to check for collision.</param>
    /// <returns><see langword="true"/> if the two components are colliding; otherwise, <see langword="false"/>.</returns>
    private bool CheckOBBCollision(InteractiveComponent componentA, InteractiveComponent componentB)
    {
        var obbA = CreateOBB(componentA);
        var obbB = CreateOBB(componentB);

        // Use Separating Axis Theorem (SAT) for OBB collision detection.
        return CheckSATCollision(obbA, obbB);
    }

    /// <summary>
    /// Determines whether two oriented bounding boxes (OBBs) are colliding using the Separating Axis Theorem (SAT).
    /// </summary>
    /// <remarks>This method performs collision detection between two oriented bounding boxes using the SAT
    /// algorithm. It tests for potential separation along 15 axes: the three axes of each bounding box and the nine
    /// cross-product axes. If a separating axis is found, the method returns <see langword="false"/>; otherwise, it
    /// returns <see langword="true"/>.</remarks>
    /// <param name="a">The first oriented bounding box to test for collision.</param>
    /// <param name="b">The second oriented bounding box to test for collision.</param>
    /// <returns><see langword="true"/> if the two oriented bounding boxes are colliding; otherwise, <see langword="false"/>.</returns>
    private bool CheckSATCollision(OrientedBoundingBox a, OrientedBoundingBox b)
    {
        // SAT for OBB collision detection using 15 axis test (3 from A, 3 from B, and 9 cross products)

        // Separation vector
        var separation = b.Center - a.Center;

        // Test axes from A's rotation matrix
        if (!TestAxis(a.AxisX, a, b, separation)) return false;
        if (!TestAxis(a.AxisY, a, b, separation)) return false;
        if (!TestAxis(a.AxisZ, a, b, separation)) return false;

        // Test axes from B's rotation matrix
        if (!TestAxis(b.AxisX, a, b, separation)) return false;
        if (!TestAxis(b.AxisY, a, b, separation)) return false;
        if (!TestAxis(b.AxisZ, a, b, separation)) return false;

        // Test cross products of axes
        if (!TestCrossAxis(a.AxisX, b.AxisX, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisX, b.AxisY, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisX, b.AxisZ, a, b, separation)) return false;

        if (!TestCrossAxis(a.AxisY, b.AxisX, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisY, b.AxisY, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisY, b.AxisZ, a, b, separation)) return false;

        if (!TestCrossAxis(a.AxisZ, b.AxisX, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisZ, b.AxisY, a, b, separation)) return false;
        if (!TestCrossAxis(a.AxisZ, b.AxisZ, a, b, separation)) return false;

        return true;

    }

    /// <summary>
    /// Tests whether two oriented bounding boxes (OBBs) are separated along a given axis.
    /// </summary>
    /// <remarks>This method determines whether the two OBBs overlap when projected onto the specified axis.
    /// If the axis is a zero vector (or near zero), it is treated as degenerate, and the method assumes no separation
    /// along that axis.</remarks>
    /// <param name="axis">The axis to test for separation. Must not be a zero vector.</param>
    /// <param name="a">The first oriented bounding box.</param>
    /// <param name="b">The second oriented bounding box.</param>
    /// <param name="separation">The vector representing the separation between the centers of the two OBBs.</param>
    /// <returns><see langword="true"/> if the two OBBs are not separated along the specified axis; otherwise, <see
    /// langword="false"/>.</returns>
    private bool TestAxis(Vector3 axis, OrientedBoundingBox a, OrientedBoundingBox b, Vector3 separation)
    {
        // Check for zero-length axis to avoid NaN from normalization
        //  If "axis.Length Squared() == 0" implies that the axis vector is the zero vector.
        const float EPSILON = 1e-6f;
        if (axis.LengthSquared() < EPSILON)
        {
            // Degenerate axis, treat as no separation
            return true;
        }
        // Normalize axis
        var normalizedAxis = Vector3.Normalize(axis);

        // Project both OBBs onto the normalized axis
        float projectionA = GetProjectionRadius(a, normalizedAxis);
        float projectionB = GetProjectionRadius(b, normalizedAxis);

        // Project separation vector
        float separationProjection = Math.Abs(Vector3.Dot(separation, normalizedAxis));

        // Check for separation
        return separationProjection <= (projectionA + projectionB);
    }

    /// <summary>
    /// Calculates the projection radius of an oriented bounding box (OBB) along a specified axis.
    /// </summary>
    /// <param name="obb">The oriented bounding box to calculate the projection radius for.</param>
    /// <param name="axis">The axis along which the projection radius is calculated.</param>
    /// <returns>The projection radius of the oriented bounding box along the specified axis.</returns>
    private float GetProjectionRadius(OrientedBoundingBox obb, Vector3 axis)
    {
        return Math.Abs(Vector3.Dot(obb.AxisX * obb.HalfExtents.X, axis)) +
               Math.Abs(Vector3.Dot(obb.AxisY * obb.HalfExtents.Y, axis)) +
               Math.Abs(Vector3.Dot(obb.AxisZ * obb.HalfExtents.Z, axis));
    }

    /// <summary>
    /// Tests whether two oriented bounding boxes overlap along the cross product of two specified axes.
    /// </summary>
    /// <remarks>This method calculates the cross product of the specified axes and uses it as a potential
    /// separating axis to determine whether the two bounding boxes overlap. If the cross axis is near zero (indicating
    /// near-parallel axes), the method assumes overlap along this axis. Otherwise, the method projects the bounding
    /// boxes onto the cross axis and checks whether their projections overlap.</remarks>
    /// <param name="axisA">The first axis to use in the cross product calculation.</param>
    /// <param name="axisB">The second axis to use in the cross product calculation.</param>
    /// <param name="a">The first oriented bounding box to test.</param>
    /// <param name="b">The second oriented bounding box to test.</param>
    /// <param name="separation">The vector representing the separation between the centers of the two bounding boxes.</param>
    /// <returns><see langword="true"/> if the bounding boxes overlap along the cross axis; otherwise, <see
    /// langword="false"/>.</returns>
    private bool TestCrossAxis(Vector3 axisA, Vector3 axisB, OrientedBoundingBox a, OrientedBoundingBox b, Vector3 separation)
    {
        var crossAxis = Vector3.Cross(axisA, axisB);

        // Handle near-parallel axes
        if (crossAxis.LengthSquared() < 0.0001f)
            return true;

        crossAxis = Vector3.Normalize(crossAxis);

        float projectionA = GetProjectionRadius(a, crossAxis);
        float projectionB = GetProjectionRadius(b, crossAxis);
        float separationProjection = Math.Abs(Vector3.Dot(separation, crossAxis));

        return separationProjection <= (projectionA + projectionB);
    }

    /// <summary>
    /// Creates an oriented bounding box (OBB) for the specified interactive component.
    /// </summary>
    /// <remarks>The method calculates the center, half-extents, and rotation matrix of the component based on
    /// its properties and constructs an OBB using these values. The resulting OBB can be used for collision detection,
    /// spatial queries, or other geometric operations.</remarks>
    /// <param name="component">The interactive component for which the OBB is created. The component must have valid coordinates, dimensions,
    /// and rotations.</param>
    /// <returns>An <see cref="OrientedBoundingBox"/> representing the spatial bounds of the component, including its position,
    /// size, and orientation.</returns>
    private OrientedBoundingBox CreateOBB(InteractiveComponent component)
    {
        var center = new Vector3(
            (float)component.Coordinates.X,
            (float)component.Coordinates.Y,
            (float)component.Coordinates.Z
         );

        var halfExtents = new Vector3(
            (float)component.Dimensions.Width / 2f,
            (float)component.Dimensions.Height / 2f,
            (float)component.Dimensions.Depth / 2f
         );

        // Create rotation matrix from Rotations VO
        var rotation = CreateRotationMatrix(component.Rotations);

        return new OrientedBoundingBox(center, halfExtents, rotation);
    }

    /// <summary>
    /// Creates a 4x4 rotation matrix based on the specified rotations around the X, Y, and Z axes.
    /// </summary>
    /// <remarks>The rotation angles are applied in the order of X-axis, Y-axis, and then Z-axis. The angles
    /// are converted from degrees to radians before creating the rotation matrices.</remarks>
    /// <param name="rotations">An object containing the rotation angles, in degrees, for the X, Y, and Z axes.</param>
    /// <returns>A <see cref="Matrix4x4"/> representing the combined rotation. If all rotation angles are approximately zero, the
    /// identity matrix is returned.</returns>
    private Matrix4x4 CreateRotationMatrix(Rotations rotations)
    {
        // If there is no rotation, return identity matrix
        if (AreApproximatelyEqual(rotations.XAxisRotation, 0) 
            && AreApproximatelyEqual(rotations.YAxisRotation, 0) 
            && AreApproximatelyEqual(rotations.ZAxisRotation, 0))
            return Matrix4x4.Identity;

        // Convert degrees to radians and create rotation matrices for each axis
        return Matrix4x4.CreateRotationX((float)(rotations.XAxisRotation * Math.PI / 180.0)) *
               Matrix4x4.CreateRotationY((float)(rotations.YAxisRotation * Math.PI / 180.0)) *
               Matrix4x4.CreateRotationZ((float)(rotations.ZAxisRotation * Math.PI / 180.0));
    }

    /// <summary>
    /// Determines if two interactive components collide based on their AABB (Axis-Aligned Bounding Box).
    /// No rotation is considered in this simple implementation.
    /// Assuming coordinates (x,y,z) represent the center of the component.
    /// </summary>
    /// <param name="componentA"> First interactive component. </param>
    /// <param name="componentB"> Second interactive component. </param>
    /// <returns>True if the components collide; otherwise, false.</returns>
    public bool HasCollision(InteractiveComponent componentA, InteractiveComponent componentB)
    {
        if (componentA == null || componentB == null)
            throw new InteractiveComponentNotFoundException("Interactive component could not be found.");

        if (componentA.PlateId.Value == componentB.PlateId.Value)
            return false;

        if (HasNoRotation(componentA.Rotations) && HasNoRotation(componentB.Rotations))
            return CheckAABBCollision(componentA, componentB);
        
        return CheckOBBCollision(componentA, componentB);
    }

    /// <summary>
    /// Checks if the given Rotations object has no rotation (all axes are zero).
    /// </summary>
    /// <param name="rotations">The Rotations object to check.</param>
    /// <returns>True if all rotation axes are zero; otherwise, false.</returns>
    private static bool HasNoRotation(Rotations rotations)
    {
        return AreApproximatelyEqual(rotations.XAxisRotation, 0) &&
               AreApproximatelyEqual(rotations.YAxisRotation, 0) &&
               AreApproximatelyEqual(rotations.ZAxisRotation, 0);
    }

    /// <summary>
    /// Determines whether two double-precision floating-point numbers are approximately equal within a specified
    /// tolerance.
    /// </summary>
    /// <param name="a">The first double-precision floating-point number to compare.</param>
    /// <param name="b">The second double-precision floating-point number to compare.</param>
    /// <param name="epsilon">The maximum allowable difference between the two numbers for them to be considered approximately equal. Defaults
    /// to <c>1e-9</c>.</param>
    /// <returns><see langword="true"/> if the absolute difference between <paramref name="a"/> and <paramref name="b"/> is less
    /// than or equal to <paramref name="epsilon"/>; otherwise, <see langword="false"/>.</returns>
    private static bool AreApproximatelyEqual(double a, double b, double epsilon = 1e-9)
    {
        return Math.Abs(a - b) <= epsilon;
    }
}
