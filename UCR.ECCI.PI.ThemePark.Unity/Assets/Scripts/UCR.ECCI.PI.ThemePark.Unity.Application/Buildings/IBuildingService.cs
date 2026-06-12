using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Unity.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.Buildings.Services
{
    /// <summary>
    /// Defines a service for retrieving building data in the frontend application.
    /// </summary>
    public interface IBuildingService
    {
        /// <summary>
        /// Asynchronously retrieves a collection of buildings.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of buildings.</returns>
        Task<IEnumerable<Building>> GetBuildingsAsync();
    }
}