using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Repositories;

/// <summary>
/// Defines a contract for accessing building data in the frontend domain.
/// </summary>
public interface IBuildingRepository
{
    /// <summary>
    /// Asynchronously retrieves a collection of buildings.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains an enumerable list of <see cref="Building"/> entities.
    /// </returns>
    Task<IEnumerable<Building>> GetBuildingsAsync();
    /// <summary>
    /// Asynchronously creates a new building entity.
    /// </summary>
    /// <param name="building">The building entity to create.</param>
    Task<Building> CreateBuildingAsync(Building building);
    /// <summary>
    /// Asynchronously deletes a building entity by its official ID.
    /// </summary>
    /// <param name="officialId">The official ID of the building to delete.</param>
    Task DeleteBuildingAsync(string officialId);

    /// <summary>
    /// Asynchronously updates a building entity.
    /// </summary>
    /// <param name="building">The building entity to update.</param>
    Task UpdateBuildingAsync(Building building);
}
