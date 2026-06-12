using System.Numerics;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector.Implementations;

/// <summary>
/// Provides functionality to determine whether an interactive component is fully contained within a specified learning
/// space.
/// </summary>
/// <remarks>This class implements the <see cref="IInteractiveComponentContainmentDetector"/> interface and
/// provides methods to evaluate the spatial relationship between interactive components and learning spaces. The
/// containment check is performed using oriented bounding boxes (OBBs) to account for the dimensions, positions, and
/// rotations of the objects.</remarks>
internal class InteractiveComponentContainmentDetector : IInteractiveComponentContainmentDetector
{
    /// <summary>
    /// Determines whether the specified interactive component is fully contained within the given learning space.
    /// </summary>
    /// <remarks>This method evaluates the spatial relationship between the interactive component and the
    /// learning space using oriented bounding boxes (OBBs). The containment check considers the full extent of the
    /// component relative to the boundaries of the learning space.</remarks>
    /// <param name="interactiveComponent">The interactive component to check for containment.</param>
    /// <param name="learningSpace">The learning space within which to check for containment.</param>
    /// <returns><see langword="true"/> if the interactive component is fully contained within the learning space; otherwise,
    /// <see langword="false"/>.</returns>
    public bool IsContained(InteractiveComponent interactiveComponent, LearningSpace learningSpace)
    {
        var learningSpaceOBB = CreateLearningSpaceOBB(learningSpace);
        var interactiveComponentOBB = CreateComponentOBB(interactiveComponent, learningSpace);

        return IsOBBContainedInOBB(interactiveComponentOBB, learningSpaceOBB);
    }

    /// <summary>
    /// Creates an oriented bounding box (OBB) for the specified learning space.
    /// </summary>
    /// <remarks>The resulting oriented bounding box uses an identity rotation matrix, as learning spaces
    /// currently do not support rotations. If rotation support is added in the future, the rotation matrix will be
    /// derived from the learning space's rotation properties.</remarks>
    /// <param name="learningSpace">The learning space for which the oriented bounding box is created. Contains the coordinates, dimensions, and
    /// other spatial properties of the learning space.</param>
    /// <returns>An <see cref="OrientedBoundingBox"/> representing the spatial boundaries of the learning space. The bounding
    /// box is centered at the learning space's coordinates, with dimensions derived from its width, height, and length.</returns>
    private OrientedBoundingBox CreateLearningSpaceOBB(LearningSpace learningSpace)
    {
        // Calculating center point.
        var center = new Vector3(
            learningSpace.Coordinates.XCoordinate,
            learningSpace.Coordinates.YCoordinate + (learningSpace.Dimensions.Height / 2f),
            learningSpace.Coordinates.ZCoordinate
        );

        // Half extents (half of each dimension).
        var halfExtents = new Vector3(
            learningSpace.Dimensions.Width / 2f,
            learningSpace.Dimensions.Height / 2f,
            learningSpace.Dimensions.Length / 2f
        );
        // Learning spaces currently do not have rotations, so we use identity matrix
        // When learning spaces get rotations, we'll use:
        // var rotation = CreateRotationMatrix(learningSpace.Rotations);
        var rotation = Matrix4x4.Identity;

        return new OrientedBoundingBox(center, halfExtents, rotation);
    }

    /// <summary>
    /// Creates an <see cref="OrientedBoundingBox"/> for the specified interactive component within the context of a
    /// learning space.
    /// </summary>
    /// <remarks>The method calculates the oriented bounding box by transforming the component's local
    /// coordinates, dimensions, and rotations into the global coordinate system defined by the learning space. The
    /// resulting bounding box accounts for both the component's and the learning space's rotations.</remarks>
    /// <param name="interactiveComponent">The interactive component for which the oriented bounding box is created. Contains the component's coordinates,
    /// dimensions, and rotations.</param>
    /// <param name="learningSpace">The learning space that defines the global coordinate system and context for the interactive component.</param>
    /// <returns>An <see cref="OrientedBoundingBox"/> representing the interactive component's position, size, and orientation in
    /// global coordinates.</returns>
    private OrientedBoundingBox CreateComponentOBB(
        InteractiveComponent interactiveComponent,
        LearningSpace learningSpace)
    {
        // Coordinates relative to a Learning Space.
        var localCenter = new Vector3(
            (float)interactiveComponent.Coordinates.X,
            (float)interactiveComponent.Coordinates.Y,
            (float)interactiveComponent.Coordinates.Z
        );

        // Half extents (half of each dimension).
        var halfExtents = new Vector3(
            (float)interactiveComponent.Dimensions.Width / 2f,
            (float)interactiveComponent.Dimensions.Height / 2f,
            (float)interactiveComponent.Dimensions.Depth / 2f
        );

        // Interactive Component rotation.
        var interactiveComponentRotation = CreateRotationMatrix(interactiveComponent.Rotations);
        // Learning Space rotation (currently identity as LS has no rotation).
        var learningSpaceRotation = Matrix4x4.Identity;

        // Converting to global coordinates.
        var learningSpaceCenter = new Vector3(
            learningSpace.Coordinates.XCoordinate,
            learningSpace.Coordinates.YCoordinate,
            learningSpace.Coordinates.ZCoordinate
        );

        // Center in global coordinates.
        var globalCenter = Vector3.Transform(localCenter, learningSpaceRotation) + learningSpaceCenter;

        // Final rotation combining both Interactive Component and Learning Space rotations.
        var finalRotation = interactiveComponentRotation * learningSpaceRotation;

        return new OrientedBoundingBox(globalCenter, halfExtents, finalRotation);
    }

