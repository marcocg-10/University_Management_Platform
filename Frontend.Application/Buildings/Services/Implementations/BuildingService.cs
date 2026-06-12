using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Buildings.Services.Implementations;

/// <summary>
/// Service implementation for retrieving building data using a repository.
/// </summary>
internal class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingService"/> class.
    /// </summary>
    /// <param name="buildingRepository">The repository used to access building data.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="buildingRepository"/> is null.
    /// </exception>
    public BuildingService(IBuildingRepository buildingRepository)
    {
        _buildingRepository = buildingRepository
            ?? throw new ArgumentNullException(nameof(buildingRepository));
    }

    /// <summary>
    /// Asynchronously retrieves a collection of buildings from the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable list of <see cref="Building"/> entities.
    /// </returns>
    public async Task<IEnumerable<Building>> GetBuildingsAsync()
    {

        return await _buildingRepository.GetBuildingsAsync();

    }

    public async Task DeleteBuildingAsync(string officialId)
    {
        try
        {
            await _buildingRepository.DeleteBuildingAsync(officialId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while deleting the building with OfficialId: {officialId}.", ex);
        }
    }

    /// <summary>
    /// Asynchronously updates a building entity.
    /// </summary>
    /// <param name="building">The building entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateBuildingAsync(Building building)
    {
            await _buildingRepository.UpdateBuildingAsync(building);
    }

    public async Task<Building> CreateBuildingAsync(Building building)
    {

        return await _buildingRepository.CreateBuildingAsync(building);


    }
}
