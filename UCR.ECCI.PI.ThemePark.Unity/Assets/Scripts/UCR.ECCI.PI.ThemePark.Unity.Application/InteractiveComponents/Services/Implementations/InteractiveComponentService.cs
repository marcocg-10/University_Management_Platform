using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Repositories;

namespace UCR.ECCI.PI.ThemePark.Unity.Application
{
    /// <summary>
    /// Service layer implementation responsible for managing <see cref="InteractiveComponent"/> entities,
    /// specifically <see cref="Board"/> and <see cref="Projector"/> types.
    /// </summary>
    /// <remarks>
    /// This service provides methods for listing boards.
    /// It acts as a mediator between the application layer and the persistence layer, ensuring 
    /// that domain rules are enforced when performing operations.
    /// </remarks>
    public class InteractiveComponentService : IInteractiveComponentService
    {
        private readonly IInteractiveComponentRepository _interactiveComponentRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="InteractiveComponentService"/>.
        /// </summary>
        /// <param name="interactiveComponentRepository">
        /// Repository responsible for persisting and retrieving interactive components.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentRepository"/> is null.</exception>
        public InteractiveComponentService(IInteractiveComponentRepository interactiveComponentRepository)
        {
            _interactiveComponentRepository = interactiveComponentRepository
                ?? throw new ArgumentNullException(nameof(interactiveComponentRepository));
        }

        /// <summary>
        /// Retrieves all <see cref="Board"/> instances from the repository.
        /// </summary>
        /// <returns>A task containing a collection of all boards in the system.</returns>
        public async Task<IEnumerable<Board>> ListAllBoardsAsync()
        {
            var boards = await _interactiveComponentRepository.ListAllBoardsAsync();
            return boards;
        }

        /// <summary>
        /// Refreshes the list of <see cref="Board"/> entities by reloading the most up-to-date data
        /// from the repository. Intended to be used when the board state in the scene needs to
        /// synchronize with the current persisted data.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing the latest collection
        /// of <see cref="Board"/> objects retrieved from the repository.
        /// </returns>
        public async Task<IEnumerable<Board>> RefreshBoardsAsync()
        {
            var boards = await _interactiveComponentRepository.ListAllBoardsAsync();
            return boards;
        }
    }
}
