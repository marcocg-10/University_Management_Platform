using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;

/// <summary>
/// Defines the contract for building-related data operations in the domain layer.
/// </summary>
/// <remarks>
/// This interface provides methods to access building entities from the data source.
/// </remarks>
public interface IBuildingRepository
{
    /// <summary>
    /// Asynchronously retrieves all building entities.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a collection of <see cref="Building"/> entities.
    /// </returns>
    Task<IEnumerable<Building>> GetBuildingsAsync();
    Task<Building> AddBuildingAsync(Building building);

    /// <summary>
    /// Asynchronously updates an existing building entity.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity to update.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task UpdateBuildingAsync(Building building);

    /// <summary>
    /// Asynchronously deletes a building entity by its official ID.
    /// </summary>
    /// <param name="officialId">The official ID of the building to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteBuildingAsync(string officialId);
}
