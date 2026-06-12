using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector;

/// <summary>
/// Detects spatial collisions between <see cref="LearningSpace"/> entities.
/// </summary>
public interface ILearningSpaceCollisionDetector
{
    /// <summary>
    /// Checks if the candidate learning space collides with any of the provided existing learning spaces.
    /// </summary>
    /// <param name="candidate">Learning space to evaluate.</param>
    /// <param name="existingLearningSpaces">Collection of learning spaces in the relevant scope.</param>
    /// <returns>True if a collision is detected; otherwise false.</returns>
    bool DetectCollision(LearningSpace candidate, IEnumerable<LearningSpace> existingLearningSpaces);

    /// <summary>
    /// Determines whether two learning spaces collide (AABB, center-based).
    /// </summary>
    /// <param name="a">First learning space.</param>
    /// <param name="b">Second learning space.</param>
    /// <returns>True if they overlap on X, Y and Z; otherwise false.</returns>
    bool HasCollision(LearningSpace a, LearningSpace b);
}
