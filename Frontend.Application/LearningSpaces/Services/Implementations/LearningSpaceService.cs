using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.LearningSpaces.Services.Implementations;

/// <summary>
/// Provides services for managing and retrieving information about learning spaces.
/// </summary>
internal class LearningSpaceService : ILearningSpaceService
{
    private readonly ILearningSpaceRepository _learningSpaceRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceService"/> class.
    /// </summary>
    /// <param name="learningSpaceRepository">The repository used to manage and access learning space data.</param>
    public LearningSpaceService(ILearningSpaceRepository learningSpaceRepository)
    {
        _learningSpaceRepository = learningSpaceRepository;
    }

    /// <summary>
    /// Creates a new laboratory with the specified dimensions, coordinates, and identifiers, and adds it to the
    /// repository.
    /// </summary>
    /// <remarks>The laboratory is created with the specified dimensions and coordinates, and is associated
    /// with the provided building and floor level identifiers, if any. The method adds the laboratory to the repository
    /// before returning it.</remarks>
    /// <param name="buildingId">The identifier of the building where the laboratory is located. Can be null if the laboratory is
    /// not associated with a specific building.</param>
    /// <param name="floorLevel">The floor level of the building where the laboratory is located. Can be null if the floor level
    /// is not specified.</param>
    /// <param name="roomId">The unique identifier for the laboratory. Cannot be null, empty, or whitespace.</param>
    /// <param name="colorValue">The color of the laboratory.</param>
    /// <param name="textureValue">The texture of the laboratory.</param>
    /// <param name="width">The width of the laboratory in meters. Must be a positive value.</param>
    /// <param name="length">The length of the laboratory in meters. Must be a positive value.</param>
    /// <param name="height">The height of the laboratory in meters. Must be a positive value.</param>
    /// <param name="x">The x-coordinate of the laboratory's location in the learning space.</param>
    /// <param name="y">The y-coordinate of the laboratory's location in the learning space.</param>
    /// <param name="z">The z-coordinate of the laboratory's location in the learning space.</param>
    /// <returns>A <see cref="Laboratory"/> object representing the newly created laboratory.</returns>
    /// <exception cref="ArgumentException">Thrown if roomId is null, empty, or consists only of whitespace.</exception>
    public async Task<Laboratory> CreateLaboratoryAsync(
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
        float z)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("The Room ID cannot be empty or consist only of whitespace");

        var color = LearningSpaceColor.Create(colorValue);
        var texture = LearningSpaceTexture.Create(textureValue);
        var dimensions = LearningSpaceDimensions.Create(width, length, height);
        var coordinates = LearningSpaceCoordinates.Create(x, y, z);

        var laboratory = new Laboratory(buildingId, floorLevel, roomId, color, texture, dimensions, coordinates);

        await _learningSpaceRepository.AddLaboratoryAsync(laboratory);

        return laboratory;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of all laboratories.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains 
    /// a collection of laboratories.
    /// </returns>
    public async Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
    {
        return await _learningSpaceRepository.ListLaboratoriesAsync();
    }

    /// <summary>
    /// Deletes a laboratory identified by its unique <paramref name="Id"/>.
    /// </summary>
    /// <param name="Id">The unique identifier of the laboratory to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="ValidationException">Thrown when the laboratory ID is invalid.</exception>
    /// <exception cref="DomainException">Thrown when domain-related errors occur during deletion, such as the laboratory not being found or conflicts preventing deletion.</exception>
    public async Task DeleteLaboratoryAsync(int Id)
    {
        if (Id <= 0)
            throw new ValidationException("A valid laboratory ID is required.");

        await _learningSpaceRepository.DeleteLaboratoryAsync(Id);
    }

    /// <summary>
    /// Deletes a classroom identified by its unique <paramref name="Id"/>.
    /// </summary>
    /// <param name="Id">The unique identifier of the classroom to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="ValidationException">Thrown when the classroom ID is invalid.</exception>
    /// <exception cref="DomainException">Thrown when domain-related errors occur during deletion, such as the classroom not being found or conflicts preventing deletion.</exception>
    public async Task DeleteClassroomAsync(int Id)
    {
        if (Id <= 0)
            throw new ValidationException("A valid classroom ID is required.");

        await _learningSpaceRepository.DeleteClassroomAsync(Id);
    }

