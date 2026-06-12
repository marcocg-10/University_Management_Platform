using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Buildings.Services.Implementations;

/// <summary>
/// Application service for managing building entities.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IBuildingService"/> 
/// interface and provides methods to interact with building data
/// through the <see cref="IBuildingRepository"/>.
/// </remarks>
internal class BuildingService: IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IBuildingCollisionService _collisionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingService"/> 
    /// class with the specified building repository.
    /// </summary>
    /// <param name="buildingRepository">The building repository used to access building data.</param>
    public BuildingService(IBuildingRepository buildingRepository, IBuildingCollisionService collisionService)
    {
        _buildingRepository = buildingRepository;
        _collisionService = collisionService;
    }

    /// <summary>
    /// Asynchronously retrieves all building entities.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a collection of <see cref="Building"/> entities.
    /// </returns>
    public async Task<IEnumerable<Building>> GetBuildingsAsync()
    {
        return await _buildingRepository.GetBuildingsAsync();
    }

    /// <summary>
    /// Asynchronously creates a new building entity after validating its properties.
    /// </summary>
    /// <param name="newBuilding">The <see cref="Building"/> entity to create.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the created <see cref="Building"/> entity.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any required property of the building is invalid or missing.
    /// </exception>
    public async Task<Building> CreateBuildingAsync(Building newBuilding)
    {
        if (await _collisionService.HasCollisionAsync(newBuilding))
        {
            throw new BuildingCollisionException(newBuilding.RenderInfo);
        }

            // Save the Building entity using the repository
            await _buildingRepository.AddBuildingAsync(newBuilding);
        // Return the created Building entity if we want to use it later o see the info
        return newBuilding;
    }
    
    /// <summary>
    /// Asynchronously updates an existing building entity after validating its properties.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity with updated information.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the updated <see cref="Building"/> entity.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when any required property of the building is invalid or missing.
    /// </exception>
    public async Task<Building> UpdateBuildingAsync(Building building)
    {
        if (await _collisionService.HasCollisionAsync(building, building.OfficialId.Value))
        {
            throw new BuildingCollisionException(building.RenderInfo);
        }
     
        await _buildingRepository.UpdateBuildingAsync(building);
        return building;
    }

    /// <summary>
    /// Asynchronously deletes a building entity by its official ID.
    /// </summary>
    /// <param name="officialId">The official ID of the building to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the official ID is invalid or missing.</exception>
    public async Task DeleteBuildingAsync(string officialId)
    {
        if (string.IsNullOrWhiteSpace(officialId))
            throw new ArgumentException("BuildingId is required");

        await _buildingRepository.DeleteBuildingAsync(officialId);
    }
}
