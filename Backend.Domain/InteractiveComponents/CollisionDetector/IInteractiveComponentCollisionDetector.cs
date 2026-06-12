using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector;

/// <summary>
/// Interface for detecting collisions between Interactive Components.
/// </summary>
public interface IInteractiveComponentCollisionDetector
{
    /// <summary>
    /// Determines whether the specified interactive component collides with any of the already placed interactive
    /// components.
    /// </summary>
    /// <param name="interactiveComponent">The interactive component to check for collisions.</param>
    /// <param name="placedInteractiveComponents">A collection of interactive components that are already placed and against which collisions will be checked.</param>
    /// <returns><see langword="true"/> if a collision is detected; otherwise, <see langword="false"/>.</returns>
    bool DetectCollision(InteractiveComponent interactiveComponent, IEnumerable<InteractiveComponent> placedInteractiveComponents);

    /// <summary>
    /// Detects if two Interactive Components collide with each other.
    /// </summary>
    /// <param name="interactiveComponent1"> First Intercative Component. </param>
    /// <param name="interactiveComponent2"> Second Interactive Component. </param>
    /// <returns></returns>
    bool HasCollision(InteractiveComponent interactiveComponent1, InteractiveComponent interactiveComponent2);
}
