using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Services;

/// <summary>
/// Service for detecting spatial collisions between <see cref="LearningSpace"/> entities.
/// </summary>
public interface ILearningSpaceCollisionService
{
    /// <summary>
    /// Determines whether the provided learning space collides with any other learning space
    /// within its valid scope.
    /// </summary>
    /// <param name="candidate">Learning space to evaluate.</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    Task<bool> DetectCollisionAsync(LearningSpace candidate);
}
