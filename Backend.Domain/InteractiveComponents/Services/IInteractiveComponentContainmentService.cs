using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services;

/// <summary>
/// Defines a service for determining the containment status of an interactive component within a specific context.
/// </summary>
/// <remarks>This interface is typically used to check whether a given interactive component is contained within a
/// designated scope or environment. Implementations may define the specific criteria for containment.</remarks>
public interface IInteractiveComponentContainmentService
{
    /// <summary>
    /// Determines whether the specified interactive component is contained within the given learning space.
    /// </summary>
    /// <param name="actualInteractiveComponent">The interactive component to check for containment.</param>
    /// <returns><see langword="true"/> if the interactive component is contained within the specified learning space; otherwise,
    /// <see langword="false"/>.</returns>
    Task<bool> GetContainmentStatusAsync(InteractiveComponent actualInteractiveComponent);
}