    /// <summary>
    /// Creates a 4x4 rotation matrix based on the specified rotations around the X, Y, and Z axes.
    /// </summary>
    /// <remarks>The rotation angles are applied in the order of X-axis, Y-axis, and then Z-axis. The angles
    /// are converted from degrees to radians before creating the rotation matrices.</remarks>
    /// <param name="rotations">An object containing the rotation angles, in degrees, for the X, Y, and Z axes.</param>
    /// <returns>A <see cref="Matrix4x4"/> representing the combined rotation. If all rotation angles are approximately zero, the
    /// identity matrix is returned.</returns>
    private static Matrix4x4 CreateRotationMatrix(Rotations rotations)
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

    /// <summary>
    /// Determines whether the specified oriented bounding box (OBB) is fully contained within another OBB.
    /// </summary>
    /// <param name="innerOBB">The oriented bounding box to check for containment.</param>
    /// <param name="outerOBB">The oriented bounding box that is tested for containing the <paramref name="innerOBB"/>.</param>
    /// <returns><see langword="true"/> if all vertices of the <paramref name="innerOBB"/> are inside the <paramref
    /// name="outerOBB"/>; otherwise, <see langword="false"/>.</returns>
    private static bool IsOBBContainedInOBB(OrientedBoundingBox innerOBB, OrientedBoundingBox outerOBB)
    {
        var vertices = OBBVertices(innerOBB);

        foreach (var vertex in vertices)
        {
            if (!IsPointInsideOBB(vertex, outerOBB))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether a specified point is inside the given oriented bounding box (OBB).
    /// </summary>
    /// <remarks>The method accounts for the orientation of the bounding box by transforming the point into
    /// the local space of the OBB.</remarks>
    /// <param name="point">The point to test, represented as a <see cref="Vector3"/>.</param>
    /// <param name="obb">The oriented bounding box to test against, represented as an <see cref="OrientedBoundingBox"/>.</param>
    /// <returns><see langword="true"/> if the specified point is inside the oriented bounding box; otherwise, <see
    /// langword="false"/>.</returns>
    private static bool IsPointInsideOBB(Vector3 point, OrientedBoundingBox obb)
    {
        var invRotation = Matrix4x4.Transpose(obb.Rotation);
        var localPoint = Vector3.Transform(point - obb.Center, invRotation);

        return Math.Abs(localPoint.X) <= obb.HalfExtents.X &&
               Math.Abs(localPoint.Y) <= obb.HalfExtents.Y &&
               Math.Abs(localPoint.Z) <= obb.HalfExtents.Z;
    }

    /// <summary>
    /// Calculates the vertices of an oriented bounding box (OBB) in world space.
    /// </summary>
    /// <remarks>The method transforms the local-space vertices of a unit cube to world space based on the
    /// specified OBB's properties. The vertices are calculated by scaling the unit cube by the OBB's half-extents,
    /// applying the OBB's rotation, and translating by the OBB's center.</remarks>
    /// <param name="obb">The <see cref="OrientedBoundingBox"/> instance representing the OBB, including its center, rotation, and
    /// half-extents.</param>
    /// <returns>An array of eight <see cref="Vector3"/> structures representing the vertices of the OBB in world space.</returns>
    private static Vector3[] OBBVertices(OrientedBoundingBox obb)
    {
        var localVertices = new Vector3[]
        {
            new Vector3(-1, -1, -1), new Vector3(1, -1, -1),
            new Vector3(1, 1, -1), new Vector3(-1, 1, -1),
            new Vector3(-1, -1, 1), new Vector3(1, -1, 1),
            new Vector3(-1, 1, 1), new Vector3(1, 1, 1)
        };

        var worldVertices = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            var vertex = localVertices[i] * obb.HalfExtents;

            vertex = Vector3.Transform(vertex, obb.Rotation);

            worldVertices[i] = vertex + obb.Center;
        }

        return worldVertices;
    }
}
