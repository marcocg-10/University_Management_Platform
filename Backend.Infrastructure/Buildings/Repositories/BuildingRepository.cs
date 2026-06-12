using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.Repositories;

/// <summary>
/// Represents a repository for managing building entities in the infrastructure layer.
/// </summary>
/// <remarks>  This class provides methods to access an manage building data using Entity Framework Core.
/// It implements the <see cref="IBuildingRepository"/> interface and interacts with the application's database context.
/// </remarks>
internal class BuildingRepository : IBuildingRepository
{
    /// <summary>
    /// The application's database context used for data access.
    /// </summary>
    /// <remarks>
    /// This field is used to interact with the database and perform CRUD operations on building entities.
    /// </remarks>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The application's 
    public BuildingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Asynchronously retrieves all building entities.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Building"/> entities.
    /// </returns>
    public async Task<IEnumerable<Building>> GetBuildingsAsync()
    {
        return await _dbContext.Building.Include(Building => Building.RenderInfo).ToListAsync();
    }

    /// <summary>
    /// Asynchronously adds a new building and its render info to the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added <see cref="Building"/> entity.
    /// </returns> 
    public async Task<Building> AddBuildingAsync(Building building)
    {
        try
        {
            // Attempt to add the building to the EF Core DbSet asynchronously
            await _dbContext.Building.AddAsync(building);

            // Save the changes to the database asynchronously
            await _dbContext.SaveChangesAsync();
        }
        // Check if the exception is due to a unique constraint violation
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            throw new DuplicateBuildingException(building.OfficialId, building.Name);
        }
        catch (Exception exception)
        {
            // Catch any other unexpected exceptions and wrap them in a building data exception
            throw new BuildingDataException(
                "An unexpected error occurred while adding the building.", exception);
        }

        // If everything went well, return the building that was added
        return building;
    }

    /// <summary>
    /// Asynchronously updates an existing building entity in the database.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity to update.</param>
    /// <exception cref="BuildingDataException"></exception>
    /// <exception cref="DuplicateBuildingException"></exception>
    public async Task UpdateBuildingAsync(Building building)
    {
        try
        {
            var buildingExists = await _dbContext.Building
            .Include(b => b.RenderInfo)
            .FirstOrDefaultAsync(b => b.OfficialId == building.OfficialId);

            if (buildingExists == null)
            {
                throw new BuildingDataException($"Building with OfficialId '{building.OfficialId}' not found.", null);
            }
            buildingExists.UpdateBuilding(building);

            // Save the changes to the database asynchronously
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            // If there is a database update error, check if it is a unique constraint violation
            if (exception.InnerException?.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) == true ||
                exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new DuplicateBuildingException(building.OfficialId, building.Name);
            }
            // For other database errors, throw a generic building data exception
            throw new BuildingDataException(
                "An error occurred while updating the building in the database.", exception);
        }
        // Catch any other unexpected exceptions and wrap them in a building data exception
        catch (Exception exception)
        {
            throw new BuildingDataException(
                "An unexpected error occurred while updating the building.", exception);
        }

    }

    /// <summary>
    /// Asynchronously deletes a building entity by its official ID.
    /// </summary>
    /// <param name="officialId">The official ID of the building to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="BuildingDataException">Thrown when the building is not found.</exception>
    public async Task DeleteBuildingAsync(string officialId)
    {
        try
        {
            var officialIdValueObject = BuildingOfficialId.Create(officialId);
            var buildingExists = await _dbContext.Building
                .Include(b => b.RenderInfo)
                .FirstOrDefaultAsync(b => b.OfficialId == officialIdValueObject);
            if (buildingExists is null)
            {
                throw new BuildingDataException($"Building with OfficialId '{officialId}' not found.", null);
            }
            if (buildingExists.RenderInfo is not null)
            {
                _dbContext.Remove(buildingExists.RenderInfo);
            }
            _dbContext.Building.Remove(buildingExists);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            throw new BuildingDataException("An error occurred while deleting the building from the database.", exception);
        }
        catch (Exception exception)
        {
            throw new BuildingDataException("An unexpected error occurred while deleting the building.", exception);
        }
    }

    /// <summary>
    /// Determines if a <see cref="DbUpdateException"/> was caused by a unique constraint violation.
    /// </summary>
    /// <param name="exception">The exception thrown during a database update or add operation.</param>
    /// <returns><c>true</c> if it is a unique constraint violation; otherwise, <c>false</c>.</returns>
    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlEx &&
               (sqlEx.Number == 2627 || sqlEx.Number == 2601);
    }
}
