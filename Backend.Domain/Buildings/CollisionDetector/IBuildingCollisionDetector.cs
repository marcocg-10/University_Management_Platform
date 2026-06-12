using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector;

/// <summary>
/// Provides methods to detect collisions between buildings in the theme park.
/// </summary>
public interface IBuildingCollisionDetector
{
    /// <summary>
    /// Determines whether two buildings collide with each other.
    /// </summary>
    /// <param name="building1">The first building to compare.</param>
    /// <param name="building2">The second building to compare.</param>
    /// <returns>True if the buildings collide; otherwise, false.</returns>
    bool DoBuildingsCollide(Building building1, Building building2);

    /// <summary>
    /// Checks if the given building collides with any building in the provided collection.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="existingBuildings">A collection of existing buildings to compare against.</param>
    /// <returns>True if a collision is found; otherwise, false.</returns>
    bool HasCollision(Building building, IEnumerable<Building> existingBuildings);

    /// <summary>
    /// Checks if the given building collides with any building in the provided collection,
    /// excluding a specific building by its official ID.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="existingBuildings">A collection of existing buildings to compare against.</param>
    /// <param name="excludeOfficialId">The official ID of a building to exclude from collision checks (typically the building being updated).</param>
    /// <returns>True if a collision is found; otherwise, false.</returns>
    bool HasCollision(Building building, IEnumerable<Building> existingBuildings, string excludeOfficialId);
}
