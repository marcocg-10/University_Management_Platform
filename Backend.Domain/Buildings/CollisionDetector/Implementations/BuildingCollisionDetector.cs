using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector.Implementations;

/// <summary>
/// Implements collision detection logic between buildings in the theme park.
/// </summary>
internal class BuildingCollisionDetector : IBuildingCollisionDetector
{
    /// <summary>
    /// Checks if the specified building collides with any building in the provided collection.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="existingBuildings">A collection of existing buildings to compare against.</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    public bool HasCollision(Building building, IEnumerable<Building> existingBuildings)
    {
        return existingBuildings.Any(b => DoBuildingsCollide(b, building));
    }

    /// <summary>
    /// Checks if the specified building collides with any building in the provided collection,
    /// excluding a specific building by its official ID.
    /// </summary>
    /// <param name="building">The building to check for collisions.</param>
    /// <param name="existingBuildings">A collection of existing buildings to compare against.</param>
    /// <param name="excludeOfficialId">The official ID of a building to exclude from collision checks (typically the building being updated).</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when excludeOfficialId is null, empty, or whitespace.</exception>
    public bool HasCollision(Building building, IEnumerable<Building> existingBuildings, string excludeOfficialId)
    {
        return existingBuildings
            .Where(b => b.OfficialId.Value != excludeOfficialId)
            .Any(b => DoBuildingsCollide(b, building));
    }

    /// <summary>
    /// Determines whether two buildings collide based on their coordinates and dimensions.
    /// </summary>
    /// <param name="building1">The first building.</param>
    /// <param name="building2">The second building.</param>
    /// <returns>True if the buildings collide; otherwise, false.</returns>
    public bool DoBuildingsCollide(Building building1, Building building2)
    {
        var x1 = building1.RenderInfo.XCoodinate.XValue;
        var z1 = building1.RenderInfo.ZCoodinate.ZValue;
        var w1 = building1.RenderInfo.Width.Value;
        var d1 = building1.RenderInfo.Depth.Value;


        var x2 = building2.RenderInfo.XCoodinate.XValue;
        var z2 = building2.RenderInfo.ZCoodinate.ZValue;
        var w2 = building2.RenderInfo.Width.Value;
        var d2 = building2.RenderInfo.Depth.Value;


        var b1Left = x1 - (w1 / 2);
        var b1Right = x1 + (w1 / 2);
        var b1Top = z1 - (d1 / 2);
        var b1Bottom = z1 + (d1 / 2);


        var b2Left = x2 - (w2 / 2);
        var b2Right = x2 + (w2 / 2);
        var b2Top = z2 - (d2 / 2);
        var b2Bottom = z2 + (d2 / 2);


        bool overlapX = b1Left < b2Right && b1Right > b2Left;
        bool overlapZ = b1Top < b2Bottom && b1Bottom > b2Top;


        return overlapX && overlapZ;
    }
}