    /// <summary>
    /// Updates an existing laboratory with the specified dimensions, coordinates, and identifiers.
    /// </summary>
    /// <remarks>
    /// The method creates a new laboratory entity with the provided parameters and the specified ID,
    /// then updates it in the repository. The laboratory is reconstructed with the new dimensions
    /// and coordinates, and is associated with the provided building and floor level identifiers.
    /// </remarks>
    /// <param name="laboratoryId">The unique identifier of the laboratory to update.</param>
    /// <param name="buildingId">The identifier of the building where the laboratory is located. Can be null if the laboratory is
    /// not associated with a specific building.</param>
    /// <param name="floorLevel">The floor level of the building where the laboratory is located. Can be null if the floor level
    /// is not specified.</param>
    /// <param name="roomId">The unique identifier for the laboratory. Cannot be null, empty, or whitespace.</param>
    /// <param name="colorValue">The color of the laboratory.</param>
    /// <param name="textureValue">The texture of the laboratory.</param>
    /// <param name="width">The width of the laboratory in meters. Must be a positive value.</param>
    /// <param name="length">The length of the laboratory in meters. Must be a positive value.</param>
    /// <param name="height">The height of the laboratory in meters. Must be a positive value.</param>
    /// <param name="x">The x-coordinate of the laboratory's location in the learning space.</param>
    /// <param name="y">The y-coordinate of the laboratory's location in the learning space.</param>
    /// <param name="z">The z-coordinate of the laboratory's location in the learning space.</param>
    /// <returns>A <see cref="Laboratory"/> object representing the updated laboratory.</returns>
    /// <exception cref="ArgumentException">Thrown if roomId is null, empty, or consists only of whitespace.</exception>
    public async Task<Laboratory> UpdateLaboratoryAsync(
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
        float z)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ArgumentException("The Room ID cannot be empty or consist only of whitespace", nameof(roomId));

        var color = LearningSpaceColor.Create(colorValue);
        var texture = LearningSpaceTexture.Create(textureValue);
        var dimensions = LearningSpaceDimensions.Create(width, length, height);
        var coordinates = LearningSpaceCoordinates.Create(x, y, z);

        var laboratory = new Laboratory(laboratoryId, buildingId, floorLevel, roomId, color, texture, dimensions, coordinates);

        await _learningSpaceRepository.UpdateLaboratoryAsync(laboratory);

