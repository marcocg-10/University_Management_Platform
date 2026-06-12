using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Unity.Application
{
    /// <summary>
    /// Defines the contract for operations related to interactive components within the Theme Park domain,
    /// specifically for managing <see cref="Board"/> entitie.
    /// </summary>
    public interface IInteractiveComponentService
    {
        /// <summary>
        /// Retrieves all <see cref="Board"/> instances from the repository.
        /// </summary>
        /// <returns>A task containing a collection of all boards in the system.</returns>
        Task<IEnumerable<Board>> ListAllBoardsAsync();

        /// <summary>
        /// Refreshes the list of <see cref="Board"/> entities by reloading the most up-to-date data
        /// from the repository. Intended to be used when the board state in the scene needs to
        /// synchronize with the current persisted data.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing the latest collection
        /// of <see cref="Board"/> objects retrieved from the repository.
        /// </returns>
        Task<IEnumerable<Board>> RefreshBoardsAsync();
    }
}
