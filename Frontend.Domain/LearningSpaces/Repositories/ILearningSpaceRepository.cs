using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;

/// <summary>
/// Defines the contract for managing the learning space entities.
/// </summary>
public interface ILearningSpaceRepository
{
    /// <summary>
    /// Asynchronously adds a new laboratory to the repository.
    /// </summary>
    /// <param name="laboratory">
    /// The laboratory entity to add to the repository.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous add operation.
    /// </returns>
    Task AddLaboratoryAsync(Laboratory laboratory);

    /// <summary>
    /// Asynchronously retrieves a collection of all laboratories.
    /// </summary>
    /// <remarks>
    /// This method does not filter or paginate the results. It retrieves all available laboratories
    /// in a single operation.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of objects representing all laboratories. 
    /// If no laboratories are found, the result is an empty collection.
    /// </returns>
    Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

    /// <summary>
    /// Asynchronously deletes a laboratory identified by its unique ID.
    /// </summary>
    /// <remarks>This method removes the specified laboratory from the database.</remarks>
    /// <param name="id">The unique identifier of the laboratory to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteLaboratoryAsync(int id);

    /// <summary>
    /// Asynchronously updates a laboratory entity.
    /// </summary>
    /// <param name="laboratory">The laboratory entity to update.</param>
    Task UpdateLaboratoryAsync(Laboratory laboratory);

    /// <summary>
    /// Asynchronously adds a new classroom to the repository.
    /// </summary>
    /// <param name="classroom">
    /// The classroom entity to add to the repository.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous add operation.
    /// </returns>
    Task AddClassroomAsync(Classroom classroom);

    /// Asynchronously retrieves a collection of all available classrooms.
    /// </summary>
    /// <remarks>
    /// This method does not filter or paginate the results. It retrieves all available classrooms
    /// in a single operation.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a collection of objects representing all classrooms. 
    /// If no classrooms are found, the result is an empty collection.
    /// </returns>
    Task<IEnumerable<Classroom>> ListClassroomsAsync();

    /// <summary>
    /// Asynchronously deletes a classroom identified by its unique ID.
    /// </summary>
    /// <remarks>This method removes the specified classroom from the database.</remarks>
    /// <param name="id">The unique identifier of the classroom to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteClassroomAsync(int id);

    /// <summary>
    /// Retrieves a paginated list of classrooms along with pagination metadata.
    /// </summary>
    /// <remarks>Use this method to retrieve classrooms in a paginated manner. Ensure that <paramref
    /// name="pageNumber"/> and <paramref name="pageSize"/>  are within valid ranges to avoid unexpected
    /// results.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of classrooms to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter classrooms by name or other attributes.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Classroom}"/> representing the classrooms in the requested
    /// page.</description> </item> <item> <description><see cref="PaginationMetadata"/> containing information about
    /// the total number of items and pages.</description> </item> </list></returns>
    Task<(IEnumerable<Classroom> Classrooms, PaginationMetadata Metadata)> ListClassroomsPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? keyword = null);

    /// Asynchronously updates a classroom entity.
    /// </summary>
    /// <param name="classroom">The classroom entity to update.</param>
    Task UpdateClassroomAsync(Classroom classroom);

    /// <summary>
    /// Retrieves a paginated list of laboratories along with pagination metadata.
    /// </summary>
    /// <remarks>Use this method to retrieve laboratories in a paginated manner. Ensure that <paramref
    /// name="pageNumber"/> and <paramref name="pageSize"/>  are within valid ranges to avoid unexpected
    /// results.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of laboratories to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter laboratories by name or other attributes.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Laboratory}"/> representing the laboratories in the requested
    /// page.</description> </item> <item> <description><see cref="PaginationMetadata"/> containing information about
    /// the total number of items and pages.</description> </item> </list></returns>
    Task<(IEnumerable<Laboratory> Laboratories, PaginationMetadata Metadata)> ListLaboratoriesPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? keyword = null);
}