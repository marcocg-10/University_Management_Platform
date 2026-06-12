using System.Collections.Generic;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Repositories
{
    /// <summary>
    /// Interface for a learning space repository.
    /// </summary>
    public interface ILearningSpaceRepository
    {


        /// <summary>
        /// Asynchronous operation that gets a laboratory by its ID.
        /// </summary>
        /// <param name="laboratoryId">The ID of the laboratory to retrieve.</param>
        /// <returns>Laboratory entity if found, null otherwise.</returns>
        Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId);

        /// <summary>
        /// Asynchronous operation that lists all laboratories.
        /// </summary>
        /// <returns>Laboratory collection as an asynchronous operation.</returns>
        Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

        /// <summary>
        /// Asynchronous operation that gets a classroom by its ID.
        /// </summary>
        /// <param name="classroomId">The ID of the classroom to retrieve.</param>
        /// <returns>Classroom entity if found, null otherwise.</returns>
        Task<Classroom?> GetClassroomByIdAsync(int classroomId);

        /// <summary>
        /// Asynchronous operation that lists all classrooms.
        /// </summary>
        /// <returns>Classroom collection as an asynchronous operation.</returns>
        Task<IEnumerable<Classroom>> ListClassroomsAsync();

    }

}
