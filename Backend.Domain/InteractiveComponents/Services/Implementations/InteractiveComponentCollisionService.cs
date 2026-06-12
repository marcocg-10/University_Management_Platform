using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services.Implementations;

/// <summary>
/// Provides services for detecting collisions between interactive components within a learning space.
/// </summary>
/// <remarks>This service is responsible for evaluating whether a given interactive component collides with any
/// other components in the same learning space. It relies on a repository to retrieve existing components and a
/// collision detector to perform the collision evaluation.</remarks>
internal class InteractiveComponentCollisionService : IInteractiveComponentCollisionService
{
    private readonly IInteractiveComponentRepository _interactiveComponentRepository;
    private readonly IInteractiveComponentCollisionDetector _interactiveComponentCollisionDetector;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveComponentCollisionService"/> class.
    /// </summary>
    /// <param name="interactiveComponentRepository">The repository used to manage and retrieve interactive components.</param>
    /// <param name="interactiveComponentCollisionDetector">The collision detector used to evaluate interactions between components.</param>
    public InteractiveComponentCollisionService(
        IInteractiveComponentRepository interactiveComponentRepository,
        IInteractiveComponentCollisionDetector interactiveComponentCollisionDetector)
    {
        _interactiveComponentRepository = interactiveComponentRepository;
        _interactiveComponentCollisionDetector = interactiveComponentCollisionDetector;
    }

    /// <summary>
    /// Detects if the given interactive component collides with any existing components in the same learning space.
    /// </summary>
    /// <param name="actualInteractiveComponent"> Interactive Component to check collisions for. </param>
    /// <returns> True if the Interactive Component collided with another. False if no collision is detected. </returns>
    public async Task<bool> DetectCollisionAsync(InteractiveComponent actualInteractiveComponent)
    {
        var placedInteractiveComponents = await _interactiveComponentRepository.GetInteractiveComponentsByLearningSpaceAsync(actualInteractiveComponent.LearningSpaceId);
        return _interactiveComponentCollisionDetector.DetectCollision(actualInteractiveComponent, placedInteractiveComponents);
    }
}
