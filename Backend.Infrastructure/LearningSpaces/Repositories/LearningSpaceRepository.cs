using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.LearningSpaces.Repositories;

/// <summary>
/// Implementation of a learning space repository interface.
/// </summary>
internal class LearningSpaceRepository : ILearningSpaceRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Constructor that initializes an instance of a LearningSpaceRepository
    /// </summary>
    /// <param name="dbContext"></param>
    public LearningSpaceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Asynchronous operation that lists all laboratories.
    /// </summary>
    /// <returns>Laboratory collection.</returns>
    public async Task<IEnumerable<Laboratory>> ListLaboratoriesAsync()
    {
        return await _dbContext.Laboratories.ToListAsync();
    }

    /// <summary>
    /// Asynchronous operation that lists all laboratories with paging.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="keyword">The keyword to match laboratories' names.</param>
    /// <returns>A tuple containing the paged laboratory collection and the total count of items.</returns>
    public async Task<(IReadOnlyList<Laboratory> Laboratories, int TotalCount)> ListLaboratoriesPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.LearningSpaces.OfType<Laboratory>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(l => l.RoomId.ToLower().Contains(keyword.ToLower()));
        }

        // Get total count before applying pagination
        var totalCount = await query.CountAsync();

        // Retrieve paged results
        var laboratories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (laboratories, totalCount);
    }

    /// <summary>
    /// Asynchronous operation that lists all classrooms with paging.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the paged classroom collection and the total count of items.</returns>
    public async Task<(IReadOnlyList<Classroom> Classrooms, int TotalCount)> ListClassroomsPagedAsync(
        int pageNumber,
        int pageSize,
        string? keyword = null)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.LearningSpaces.OfType<Classroom>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(c => c.RoomId.ToLower().Contains(keyword.ToLower()));
        }

        // Get total count before applying pagination
        var totalCount = await query.CountAsync();

        // Retrieve paged results
        var classrooms = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (classrooms, totalCount);
    }

    /// <summary>
    /// Asynchronous operation that adds a learning space.
    /// </summary>
    /// <param name="learningSpace">The learning space to add.</param>
    /// <returns>Asynchronous operation</returns>
    public async Task AddLearningSpaceAsync(LearningSpace learningSpace)
    {
        // Add the learning space to the DbSet
        await _dbContext.LearningSpaces.AddAsync(learningSpace);

        // Save changes with SQL exception handling
        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Asynchronous operation that lists all learning spaces by building ID.
    /// </summary>
    /// <param name="buildingId">The ID of the building to filter learning spaces by.</param>
    /// <returns>Learning space collection associated with the specified building ID.</returns>
    public async Task<IEnumerable<LearningSpace>> ListLearningSpacesByBuildingIdAsync(int buildingId)
    {
        return await _dbContext.LearningSpaces
            .Where(ls => ls.BuildingId == buildingId)
            .ToListAsync();
    }


    /// <summary>
    /// Asynchronous operation that deletes a learning space from the database.
    /// </summary>
    /// <param name="learningSpaceId">Unique identifier of the learning space to delete.</param>
    /// <returns>Asynchronous operation.</returns>
    /// <exception cref="LearningSpaceNotFoundException">
    /// Thrown when the specified learning space does not exist in the database.
    /// </exception>
    public async Task DeleteLearningSpaceAsync(int learningSpaceId)
    {
            var learningSpace = await _dbContext.LearningSpaces
                .FirstOrDefaultAsync(ls => ls.Id == learningSpaceId);

            if (learningSpace is null)
                throw new LearningSpaceNotFoundException(learningSpaceId);

            _dbContext.LearningSpaces.Remove(learningSpace);
            // Save changes with SQL exception handling
            await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());

    }

    /// <summary>
    /// Asynchronous operation that gets a laboratory by its ID.
    /// </summary>
    /// <param name="laboratoryId">The ID of the laboratory to retrieve.</param>
    /// <returns>Laboratory entity if found, null otherwise.</returns>
    /// <exception cref="LearningSpaceDataException">Thrown when a database operation fails.</exception>
    public async Task<Laboratory?> GetLaboratoryByIdAsync(int laboratoryId)
    {
        return await _dbContext.Laboratories.FirstOrDefaultAsync(l => l.Id == laboratoryId);
    }

    /// <summary>
    /// Asynchronous operation that gets a classroom by its ID.
    /// </summary>
    /// <param name="classroomId">The ID of the classroom to retrieve.</param>
    /// <returns>Classroom entity if found, null otherwise.</returns>
    /// <exception cref="LearningSpaceDataException">Thrown when a database operation fails.</exception>
    public async Task<Classroom?> GetClassroomByIdAsync(int classroomId)
    {
        return await _dbContext.Classrooms.FirstOrDefaultAsync(c => c.Id == classroomId);
    }

    /// <summary>
    /// Asynchronous operation that gets a learning space by its ID.
    /// This method retrieves any type of learning space (Laboratory, Classroom, etc.).
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space to retrieve.</param>
    /// <returns>LearningSpace entity if found, null otherwise.</returns>
    /// <exception cref="LearningSpaceDataException">Thrown when a database operation fails.</exception>
    public async Task<LearningSpace?> GetLearningSpaceByIdAsync(int learningSpaceId)
    {
        return await _dbContext.LearningSpaces.FirstOrDefaultAsync(ls => ls.Id == learningSpaceId);
    }

    /// <summary>
    /// Asynchronous operation that updates a laboratory specifically.
    /// This method is optimized for Laboratory entities and handles TPT inheritance properly.
    /// </summary>
    /// <param name="laboratory">The laboratory to update.</param>
    /// <returns>Asynchronous operation</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the laboratory is not found.</exception>
    /// <exception cref="DuplicateValueInEntityException">Thrown when a unique constraint is violated.</exception>
    /// <exception cref="ForeignKeyException">Thrown when a foreign key constraint is violated.</exception>
    public async Task UpdateLaboratoryAsync(Laboratory laboratory)
    {
        // Query specifically from the Laboratory DbSet to ensure proper TPT handling
        var existingLaboratory = await _dbContext.Laboratories
            .FirstOrDefaultAsync(l => l.Id == laboratory.Id);

        if (existingLaboratory is null)
        {
            throw new LearningSpaceNotFoundException(laboratory.Id);
        }

        // Determine if location properties should be updated by comparing values
        bool shouldUpdateBuildingId = !Equals(existingLaboratory.BuildingId, laboratory.BuildingId);
        bool shouldUpdateFloorLevel = !Equals(existingLaboratory.FloorLevel, laboratory.FloorLevel);

        // Update the properties of the existing laboratory
        existingLaboratory.Update(
            buildingId: laboratory.BuildingId,
            floorLevel: laboratory.FloorLevel,
            roomId: laboratory.RoomId,
            color: laboratory.Color,
            texture: laboratory.Texture,
            dimensions: laboratory.Dimensions,
            coordinates: laboratory.Coordinates,
            updateBuildingId: shouldUpdateBuildingId,  // Only update if value changed
            updateFloorLevel: shouldUpdateFloorLevel   // Only update if value changed
        );

        // Save changes with SQL exception handling
        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Asynchronous operation that lists all classrooms.
    /// </summary>
    /// <returns>Classroom collection.</returns>
    public async Task<IEnumerable<Classroom>> ListClassroomsAsync()
    {
        return await _dbContext.Classrooms.ToListAsync();
    }

    /// <summary>
    /// Asynchronous operation that updates a classroom specifically.
    /// This method is optimized for classroom entities and handles TPT inheritance properly.
    /// </summary>
    /// <param name="classroom">The classroom to update.</param>
    /// <returns>Asynchronous operation</returns>
    /// <exception cref="LearningSpaceNotFoundException">Thrown when the classroom is not found.</exception>
    /// <exception cref="DuplicateValueInEntityException">Thrown when a unique constraint is violated.</exception>
    /// <exception cref="ForeignKeyException">Thrown when a foreign key constraint is violated.</exception>
    public async Task UpdateClassroomAsync(Classroom classroom)
    {
        // Query specifically from the classroom DbSet to ensure proper TPT handling
        var existingClassroom = await _dbContext.Classrooms
            .FirstOrDefaultAsync(l => l.Id == classroom.Id);

        if (existingClassroom is null)
        {
            throw new LearningSpaceNotFoundException(classroom.Id);
        }

        // Determine if location properties should be updated by comparing values
        bool shouldUpdateBuildingId = !Equals(existingClassroom.BuildingId, classroom.BuildingId);
        bool shouldUpdateFloorLevel = !Equals(existingClassroom.FloorLevel, classroom.FloorLevel);

        // Update the properties of the existing classroom
        existingClassroom.Update(
            buildingId: classroom.BuildingId,
            floorLevel: classroom.FloorLevel,
            roomId: classroom.RoomId,
            color: classroom.Color,
            texture: classroom.Texture,
            dimensions: classroom.Dimensions,
            coordinates: classroom.Coordinates,
            updateBuildingId: shouldUpdateBuildingId,  // Only update if value changed
            updateFloorLevel: shouldUpdateFloorLevel   // Only update if value changed
        );

        // Save changes with SQL exception handling
        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Asynchronous operation that lists all LearningSpace Textures.
    /// </summary>
    /// <returns>Textures collection.</returns>
    public async Task<IEnumerable<LearningSpaceTexture>> ListLearningSpaceTexturesAsync()
    {
        return await _dbContext.LearningSpaceTextures.ToListAsync();
    }
}
