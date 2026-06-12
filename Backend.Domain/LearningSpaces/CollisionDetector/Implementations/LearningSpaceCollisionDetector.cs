using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector.Implementations;

/// <summary>
/// Collision detector for <see cref="LearningSpace"/> entities using axis-aligned bounding boxes (AABB).
/// Assumes coordinates represent the center of the space and dimensions are full extents.
/// </summary>
internal class LearningSpaceCollisionDetector : ILearningSpaceCollisionDetector
{
    public bool DetectCollision(LearningSpace candidate, IEnumerable<LearningSpace> existingLearningSpaces)
    {
        if (candidate is null)
            throw new LearningSpaceNotFoundException(-1);

        if (existingLearningSpaces is null)
            throw new LearningSpaceNotFoundException(candidate.Id);

        foreach (var other in existingLearningSpaces)
        {
            if (HasCollision(candidate, other))
                return true;
        }
        return false;
    }

    public bool HasCollision(LearningSpace a, LearningSpace b)
    {
        if (a is null)
            throw new LearningSpaceNotFoundException(-1);

        if (b is null)
            throw new LearningSpaceNotFoundException(-1);

        // Avoid self-collision (by Id).
        if (ReferenceEquals(a, b)) return false;
        if (a.Id > 0 && b.Id > 0 && a.Id == b.Id) return false;

        // Half extents.
        double halfWidthA = a.Dimensions.Width / 2.0;
        double halfLengthA = a.Dimensions.Length / 2.0;
        double halfHeightA = a.Dimensions.Height / 2.0;

        double halfWidthB = b.Dimensions.Width / 2.0;
        double halfLengthB = b.Dimensions.Length / 2.0;
        double halfHeightB = b.Dimensions.Height / 2.0;

        // Bounds A.
        double aMinX = a.Coordinates.XCoordinate - halfWidthA;
        double aMaxX = a.Coordinates.XCoordinate + halfWidthA;
        double aMinY = a.Coordinates.YCoordinate - halfHeightA;
        double aMaxY = a.Coordinates.YCoordinate + halfHeightA;
        double aMinZ = a.Coordinates.ZCoordinate - halfLengthA;
        double aMaxZ = a.Coordinates.ZCoordinate + halfLengthA;

        // Bounds B.
        double bMinX = b.Coordinates.XCoordinate - halfWidthB;
        double bMaxX = b.Coordinates.XCoordinate + halfWidthB;
        double bMinY = b.Coordinates.YCoordinate - halfHeightB;
        double bMaxY = b.Coordinates.YCoordinate + halfHeightB;
        double bMinZ = b.Coordinates.ZCoordinate - halfLengthB;
        double bMaxZ = b.Coordinates.ZCoordinate + halfLengthB;

        bool overlapX = aMinX < bMaxX && aMaxX > bMinX;
        bool overlapY = aMinY < bMaxY && aMaxY > bMinY;
        bool overlapZ = aMinZ < bMaxZ && aMaxZ > bMinZ;

        return overlapX && overlapY && overlapZ;
    }
}
