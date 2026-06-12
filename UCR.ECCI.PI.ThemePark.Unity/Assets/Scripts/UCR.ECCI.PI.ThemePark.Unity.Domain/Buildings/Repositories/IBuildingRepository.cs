using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Repositories
{
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
    }
}