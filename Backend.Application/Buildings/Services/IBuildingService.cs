using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services;


/// <summary>
/// Defines the contract for building-related operations in the application layer.
/// </summary>
/// <remarks>
/// This interface provides methods to interact with building entities.
public interface IBuildingService
{
    /// <summary>
    /// Asynchronously retrieves all building entities.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a collection of <see cref="Building"/> entities.
    /// </returns>
    Task<IEnumerable<Building>> GetBuildingsAsync();

    /// <summary>
    /// Asynchronously creates a new building entity.
    /// </summary>
    /// <param name="newBuilding">The <see cref="Building"/> entity to create.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the created <see cref="Building"/> entity.
    /// </returns>
    Task<Building> CreateBuildingAsync(Building newBuilding);

    /// <summary>
    /// Asynchronously updates an existing building entity.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity with updated information.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the updated <see cref="Building"/> entity.
    /// </returns>
    Task<Building> UpdateBuildingAsync(Building building);

    /// <summary>
    /// Asynchronously deletes a building entity by its official ID.
    /// </summary>
    /// <param name="officialId">The official ID of the building to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteBuildingAsync(string officialId);
}
