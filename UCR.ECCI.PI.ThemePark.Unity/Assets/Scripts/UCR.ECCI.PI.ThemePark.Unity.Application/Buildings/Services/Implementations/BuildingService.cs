using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.Buildings.Services.Implementations
{
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
    }
}