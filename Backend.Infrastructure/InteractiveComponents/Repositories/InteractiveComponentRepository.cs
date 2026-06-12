using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="InteractiveComponent"/> entities, specifically <see cref="Board"/> instances,
/// in the persistence layer using Entity Framework Core.
/// </summary>
/// <remarks>
/// This repository handles CRUD operations and enforces domain-specific constraints such as:
/// <list type="bullet">
/// <item><description>Unique PlateId constraints.</description></item>
/// <item><description>Foreign key integrity with LearningSpace entities.</description></item>
/// <item><description>Concurrency checks during updates.</description></item>
/// </list>
/// Exception handling ensures that database-specific errors are translated into meaningful domain exceptions.
/// </remarks>
internal class InteractiveComponentRepository : IInteractiveComponentRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of <see cref="InteractiveComponentRepository"/>.
    /// </summary>
    /// <param name="dbContext">The EF Core database context for interacting with the database.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dbContext"/> is null.</exception>
    public InteractiveComponentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a new <see cref="InteractiveComponent"/> to the database asynchronously.
    /// </summary>
    /// <param name="component">The interactive component to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddInteractiveComponentAsync(InteractiveComponent component)
    {
        await _dbContext.InteractiveComponents.AddAsync(component);

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Retrieves a <see cref="Board"/> entity by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to retrieve. Must not be null or whitespace.</param>
    /// <returns>
    /// The matching <see cref="Board"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Board?> ReadBoardByPlateIdAsync(string plateId)
    {
        var plateIdValueObject = new PlateId(plateId);

        return await _dbContext.InteractiveComponents
            .OfType<Board>()
            .FirstOrDefaultAsync(b => b.PlateId.Equals(plateIdValueObject));
    }

    /// <summary>
    /// Updates an existing <see cref="InteractiveComponent"/> in the database asynchronously.
    /// </summary>
    /// <param name="component">The component with updated values. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if the component to update does not exist in the database.</exception>
    public async Task UpdateInteractiveComponentAsync(InteractiveComponent component)
    {

        var existingComponent = await _dbContext.InteractiveComponents
            .FirstOrDefaultAsync(ic => ic.PlateId.Equals(component.PlateId));

        if (existingComponent is null)
            throw new BoardNotFoundException(component.PlateId.Value);

        existingComponent.Update(component);

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Deletes a <see cref="Board"/> from the database by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if the board does not exist in the database.</exception>
    public async Task DeleteBoardAsync(string plateId)
    {
        var plateIdValueObject = new PlateId(plateId);

        var board = await _dbContext.InteractiveComponents
            .OfType<Board>()
            .FirstOrDefaultAsync(b => b.PlateId.Equals(plateIdValueObject));

        if (board is null)
            throw new BoardNotFoundException(plateId);

        _dbContext.InteractiveComponents.Remove(board);

        await SqlExceptionHandlingUtils.HandleSqlOperationAsync(() => _dbContext.SaveChangesAsync());
    }

    /// <summary>
    /// Retrieves all <see cref="Board"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all boards.</returns>
    public async Task<IEnumerable<Board>> ListAllBoardsAsync()
    {
        return await _dbContext.InteractiveComponents
            .OfType<Board>()
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all <see cref="Projector"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all projectors.</returns>
    public async Task<IEnumerable<Projector>> ListAllProjectorsAsync()
    {
        return await _dbContext.InteractiveComponents
            .OfType<Projector>()
            .ToListAsync();
    }
    
    /// <summary>
    /// Retrieves all <see cref="InteractiveComponent"/> entities associated with a specific LearningSpaceId.
    /// </summary>
    /// <returns>A collection of all interactive components in the specified learning space.</returns>
    public async Task<IEnumerable<InteractiveComponent>> GetInteractiveComponentsByLearningSpaceAsync(int learningSpaceId)
    {
        return await _dbContext.InteractiveComponents
            .Where(x => x.LearningSpaceId == learningSpaceId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of boards along with the total count of available boards.
    /// </summary>
    /// <remarks>This method queries the database to retrieve the specified page of boards. The total count is
    /// calculated before applying pagination, allowing the caller to determine the total number of pages
    /// available.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than zero.</param>
    /// <returns>A tuple containing the paginated list of boards and the total count of boards: <list type="bullet">
    /// <item><description><see cref="IEnumerable{Board}"/>: The collection of boards for the specified
    /// page.</description></item> <item><description><see cref="int"/>: The total count of boards
    /// available.</description></item> </list></returns>
    public async Task<(IEnumerable<Board> Boards, int TotalCount)> ListBoardsPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.InteractiveComponents.OfType<Board>();

        // Get total count before applying pagination
        var totalCount = await query.CountAsync();

        // Retrieve paged results
        var boards = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (boards, totalCount);

    }

    /// <summary>
    /// Retrieves a paginated list of boards that match the specified search criteria.
    /// </summary>
    /// <remarks>The search operation matches the <paramref name="searchTerm"/> against various string and
    /// numeric properties of the boards. Numeric properties are converted to strings for partial matching. Pagination
    /// is applied after filtering.</remarks>
    /// <param name="searchTerm">The term used to filter boards. The search is case-insensitive and matches against various properties of the
    /// boards. If <paramref name="searchTerm"/> is null, empty, or whitespace, no filtering is applied.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than zero.</param>
    /// <returns>A tuple containing the following: <list type="bullet"> <item> <description> <see cref="IEnumerable{Board}"/>:
    /// The collection of boards that match the search criteria for the specified page. </description> </item> <item>
    /// <description> <see cref="int"/>: The total number of boards that match the search criteria across all pages.
    /// </description> </item> </list></returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1 or <paramref name="pageSize"/> is less than 1.</exception>
    public async Task<(IEnumerable<Board> Boards, int TotalCount)> FilterBoardsAsync(string searchTerm, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.InteractiveComponents.OfType<Board>();
        
        // If there is no search term, filter and paginate in the database
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            var totalCount = await query.CountAsync();

            var boards = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (boards, totalCount);
        }

        // Load LearningSpace and Building to filter in memory by RoomId and Building.Name
        var allBoards = await query
            .Include(b => b.LearningSpace)
            .ThenInclude(ls => ls.Building)
            .ToListAsync();

        var filtered = allBoards.Where(b =>
            (b.PlateId?.Value != null && b.PlateId.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (b.Color?.Value != null && b.Color.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (b.MarkerColor?.Value != null && b.MarkerColor.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (b.Texture != null && b.Texture.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || b.Coordinates?.X != null && b.Coordinates.X.ToString().Contains(searchTerm)
            || b.Coordinates?.Y != null && b.Coordinates.Y.ToString().Contains(searchTerm)
            || b.Coordinates?.Z != null && b.Coordinates.Z.ToString().Contains(searchTerm)
            || b.Dimensions?.Height != null && b.Dimensions.Height.ToString().Contains(searchTerm)
            || b.Dimensions?.Width != null && b.Dimensions.Width.ToString().Contains(searchTerm)
            || b.Dimensions?.Depth != null && b.Dimensions.Depth.ToString().Contains(searchTerm)
            || b.Rotations?.XAxisRotation != null && b.Rotations.XAxisRotation.ToString().Contains(searchTerm)
            || b.Rotations?.YAxisRotation != null && b.Rotations.YAxisRotation.ToString().Contains(searchTerm)
            || b.Rotations?.ZAxisRotation != null && b.Rotations.ZAxisRotation.ToString().Contains(searchTerm)
            || (b.LearningSpace != null && b.LearningSpace.RoomId != null && b.LearningSpace.RoomId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (b.LearningSpace?.Building?.Name?.Value != null && b.LearningSpace.Building.Name.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        );

        var totalFiltered = filtered.Count();

        var paged = filtered
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (paged, totalFiltered);
    }

    /// <summary>
    /// Retrieves a paginated list of projectors along with the total count of available projectors.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of projectors to include in each page. Must be greater than zero.</param>
    /// <returns>
    /// A tuple containing the paginated list of projectors and the total count of projectors.
    /// </returns>
    public async Task<(IEnumerable<Projector> Projectors, int TotalCount)> ListProjectorsPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.InteractiveComponents.OfType<Projector>();

        var totalCount = await query.CountAsync();

        var projectors = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (projectors, totalCount);
    }

    /// <summary>
    /// Retrieves a paginated list of projectors that match the specified search criteria.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. The search is case-insensitive and matches against various properties.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than zero.</param>
    /// <param name="pageSize">The number of projectors to include in each page. Must be greater than zero.</param>
    /// <returns>
    /// A tuple containing the filtered list of projectors and the total count of matching projectors.
    /// </returns>
    public async Task<(IEnumerable<Projector> Projectors, int TotalCount)> FilterProjectorsAsync(string searchTerm, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        var query = _dbContext.InteractiveComponents.OfType<Projector>();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            var totalCount = await query.CountAsync();

            var projectors = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (projectors, totalCount);
        }

        var allProjectors = await query
            .Include(p => p.LearningSpace)
            .ThenInclude(ls => ls.Building)
            .ToListAsync();

        var filtered = allProjectors.Where(p =>
            (p.PlateId?.Value != null && p.PlateId.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (p.Color?.Value != null && p.Color.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (p.Texture != null && p.Texture.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || p.Brightness.ToString().Contains(searchTerm)
            || p.ProjectionResolution?.Width != null && p.ProjectionResolution.Width.ToString().Contains(searchTerm)
            || p.ProjectionResolution?.Height != null && p.ProjectionResolution.Height.ToString().Contains(searchTerm)
            || p.Coordinates?.X != null && p.Coordinates.X.ToString().Contains(searchTerm)
            || p.Coordinates?.Y != null && p.Coordinates.Y.ToString().Contains(searchTerm)
            || p.Coordinates?.Z != null && p.Coordinates.Z.ToString().Contains(searchTerm)
            || p.Dimensions?.Height != null && p.Dimensions.Height.ToString().Contains(searchTerm)
            || p.Dimensions?.Width != null && p.Dimensions.Width.ToString().Contains(searchTerm)
            || p.Dimensions?.Depth != null && p.Dimensions.Depth.ToString().Contains(searchTerm)
            || p.Rotations?.XAxisRotation != null && p.Rotations.XAxisRotation.ToString().Contains(searchTerm)
            || p.Rotations?.YAxisRotation != null && p.Rotations.YAxisRotation.ToString().Contains(searchTerm)
            || p.Rotations?.ZAxisRotation != null && p.Rotations.ZAxisRotation.ToString().Contains(searchTerm)
            || (p.LearningSpace != null && p.LearningSpace.RoomId != null && p.LearningSpace.RoomId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            || (p.LearningSpace?.Building?.Name?.Value != null && p.LearningSpace.Building.Name.Value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
        );

        var totalFiltered = filtered.Count();

        var paged = filtered
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (paged, totalFiltered);
    }
}
