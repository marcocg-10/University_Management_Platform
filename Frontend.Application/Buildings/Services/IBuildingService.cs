using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services;

/// <summary>
/// Defines a service for retrieving building data in the frontend application.
/// </summary>
public interface IBuildingService
{
    /// <summary>
    /// Asynchronously retrieves a collection of buildings.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of buildings.</returns>
    Task<IEnumerable<Building>> GetBuildingsAsync();
    /// <summary>
    /// Asynchronously deletes a building by its official identifier (e.g., "B001").
    /// </summary>
    /// <param name="officialId">The official identifier of the building.</param>
    Task DeleteBuildingAsync(string officialId);

    Task<Building> CreateBuildingAsync(Building building);
    /// <summary>
    /// Asynchronously updates a building entity.
    /// </summary>
    /// <param name="building">The building entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateBuildingAsync(Building building);
}