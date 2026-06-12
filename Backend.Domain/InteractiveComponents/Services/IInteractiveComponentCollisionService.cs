using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services;

/// <summary>
/// Interface for the interactive component collision service.
/// </summary>
public interface IInteractiveComponentCollisionService
{
    /// <summary>
    /// Detects if the given interactive component collides with any existing components in the same learning space.
    /// </summary>
    /// <param name="actualInteractiveComponent"> Interactive Component to check collisions for. </param>
    /// <returns> True if the Interactive Component collided with another. False if no collision is detected. </returns>
    Task<bool> DetectCollisionAsync(InteractiveComponent actualInteractiveComponent);
}
