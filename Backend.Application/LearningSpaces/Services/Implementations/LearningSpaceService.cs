using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.LearningSpaces.Services.Implementations;

/// <summary>
/// Implementation of ILearningSpaceService for managing interface of learning spaces.
/// </summary>
internal class LearningSpaceService : ILearningSpaceService
{
    /// <summary>
    /// Represents the learning space repository.
    /// </summary>
    private readonly ILearningSpaceRepository _learningSpaceRepository;

    /// <summary>
    /// Creates an instance of LearningSpaceService with a given repository.
    /// </summary>
    /// <param name="learningSpaceRepository">Repository used.</param>
    /// <param name="learningSpaceCollisionService">Service used for collision detection.</param>
    public LearningSpaceService(ILearningSpaceRepository learningSpaceRepository)
    {
        _learningSpaceRepository = learningSpaceRepository;
    }

    /// <summary>
    /// Represents the implementation of the interface of the creation of a new laboratory 
    /// learning space.
    /// </summary>
    /// <exception cref="ValidationException">Thrown when input parameters are invalid.</exception>
    public async Task<Laboratory> CreateLaboratoryAsync(
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
        float zCoordinate)
    {
        // Arguments validation.
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("Room ID is required and cannot be empty.");

        if (!LearningSpaceColor.TryCreate(colorValue, out var color))
            throw new ValidationException("Invalid color format. Color must be in hexadecimal format (e.g., #FFFFFF or #FFF).");

        if (!LearningSpaceTexture.TryCreate(textureValue, out var texture))
            throw new ValidationException("Invalid texture format. Texture cannot be empty.");

        if (!LearningSpaceDimensions.TryCreate(width, length, height, out var dimensions))
            throw new ValidationException("Invalid dimensions provided. Width, length, and height must be positive numbers.");

        if (!LearningSpaceCoordinates.TryCreate(xCoordinate, yCoordinate, zCoordinate, out var coordinates))
            throw new ValidationException("Invalid coordinates provided. X, Y, and Z coordinates must be valid numbers.");

        var laboratory = new Laboratory(
            buildingId,
            floorLevel,
            roomId,
            color!,
            texture!,
            dimensions!,
            coordinates!);

        // Let domain exceptions bubble up - they will be handled by the presentation layer
        await _learningSpaceRepository.AddLearningSpaceAsync(laboratory);

        return laboratory;
    }

