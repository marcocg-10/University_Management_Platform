using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Buildings.Mappers;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Buildings.Repositories
{
    /// <summary>
    /// Repository implementation that retrieves building data using the Kiota-generated API client.
    /// </summary>
    internal class KiotaBuildingRepository : IBuildingRepository
    {
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiotaBuildingRepository"/> class.
        /// </summary>
        /// <param name="apiClient">The Kiota API client used to communicate with the backend service.</param>
        public KiotaBuildingRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of buildings from the backend API.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains an enumerable list of <see cref="Building"/> entities.
        /// </returns>
        public async Task<IEnumerable<Building>> GetBuildingsAsync()
        {
            var response = await _apiClient.Buildings.GetAsync();

            var buildings = response?.Buildings?.Select(BuildingDtoMapper.toEntity)
                ?? Enumerable.Empty<Building>();

            return buildings;
        }
    }
}