using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Services.Implementations;

/// <summary>
/// Provides collision detection operations for <see cref="LearningSpace"/> entities.
/// </summary>
internal class LearningSpaceCollisionService : ILearningSpaceCollisionService
{
    private readonly ILearningSpaceRepository _learningSpaceRepository;
    private readonly ILearningSpaceCollisionDetector _learningSpaceCollisionDetector;

    public LearningSpaceCollisionService(
        ILearningSpaceRepository learningSpaceRepository,
        ILearningSpaceCollisionDetector learningSpaceCollisionDetector)
    {
        _learningSpaceRepository = learningSpaceRepository;
        _learningSpaceCollisionDetector = learningSpaceCollisionDetector;
    }

    /// <summary>
    /// Checks whether the given learning space collides with any other learning space
    /// in the same building and (if defined) the same floor level.
    /// </summary>
    /// <remarks>
    /// Rules:
    /// - If the candidate has no BuildingId, no collision scope exists => returns false.
    /// - Filters out the candidate itself by Id (if persisted) or reference.
    /// - If FloorLevel is set, only compares against same FloorLevel (comment out if not required).
    /// </remarks>
    public async Task<bool> DetectCollisionAsync(LearningSpace candidate)
    {
        if (candidate is null) throw new ArgumentNullException(nameof(candidate));

        var allLabs = await _learningSpaceRepository
            .ListLaboratoriesAsync();

        var allClassrooms = await _learningSpaceRepository
            .ListClassroomsAsync();

        var merged = allLabs.Cast<LearningSpace>().Concat(allClassrooms.Cast<LearningSpace>());

        // Exclude self.
        var scoped = merged.Where(ls =>
            !ReferenceEquals(ls, candidate) &&
            !(candidate.Id > 0 && ls.Id == candidate.Id));

        // Early exit if nothing to compare.
        if (!scoped.Any())
            return false;

        return _learningSpaceCollisionDetector.DetectCollision(candidate, scoped);
    }
}
