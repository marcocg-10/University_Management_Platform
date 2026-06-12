using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ContainmentDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services.Implementations;

/// <summary>
/// Provides functionality to determine the containment status of an interactive component within a specified learning
/// space.
/// </summary>
/// <remarks>This service acts as a wrapper around an existing implementation of <see
/// cref="IInteractiveComponentContainmentDetector"/>. It delegates the containment status checks to the provided
/// detector implementation.</remarks>
internal class InteractiveComponentContainmentService : IInteractiveComponentContainmentService
{
    private readonly IInteractiveComponentContainmentDetector _interactiveComponentContainmentDetector;
    private readonly ILearningSpaceRepository _learningSpaceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveComponentContainmentService"/> class.
    /// </summary>
    /// <remarks>The provided <paramref name="interactiveComponentContainmentDetector"/> is required for the
    /// service to perform containment detection. Ensure that the instance is properly configured before passing it to
    /// this constructor.</remarks>
    /// <param name="interactiveComponentContainmentDetector">An instance of <see cref="IInteractiveComponentContainmentDetector"/> used to detect containment of interactive
    /// components.</param>
    /// <param name="learningSpaceRepository">An instance of <see cref="ILearningSpaceRepository"/> used to retrieve learning space information required for containment checks.</param>
    public InteractiveComponentContainmentService(
        IInteractiveComponentContainmentDetector interactiveComponentContainmentDetector,
        ILearningSpaceRepository learningSpaceRepository)
    {
        _interactiveComponentContainmentDetector = interactiveComponentContainmentDetector;
        _learningSpaceRepository = learningSpaceRepository;
    }

    /// <summary>
    /// Determines whether the specified interactive component is contained within its associated learning space.
    /// </summary>
    /// <remarks>This method retrieves the learning space associated with the specified interactive component
    /// and checks whether the component is contained within it. The containment check is performed using the <see
    /// cref="IInteractiveComponentContainmentDetector"/> service.</remarks>
    /// <param name="actualInteractiveComponent">The interactive component to check for containment within its associated learning space.</param>
    /// <returns><see langword="true"/> if the interactive component is contained within the learning space; otherwise, <see
    /// langword="false"/>.</returns>
    public async Task<bool> GetContainmentStatusAsync(
        InteractiveComponent actualInteractiveComponent)
    {
        var learningSpace = await _learningSpaceRepository
            .GetLearningSpaceByIdAsync(actualInteractiveComponent.LearningSpaceId);

        return _interactiveComponentContainmentDetector.IsContained(actualInteractiveComponent, learningSpace);
    }
}