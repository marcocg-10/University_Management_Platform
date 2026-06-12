using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

/// <summary>
/// Interface for a learning space repository.
/// </summary>
public interface ILearningSpaceRepository
{
    /// <summary>
    /// Asynchronous operation that adds a learning space.
    /// </summary>
    /// <param name="learningSpace"></param>
    /// <returns>Asynchronous operation.</returns>
    Task AddLearningSpaceAsync(LearningSpace learningSpace);

    /// <summary>
    /// Asynchronous operation that deletes a learning space by its unique identifier.
    /// </summary>
    /// <param name="learningSpaceId">Unique identifier of the learning space.</param>
    /// <returns>Asynchronous operation.</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
    /// <exception cref="LearningSpaceDataException">Thrown when a database operation fails.</exception>
    Task DeleteLearningSpaceAsync(int learningSpaceId);

    /// <summary>
    /// Asynchronous operation that lists all learning spaces by building ID.
    /// </summary>
    /// <param name="buildingId">The ID of the building to filter learning spaces by.</param>
    /// <returns>Learning space collection associated with the specified building ID.</returns>
    Task<IEnumerable<LearningSpace>> ListLearningSpacesByBuildingIdAsync(int buildingId);

    /// <summary>
    /// Asynchronous operation that lists all laboratories.
    /// </summary>
    /// <returns>Laboratory collection as an asynchronous operation.</returns>
    Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

    /// <summary>
    /// Asynchronous operation that gets a laboratory by its ID.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to retrieve.</param>
    /// <returns>Laboratory entity if found, null otherwise.</returns>
    Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId);

    /// <summary>
    /// Asynchronous operation that updates a Laboratory specifically.
    /// </summary>
    /// <param name="laboratory">The laboratory to update.</param>
    /// <returns>Asynchronous operation.</returns>
    Task UpdateLaboratoryAsync(Laboratory laboratory);

    /// <summary>
    /// Asynchronous operation that lists all classrooms.
    /// </summary>
    /// <returns>Classroom collection as an asynchronous operation.</returns>
    Task<IEnumerable<Classroom>> ListClassroomsAsync();

    /// <summary>
    /// Asynchronous operation that gets a classroom by its ID.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to retrieve.</param>
    /// <returns>Classroom entity if found, null otherwise.</returns>
    Task<Classroom?> GetClassroomByIdAsync(int classroomId);

    /// <summary>
    /// Asynchronous operation that updates a Classroom specifically.
    /// </summary>
    /// <param name="classroom">The classroom to update.</param>
    /// <returns>Asynchronous operation.</returns>
    Task UpdateClassroomAsync(Classroom classroom);

    /// <summary>
    /// Asynchronous operation that lists all LearningSpaces Textures.
    /// </summary>
    /// <returns>Textures collection as an asynchronous operation.</returns>
    Task<IEnumerable<LearningSpaceTexture>> ListLearningSpaceTexturesAsync();

    /// <summary>
    /// Asynchronous operation that gets a learning space by its ID.
    /// This method retrieves any type of learning space (Laboratory, Classroom, etc.).
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space to retrieve.</param>
    /// <returns>LearningSpace entity if found, null otherwise.</returns>
    Task<LearningSpace?> GetLearningSpaceByIdAsync(int learningSpaceId);

    /// <summary>
    /// Paged listing for laboratories.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="keyword">The keyword to match laboratories' room identifiers.</param>
    /// <returns>A tuple containing the items and the total number of items.</returns>
    Task<(IReadOnlyList<Laboratory> Laboratories, int TotalCount)> ListLaboratoriesPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null);

    /// <summary>
    /// Paged listing for classrooms.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="keyword">The keyword to match classrooms' names.</param>
    /// <returns>A tuple containing the items and the total number of items.</returns>
    Task<(IReadOnlyList<Classroom> Classrooms, int TotalCount)> ListClassroomsPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null);
}
