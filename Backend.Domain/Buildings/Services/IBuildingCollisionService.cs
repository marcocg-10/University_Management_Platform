using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services;

/// <summary>
/// Defines a service for checking building collisions within the theme park.
/// </summary>
public interface IBuildingCollisionService
{
    /// <summary>
    /// Checks whether the given building collides with any existing buildings.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    Task<bool> HasCollisionAsync(Building building);

    /// <summary>
    /// Checks whether the given building collides with any existing buildings,
    /// excluding a specific building by its official ID.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="excludeOfficialId">The official ID of a building to exclude from collision checks (typically the building being updated).</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when excludeOfficialId is null, empty, or whitespace.</exception>
    Task<bool> HasCollisionAsync(Building building, string excludeOfficialId);
}
