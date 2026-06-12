using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services;

/// <summary>
/// Interface for managing learning spaces.
/// </summary>
public interface ILearningSpaceService
{
    /// <summary>
    /// Creates a new laboratory asynchronously.
    /// </summary>
    /// <param name="buildingId">Id of building where laboratory is located.</param>
    /// <param name="floorLevel">Floor level of laboratory inside building. </param>
    /// <param name="roomId">Room where the laboratory is located. </param>
    /// <param name="colorValue">Color of the laboratory.</param>
    /// <param name="textureValue">Texture of the laboratory.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="x">X-Coordinate</param>
    /// <param name="y">Y-Coordinate</param>
    /// <param name="z">Z-Coordinate</param>
    /// <returns>An instance of the laboratory created.</returns>
    Task<Laboratory> CreateLaboratoryAsync(
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float x,
        float y,
        float z
    );

    /// <summary>
    /// Asynchronously retrieves a collection of laboratories.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of laboratories.
    /// </returns>
    Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

    /// <summary>
    /// Asynchronously deletes a laboratory by its identifier.
    /// </summary>
    /// <param name="Id">The identifier of the laboratory.</param>
    Task DeleteLaboratoryAsync(int Id);

    /// <summary>
    /// Updates an existing laboratory asynchronously.
    /// </summary>
    /// <param name="laboratoryId">The unique identifier of the laboratory to update.</param>
    /// <param name="buildingId">Id of building where laboratory is located.</param>
    /// <param name="floorLevel">Floor level of laboratory inside building.</param>
    /// <param name="roomId">Room where the laboratory is located.</param>
    /// <param name="colorValue">Color of the laboratory.</param>
    /// <param name="textureValue">Texture of the laboratory.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="x">X-Coordinate</param>
    /// <param name="y">Y-Coordinate</param>
    /// <param name="z">Z-Coordinate</param>
    /// <returns>An instance of the updated laboratory.</returns>

    Task<Laboratory> UpdateLaboratoryAsync(
        int laboratoryId,
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float x,
        float y,
        float z
    );

    /// <summary>
    /// Creates a new classroom asynchronously.
    /// </summary>
    /// <param name="buildingId">Id of building where classroom is located.</param>
    /// <param name="floorLevel">Floor level of classroom inside building. </param>
    /// <param name="roomId">Room where the classroom is located. </param>
    /// <param name="colorValue">Color of the classroom.</param>
    /// <param name="textureValue">Texture of the classroom.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="x">X-Coordinate</param>
    /// <param name="y">Y-Coordinate</param>
    /// <param name="z">Z-Coordinate</param>
    /// <returns>An instance of the classroom created.</returns>
    Task<Classroom> CreateClassroomAsync(
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float x,
        float y,
        float z
    );

    /// <summary>
    /// Asynchronously retrieves a collection of classrooms.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of classrooms.
    /// </returns>
    Task<IEnumerable<Classroom>> ListClassroomsAsync();

    /// <summary>
    /// Asynchronously deletes a classroom by its identifier.
    /// </summary>
    /// <param name="Id">The identifier of the classroom.</param>
    Task DeleteClassroomAsync(int Id);

    /// <summary>
    /// Retrieves a paginated list of classrooms along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of classrooms to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter classrooms by name or other attributes.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Classroom}"/> representing the classrooms in the requested
    /// page.</description> </item> <item> <description><see cref="PaginationMetadata"/> containing information about
    /// the total number of items and pages.</description> </item> </list></returns>
    Task<(IEnumerable<Classroom> Classrooms, PaginationMetadata Metadata)> ListClassroomsPagedAsync(int pageNumber, int pageSize, string? keyword = null);

    /// <summary>
    /// Updates an existing classroom asynchronously.
    /// </summary>
    /// <param name="classroomId">The unique identifier of the classroom to update.</param>
    /// <param name="buildingId">Id of building where classroom is located.</param>
    /// <param name="floorLevel">Floor level of classroom inside building.</param>
    /// <param name="roomId">Room where the classroom is located.</param>
    /// <param name="colorValue">Color of the classroom.</param>
    /// <param name="textureValue">Texture of the classroom.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="x">X-Coordinate</param>
    /// <param name="y">Y-Coordinate</param>
    /// <param name="z">Z-Coordinate</param>
    /// <returns>An instance of the updated classroom.</returns>
    Task<Classroom> UpdateClassroomAsync(
        int classroomId,
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float x,
        float y,
        float z
    );

    /// <summary>
    /// Retrieves a paginated list of laboratories along with pagination metadata.
    /// </summary>
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