        return laboratory;
    }

    /// <summary>
    /// Creates a new classroom with the specified dimensions, coordinates, and identifiers, and adds it to the
    /// repository.
    /// </summary>
    /// <remarks>The classroom is created with the specified dimensions and coordinates, and is associated
    /// with the provided building and floor level identifiers, if any. The method adds the classroom to the repository
    /// before returning it.</remarks>
    /// <param name="buildingId">The identifier of the building where the classroom is located. Can be null if the classroom is
    /// not associated with a specific building.</param>
    /// <param name="floorLevel">The floor level of the building where the classroom is located. Can be null if the floor level
    /// is not specified.</param>
    /// <param name="roomId">The unique identifier for the classroom. Cannot be null, empty, or whitespace.</param>
    /// <param name="colorValue">The color of the classroom.</param>
    /// <param name="textureValue">The texture of the classroom.</param>
    /// <param name="width">The width of the classroom in meters. Must be a positive value.</param>
    /// <param name="length">The length of the classroom in meters. Must be a positive value.</param>
    /// <param name="height">The height of the classroom in meters. Must be a positive value.</param>
    /// <param name="x">The x-coordinate of the classroom's location in the learning space.</param>
    /// <param name="y">The y-coordinate of the classroom's location in the learning space.</param>
    /// <param name="z">The z-coordinate of the classroom's location in the learning space.</param>
    /// <returns>A <see cref="Classroom"/> object representing the newly created classroom.</returns>
    /// <exception cref="ArgumentException">Thrown if roomId is null, empty, or consists only of whitespace.</exception>
    public async Task<Classroom> CreateClassroomAsync(
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
        float z)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("The Room ID cannot be empty or consist only of whitespace");

        var color = LearningSpaceColor.Create(colorValue);
        var texture = LearningSpaceTexture.Create(textureValue);
        var dimensions = LearningSpaceDimensions.Create(width, length, height);
        var coordinates = LearningSpaceCoordinates.Create(x, y, z);

        var classroom = new Classroom(buildingId, floorLevel, roomId, color, texture, dimensions, coordinates);

        await _learningSpaceRepository.AddClassroomAsync(classroom);

        return classroom;
    }

    /// <summary>
    /// Asynchronously retrieves a collection of all classrooms.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains 
    /// a collection of classrooms.
    /// </returns>
    public async Task<IEnumerable<Classroom>> ListClassroomsAsync()
    {
        return await _learningSpaceRepository.ListClassroomsAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of classrooms along with the total count of available classrooms.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of classrooms to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter classrooms by name or other attributes.</param> 
    /// <returns>A task that represents the asynchronous operation. The task result is a tuple containing: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Classroom}"/> representing the classrooms in the requested
    /// page.</description> </item> <item> <description>An <see cref="int"/> representing the total count of classrooms
    /// available.</description> </item> </list></returns>
    public async Task<(IEnumerable<Classroom> Classrooms, PaginationMetadata Metadata)> ListClassroomsPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? keyword = null)
    {
        var classrooms = await _learningSpaceRepository.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);

        return classrooms;
    }

    /// <summary>
    /// Updates an existing classroom with the specified dimensions, coordinates, and identifiers.
    /// </summary>
    /// <remarks>
    /// The method creates a new classroom entity with the provided parameters and the specified ID,
    /// then updates it in the repository. The classroom is reconstructed with the new dimensions
    /// and coordinates, and is associated with the provided building and floor level identifiers.
    /// </remarks>
    /// <param name="classroomId">The unique identifier of the classroom to update.</param>
    /// <param name="buildingId">The identifier of the building where the classroom is located. Can be null if the classroom is
    /// not associated with a specific building.</param>
    /// <param name="floorLevel">The floor level of the building where the classroom is located. Can be null if the floor level
    /// is not specified.</param>
    /// <param name="roomId">The unique identifier for the classroom. Cannot be null, empty, or whitespace.</param>
    /// <param name="colorValue">The color of the classroom.</param>
    /// <param name="textureValue">The texture of the classroom.</param>
    /// <param name="width">The width of the classroom in meters. Must be a positive value.</param>
    /// <param name="length">The length of the classroom in meters. Must be a positive value.</param>
    /// <param name="height">The height of the classroom in meters. Must be a positive value.</param>
    /// <param name="x">The x-coordinate of the classroom's location in the learning space.</param>
    /// <param name="y">The y-coordinate of the classroom's location in the learning space.</param>
    /// <param name="z">The z-coordinate of the classroom's location in the learning space.</param>
    /// <returns>A <see cref="Classroom"/> object representing the updated classroom.</returns>
    /// <exception cref="ArgumentException">Thrown if roomId is null, empty, or consists only of whitespace.</exception>
    public async Task<Classroom> UpdateClassroomAsync(
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
        float z)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ArgumentException("The Room ID cannot be empty or consist only of whitespace", nameof(roomId));

        var color = LearningSpaceColor.Create(colorValue);
        var texture = LearningSpaceTexture.Create(textureValue);
        var dimensions = LearningSpaceDimensions.Create(width, length, height);
        var coordinates = LearningSpaceCoordinates.Create(x, y, z);

        var classroom = new Classroom(classroomId, buildingId, floorLevel, roomId, color, texture, dimensions, coordinates);

        await _learningSpaceRepository.UpdateClassroomAsync(classroom);

        return classroom;
    }

    /// <summary>
    /// Retrieves a paginated list of laboratories along with the total count of available laboratories.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of laboratories to include in each page. Must be greater than 0.</param>
    /// <param name="keyword">An optional keyword to filter laboratories by name or other attributes.</param> 
    /// <returns>A task that represents the asynchronous operation. The task result is a tuple containing: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Laboratory}"/> representing the laboratories in the requested
    /// page.</description> </item> <item> <description>An <see cref="int"/> representing the total count of laboratories
    /// available.</description> </item> </list></returns>
    public async Task<(IEnumerable<Laboratory> Laboratories, PaginationMetadata Metadata)> ListLaboratoriesPagedAsync(
        int pageNumber, 
        int pageSize,
        string? keyword = null)
    {
        var laboratories = await _learningSpaceRepository.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);

        return laboratories;
    }
}