using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UCR.ECCI.PI.ThemePark.Unity.Application.LearningSpaces.Services
{
    public interface ILearningSpaceService
    {
        Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId);

        /// <summary>
        /// Asynchronously retrieves a collection of laboratories.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of laboratories.
        /// </returns>
        Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

        Task<IEnumerable<Laboratory>> ListLaboratoriesByBuildingAsync(int buildingId);

        Task<Classroom?> GetClassroomByIdAsync(int classroomId);

        /// <summary>
        /// Asynchronously retrieves a collection of classrooms.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of classrooms.
        /// </returns>
        Task<IEnumerable<Classroom>> ListClassroomsAsync();

        Task<IEnumerable<Classroom>> ListClassroomsByBuildingAsync(int buildingId);
    }
}
