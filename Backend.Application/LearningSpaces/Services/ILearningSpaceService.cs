using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services;

/// <summary>
/// Interface for managing learning spaces.
/// </summary>
public interface ILearningSpaceService
{
    /// <summary>
    /// Interface method for creating a new laboratory learning space.
    /// </summary>
    /// <param name="buildingId">Building's Id where the laboratory might be located in.</param>
    /// <param name="floorLevel">Floor level where the laboratory might be located in.</param>
    /// <param name="roomId">Room Id of the laboratory.</param>
    /// <param name="colorValue">Color value as string in HEX format.</param>
    /// <param name="textureValue">Texture value as string.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="xCoordinate">X-coordinate.</param>
    /// <param name="yCoordinate">Y-coordinate.</param>
    /// <param name="zCoordinate">Z-coordinate.</param>
    /// <returns>An instance of a new laboratory.</returns>
    Task<Laboratory> CreateLaboratoryAsync(
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float xCoordinate,
        float yCoordinate,
        float zCoordinate);

    /// <summary>
    /// Interface method for updating an existing laboratory learning space.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to update.</param>
    /// <param name="buildingId">Building's Id where the laboratory might be located in.</param>
    /// <param name="floorLevel">Floor level where the laboratory might be located in.</param>
    /// <param name="roomId">Room Id of the laboratory.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="xCoordinate">X-coordinate.</param>
    /// <param name="yCoordinate">Y-coordinate.</param>
    /// <param name="zCoordinate">Z-coordinate.</param>
    /// <returns>The updated laboratory instance.</returns>
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
        float xCoordinate,
        float yCoordinate,
        float zCoordinate);

    /// <summary>
    /// Interface method for creating a new classroom learning space.
    /// </summary>
    /// <param name="buildingId">Building's Id where the classroom might be located in.</param>
    /// <param name="floorLevel">Floor level where the classroom might be located in.</param>
    /// <param name="roomId">Room Id of the classroom.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="xCoordinate">X-coordinate.</param>
    /// <param name="yCoordinate">Y-coordinate.</param>
    /// <param name="zCoordinate">Z-coordinate.</param>
    /// <returns>An instance of a new classroom.</returns>
    Task<Classroom> CreateClassroomAsync(
        int? buildingId,
        int? floorLevel,
        string roomId,
        string colorValue,
        string textureValue,
        float width,
        float length,
        float height,
        float xCoordinate,
        float yCoordinate,
        float zCoordinate);

    /// <summary>
    /// Interface method for updating an existing classroom learning space.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to update.</param>
    /// <param name="buildingId">Building's Id where the classroom might be located in.</param>
    /// <param name="floorLevel">Floor level where the classroom might be located in.</param>
    /// <param name="roomId">Room Id of the classroom.</param>
    /// <param name="width">Width in meters.</param>
    /// <param name="length">Length in meters.</param>
    /// <param name="height">Height in meters.</param>
    /// <param name="xCoordinate">X-coordinate.</param>
    /// <param name="yCoordinate">Y-coordinate.</param>
    /// <param name="zCoordinate">Z-coordinate.</param>
    /// <returns>The updated classroom instance.</returns>
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
        float xCoordinate,
        float yCoordinate,
        float zCoordinate);

    /// <summary>
    /// Represents the interface for listing all learning spaces associated with a specific building ID.
    /// </summary>
    /// <param name="buildingId">The ID of the building to filter learning spaces by.</param>
    /// <returns>An IEnumerable of LearningSpace objects representing the learning spaces in the specified building.
    /// If no learning spaces are available for the building, the result will be empty.</returns>
    Task<IEnumerable<LearningSpace>> ListLearningSpacesByBuildingIdAsync(int buildingId);

    /// <summary>
    /// Represents the interface for the listing of all laboratory learning spaces.
    /// </summary>
    /// <returns>An  IEnumerable{T} of Laboratory objects representing the laboratories.
    /// If no laboratories are available, the result will be empty.</returns>
    Task<IEnumerable<Laboratory>> ListLaboratoriesAsync();

    /// <summary>
    /// Represents the interface for the listing of all classroom learning spaces.
    /// </summary>
    /// <returns>An  IEnumerable{T} of Classroom objects representing the classrooms.
    /// If no classrooms are available, the result will be empty.</returns>
    Task<IEnumerable<Classroom>> ListClassroomsAsync();

    /// <summary>
    /// Deletes an existing laboratory learning space by its ID.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to delete.</param>
    /// <param name="isAdmin">Indicates if the user has admin privileges.</param>
    /// <returns>Asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized.</exception>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the laboratory does not exist.</exception>
    /// <exception cref="LearningSpaceDataException">Thrown when the delete operation fails.</exception>
    Task DeleteLaboratoryAsync(int laboratoryId, bool isAdmin);

    /// <summary>
    /// Deletes an existing classroom learning space by its ID.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to delete.</param>
    /// <returns>Asynchronous operation.</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the classroom does not exist.</exception>
    /// <exception cref="LearningSpaceDataException">Thrown when the delete operation fails.</exception>
    Task DeleteClassroomAsync(int classroomId);

    /// <summary>
    /// Represents the interface for reading one laboratory learning space.
    /// </summary>
    /// <param name="laboratoryId"></param>
    /// <returns>An instance of a laboratory.</returns>
    Task<Laboratory?> ReadLaboratoryByIdAsync(int laboratoryId);

    /// <summary>
    /// Represents the interface for reading one classroom learning space.
    /// </summary>
    /// <param name="classroomId"></param>
    /// <returns>An instance of a classroom.</returns>
    Task<Classroom?> ReadClassroomByIdAsync(int classroomId);

    /// <summary>
    /// Represents the interface for reading one learning space by its ID.
    /// This method retrieves any type of learning space (Laboratory, Classroom, etc.).
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space to retrieve.</param>
    /// <returns>An instance of a learning space.</returns>
    Task<LearningSpace?> ReadLearningSpaceByIdAsync(int learningSpaceId);

    /// <summary>
    /// Represents the interface for listing all available learning space colors.
    /// </summary>
    /// <returns>An  IEnumerable{T} of Texture objects representing the Learning Spaces Textures.</returns>
    /// <remarks>This are already defined in the database.</remarks>
    Task<IEnumerable<LearningSpaceTexture>> ListLearningSpaceTexturesAsync();

    /// <summary>
    /// Represents the interface for listing laboratories with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of laboratories per page.</param>
    /// <param name="keyword">The keyword to match laboratories' room identifiers.</param>
    /// <returns>
    /// A tuple containing a read-only list of laboratories for the specified page.
    /// </returns>
    Task<(IReadOnlyList<Laboratory> Laboratories, int TotalCount)> ListLaboratoriesPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null);

    /// <summary>
    /// Represents the interface for listing classrooms with pagination.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of classrooms per page.</param>
    /// <param name="keyword">The keyword to match classrooms' names.</param>
    /// <returns>
    /// A tuple containing a read-only list of classrooms for the specified page.
    /// </returns>
    Task<(IReadOnlyList<Classroom> Classrooms, int TotalCount)> ListClassroomsPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null);
}