    /// <summary>
    /// Represents the implementation of the interface for updating an existing laboratory 
    /// learning space.
    /// </summary>
    /// <exception cref="LearningSpaceDataException">Thrown when input parameters are invalid.</exception>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
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
        float xCoordinate,
        float yCoordinate,
        float zCoordinate)
    {
        // Arguments validation.
        if (laboratoryId <= 0)
            throw new ValidationException("Laboratory ID must be a positive number.");

        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("Room ID is required and cannot be empty.");

        if (!LearningSpaceColor.TryCreate(colorValue, out var color))
            throw new ValidationException("Invalid color format. Color must be in hexadecimal format (e.g., #FFFFFF or #FFF).");

        if (!LearningSpaceTexture.TryCreate(textureValue, out var texture))
            throw new ValidationException("Invalid texture format. Texture cannot be empty.");

        if (!LearningSpaceDimensions.TryCreate(width, length, height, out var dimensions))
            throw new ValidationException("Invalid dimensions provided. Width, length, and height must be positive numbers.");

        if (!LearningSpaceCoordinates.TryCreate(xCoordinate, yCoordinate, zCoordinate, out var coordinates))
            throw new ValidationException("Invalid coordinates provided. X, Y, and Z coordinates must be valid numbers.");

        // Retrieve existing laboratory
        var existingLaboratory = await _learningSpaceRepository.GetLaboratoryByIdAsync(laboratoryId);
        
        if (existingLaboratory is null)
            throw new LearningSpaceNotFoundException(laboratoryId);

        // Create updated laboratory with the same ID but new properties
        var updatedLaboratory = new Laboratory(
            laboratoryId,
            buildingId,
            floorLevel,
            roomId,
            color!,
            texture!,
            dimensions!,
            coordinates!);

        // Update the learning space
        await _learningSpaceRepository.UpdateLaboratoryAsync(updatedLaboratory);

        return updatedLaboratory;
    }

    /// <summary>
    /// Represents the implementation of the interface of the creation of a new Classroom 
    /// learning space.
    /// </summary>
    /// <param name="buildingId">Identifier of the building where the classroom is located. Can be null.</param>
    /// <param name="floorLevel">Floor level where the classroom is located. Can be null.</param>
    /// <param name="roomId">Unique identifier or name of the classroom.</param>
    /// <param name="colorValue">Color of the classroom in hexadecimal format (e.g., #FFFFFF).</param>
    /// <param name="textureValue">Texture of the classroom as a string.</param>
    /// <param name="width">Width of the classroom in meters.</param>
    /// <param name="length">Length of the classroom in meters.</param>
    /// <param name="height">Height of the classroom in meters.</param>
    /// <param name="xCoordinate">X-coordinate of the classroom's position in 3D space.</param>
    /// <param name="yCoordinate">Y-coordinate of the classroom's position in 3D space.</param>
    /// <param name="zCoordinate">Z-coordinate of the classroom's position in 3D space.</param>
    /// <returns>The newly created <see cref="Classroom"/> entity.</returns>
    /// <exception cref="ValidationException">Thrown when input parameters are invalid.</exception>
    public async Task<Classroom> CreateClassroomAsync(
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
        float zCoordinate)
    {
        // Arguments validation.
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("Room ID is required and cannot be empty.");

        if (!LearningSpaceColor.TryCreate(colorValue, out var color))
            throw new ValidationException("Invalid color format. Color must be in hexadecimal format (e.g., #FFFFFF or #FFF).");

        if (!LearningSpaceTexture.TryCreate(textureValue, out var texture))
            throw new ValidationException("Invalid texture format. Texture cannot be empty.");

        if (!LearningSpaceDimensions.TryCreate(width, length, height, out var dimensions))
            throw new ValidationException("Invalid dimensions provided. Width, length, and height must be positive numbers.");

        if (!LearningSpaceCoordinates.TryCreate(xCoordinate, yCoordinate, zCoordinate, out var coordinates))
            throw new ValidationException("Invalid coordinates provided. X, Y, and Z coordinates must be valid numbers.");

        var classroom = new Classroom(
            buildingId,
            floorLevel,
            roomId,
            color!,
            texture!,
            dimensions!,
            coordinates!);

        // Let domain exceptions bubble up - they will be handled by the presentation layer
        await _learningSpaceRepository.AddLearningSpaceAsync(classroom);

        return classroom;
    }

    /// <summary>
    /// Represents the implementation of the interface for updating an existing classroom 
    /// learning space.
    /// </summary>
    /// <exception cref="LearningSpaceDataException">Thrown when input parameters are invalid.</exception>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
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
        float xCoordinate,
        float yCoordinate,
        float zCoordinate)
    {
        // Arguments validation.
        if (classroomId <= 0)
            throw new ValidationException("Classroom ID must be a positive number.");

        if (string.IsNullOrWhiteSpace(roomId))
            throw new ValidationException("Room ID is required and cannot be empty.");

        if (!LearningSpaceColor.TryCreate(colorValue, out var color))
            throw new ValidationException("Invalid color format. Color must be in hexadecimal format (e.g., #FFFFFF or #FFF).");

        if (!LearningSpaceTexture.TryCreate(textureValue, out var texture))
            throw new ValidationException("Invalid texture format. Texture cannot be empty.");

        if (!LearningSpaceDimensions.TryCreate(width, length, height, out var dimensions))
            throw new ValidationException("Invalid dimensions provided. Width, length, and height must be positive numbers.");

        if (!LearningSpaceCoordinates.TryCreate(xCoordinate, yCoordinate, zCoordinate, out var coordinates))
            throw new ValidationException("Invalid coordinates provided. X, Y, and Z coordinates must be valid numbers.");

        // Retrieve existing classroom
        var existingClassroom = await _learningSpaceRepository.GetClassroomByIdAsync(classroomId);

        if (existingClassroom is null)
            throw new LearningSpaceNotFoundException(classroomId);

        // Create updated classroom with the same ID but new properties
        var updatedClassroom = new Classroom(
            classroomId,
            buildingId,
            floorLevel,
            roomId,
            color!,
            texture!,
            dimensions!,
            coordinates!);

        // Update the learning space
        await _learningSpaceRepository.UpdateClassroomAsync(updatedClassroom);

        return updatedClassroom;
    }

    /// <summary>
    /// Represents the implementation of the interface for listing all learning spaces
    /// associated with a specific building ID.
    /// </summary>
    /// <param name="buildingId">The ID of the building to filter learning spaces by.</param>
    /// <returns>Learning space collection associated with the specified building ID.</returns>
    /// <exception cref="ValidationException">Thrown when input parameters are invalid.</exception>
    public async Task<IEnumerable<LearningSpace>> ListLearningSpacesByBuildingIdAsync(int buildingId)
    {
        // Validate building ID
        if (buildingId <= 0)
            throw new ValidationException("Building ID must be a positive number.");

        return await _learningSpaceRepository.ListLearningSpacesByBuildingIdAsync(buildingId);
    }

    /// <summary>
    /// Represents the implementation of the interface for the listing of all laboratory
    /// learning spaces.
    /// </summary>
    /// <returns>Laboratory collection as an asynchronous operation.</returns>
    public Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
    {
        return _learningSpaceRepository.ListLaboratoriesAsync();
    }

    /// <summary>
    /// Deletes an existing laboratory learning space by its ID.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to delete.</param>
    /// <param name="isAdmin">Indicates if the user has admin privileges.</param>
    /// <returns>Asynchronous operation.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized.</exception>
    public async Task DeleteLaboratoryAsync(int laboratoryId, bool isAdmin)
    {
        if (!isAdmin)
            throw new UnauthorizedAccessException("Only administrators can delete laboratories.");

        await _learningSpaceRepository.DeleteLearningSpaceAsync(laboratoryId);
    }

    /// <summary>
    /// Deletes an existing classroom learning space by its ID.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to delete.</param>
    /// <returns>Asynchronous operation.</returns>
    public async Task DeleteClassroomAsync(int classroomId)
    {
        if (classroomId <= 0)
            throw new ValidationException("Classroom ID must be a positive number.");
        await _learningSpaceRepository.DeleteLearningSpaceAsync(classroomId);
    }

    /// <summary>
    /// Represents the implementation of the interface for the listing of all classroom
    /// learning spaces.
    /// </summary>
    /// <returns>Classroom collection as an asynchronous operation.</returns>
    public Task<IEnumerable<Classroom>> ListClassroomsAsync()
    {
        return _learningSpaceRepository.ListClassroomsAsync();
    }

    /// <summary>
    /// Reads a laboratory by its ID.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to retrieve.</param>
    /// <returns>The laboratory if found.</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
    public async Task<Laboratory?> ReadLaboratoryByIdAsync(int laboratoryId)
    {
        var laboratory = await _learningSpaceRepository.GetLaboratoryByIdAsync(laboratoryId);

        // Handle not found case.
        return laboratory is null ? throw new LearningSpaceNotFoundException(laboratoryId) : laboratory;
    }

    /// <summary>
    /// Reads a classroom by its ID.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to retrieve.</param>
    /// <returns>The classroom if found.</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
    public async Task<Classroom?> ReadClassroomByIdAsync(int classroomId)
    {
        var classroom = await _learningSpaceRepository.GetClassroomByIdAsync(classroomId);

        // Handle not found case.
        return classroom is null ? throw new LearningSpaceNotFoundException(classroomId) : classroom;
    }

    /// <summary>
    /// Reads a learning space by its ID.
    /// This method retrieves any type of learning space (Laboratory, Classroom, etc.).
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space to retrieve.</param>
    /// <returns>The learning space if found.</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the learning space does not exist.</exception>
    public async Task<LearningSpace?> ReadLearningSpaceByIdAsync(int learningSpaceId)
    {
        var learningSpace = await _learningSpaceRepository.GetLearningSpaceByIdAsync(learningSpaceId);

        // Handle not found case.
        return learningSpace is null ? throw new LearningSpaceNotFoundException(learningSpaceId) : learningSpace;
    }

    /// <summary>
    /// Represents the implementation of the interface for the listing of all Learning Space textures.
    /// </summary>
    /// <returns>LearningSpaceTexture collection as an asynchronous operation.</returns>
    public Task<IEnumerable<LearningSpaceTexture>> ListLearningSpaceTexturesAsync()
    {
        return _learningSpaceRepository.ListLearningSpaceTexturesAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of laboratories from the repository.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="keyword">The keyword to match laboratories' room identifiers.</param>
    /// <returns>A tuple containing the laboratories for the current page and the total count of laboratories.</returns>
    public Task<(IReadOnlyList<Laboratory> Laboratories, int TotalCount)> ListLaboratoriesPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null)
    {
        return _learningSpaceRepository.ListLaboratoriesPagedAsync(pageNumber, pageSize, keyword);
    }

    /// <summary>
    /// Retrieves a paginated list of classrooms from the repository.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="keyword">The keyword to match classrooms' names.</param>
    /// <returns>A tuple containing the classrooms for the current page and the total count of classrooms.</returns>
    public Task<(IReadOnlyList<Classroom> Classrooms, int TotalCount)> ListClassroomsPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null)
    {
        return _learningSpaceRepository.ListClassroomsPagedAsync(pageNumber, pageSize, keyword);
    }
}
