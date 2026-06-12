using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector;

/// <summary>
/// Provides functionality to detect whether an interactive component is contained within a specified learning space.
/// </summary>
/// <remarks>This interface defines a method for verifying the containment relationship between interactive
/// components and learning spaces. Implementations of this interface should ensure that the containment check is
/// performed accurately based on the provided parameters.</remarks>
public interface IInteractiveComponentContainmentDetector
{
    /// <summary>
    /// Determines whether the specified interactive component is contained within the given learning space.
    /// </summary>
    /// <param name="interactiveComponents">The interactive component to check for containment.</param>
    /// <param name="learningSpace">The learning space in which to check for the presence of the interactive component.</param>
    /// <returns><see langword="true"/> if the interactive component is contained within the learning space; otherwise, <see
    /// langword="false"/>.</returns>
    bool IsContained(InteractiveComponent interactiveComponents, LearningSpace learningSpace);
}
