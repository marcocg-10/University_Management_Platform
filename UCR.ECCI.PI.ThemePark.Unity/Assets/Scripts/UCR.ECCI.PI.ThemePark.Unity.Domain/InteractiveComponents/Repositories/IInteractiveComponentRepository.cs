using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Repositories
{
    /// <summary>
    /// Defines the contract for managing <see cref="InteractiveComponent"/> entities, specifically <see cref="Board"/> instances,
    /// in a persistence layer.
    /// </summary>
    /// <remarks>
    /// This interface allows the service layer to interact with the data layer without being coupled to a specific persistence mechanism.
    /// </remarks>
    public interface IInteractiveComponentRepository
    {
        /// <summary>
        /// Retrieves all <see cref="Board"/> entities.
        /// </summary>
        /// <returns>A collection of all boards.</returns>
        Task<IEnumerable<Board>> ListAllBoardsAsync();
    }
}
