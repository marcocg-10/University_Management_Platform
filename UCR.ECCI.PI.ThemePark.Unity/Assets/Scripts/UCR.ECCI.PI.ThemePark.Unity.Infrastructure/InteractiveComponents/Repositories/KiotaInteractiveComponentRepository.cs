using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.InteractiveComponents.Mappers;
using UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Kiota;

/// <summary>
/// Repository implementation for managing <see cref="InteractiveComponent"/> entities.
/// </summary>
/// <remarks>
/// This repository handles CRUD operations.
/// </remarks>
namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.InteractiveComponents.Repositories
{
    public class KiotaInteractiveComponentRepository : IInteractiveComponentRepository
    {
        private readonly ApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiClient"/>.
        /// </summary>
        /// <param name="apiClient">The Api client to make requests using Kiota.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiClient"/> is null.</exception>
        public KiotaInteractiveComponentRepository(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Retrieves all <see cref="Board"/> entities.
        /// </summary>
        /// <returns>A collection of all boards.</returns>
        public async Task<IEnumerable<Board>> ListAllBoardsAsync()
        {
            var response = await _apiClient.InteractiveComponents.Board.GetAsync();

            var boards = response?.Boards?.Select(BoardDtoMapper.ToEntity)
                          ?? Enumerable.Empty<Board>();

            return boards;
        }
    }
}
