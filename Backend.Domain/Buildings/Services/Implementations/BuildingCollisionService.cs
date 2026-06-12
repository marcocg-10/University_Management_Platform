using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services.Implementations;

/// <summary>
/// Service that checks for building collisions using repository data and collision detection logic.
/// </summary>
internal class BuildingCollisionService : IBuildingCollisionService
{
    private readonly IBuildingRepository _buildingRepo;
    private readonly IBuildingCollisionDetector _buildingCollisionDetector;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingCollisionService"/> class.
    /// </summary>
    /// <param name="buildingRepo">Repository to retrieve existing buildings.</param>
    /// <param name="collisionDetector">Collision detector to evaluate building overlaps.</param>
    public BuildingCollisionService(IBuildingRepository buildingRepo, IBuildingCollisionDetector collisionDetector)
    {
        _buildingRepo = buildingRepo;
        _buildingCollisionDetector = collisionDetector;
    }

    /// <summary>
    /// Asynchronously checks if the given building collides with any existing buildings.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <returns>True if a collision is found; otherwise, false.</returns>
    public async Task<bool> HasCollisionAsync(Building building)
    {
        var buildings = await _buildingRepo.GetBuildingsAsync();
        var result = _buildingCollisionDetector.HasCollision(building, buildings);

        return result;

    }

    /// <summary>
    /// Asynchronously checks if the given building collides with any existing buildings,
    /// excluding a specific building by its official ID.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="excludeOfficialId">The official ID of a building to exclude from collision checks (typically the building being updated).</param>
    /// <returns>True if a collision is found; otherwise, false.</returns>
    public async Task<bool> HasCollisionAsync(Building building, string excludeOfficialId)
    {
        var buildings = await _buildingRepo.GetBuildingsAsync();
        var result = _buildingCollisionDetector.HasCollision(building, buildings, excludeOfficialId);

        return result;
    }
}
