using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;

/// <summary>
/// Defines the contract for managing <see cref="InteractiveComponent"/> entities, specifically <see cref="Board"/> instances,
/// in a persistence layer.
/// </summary>
/// <remarks>
/// Implementations of this interface are responsible for performing CRUD operations on interactive components
/// and ensuring domain constraints, such as:
/// <list type="bullet">
/// <item><description>Unique PlateId constraints.</description></item>
/// <item><description>Foreign key integrity with LearningSpace entities.</description></item>
/// <item><description>Concurrency handling for updates.</description></item>
/// </list>
/// This interface allows the service layer to interact with the data layer without being coupled to a specific persistence mechanism.
/// </remarks>
public interface IInteractiveComponentRepository
{
    /// <summary>
    /// Adds a new <see cref="InteractiveComponent"/> to the database asynchronously.
    /// </summary>
    /// <param name="component">The interactive component to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="component"/> is null.</exception>
    /// <exception cref="PlateIdAlreadyExistsException">Thrown if a component with the same PlateId already exists.</exception>
    /// <exception cref="LearningSpaceIdDoesNotExistException">Thrown if the associated LearningSpaceId does not exist in the database.</exception>
    Task AddInteractiveComponentAsync(InteractiveComponent component);

    /// <summary>
    /// Retrieves a <see cref="Board"/> entity by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to retrieve. Must not be null or whitespace.</param>
    /// <returns>
    /// The matching <see cref="Board"/> if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="plateId"/> is null, empty, or whitespace.</exception>
    Task<Board?> ReadBoardByPlateIdAsync(string plateId);

    /// <summary>
    /// Updates an existing <see cref="InteractiveComponent"/> in the database asynchronously.
    /// </summary>
    /// <param name="component">The component with updated values. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="component"/> is null.</exception>
    /// <exception cref="BoardNotFoundException">Thrown if the component to update does not exist in the database.</exception>
    /// <exception cref="PlateIdAlreadyExistsException">Thrown if the PlateId conflicts with another existing component.</exception>
    /// <exception cref="LearningSpaceIdDoesNotExistException">Thrown if the associated LearningSpaceId does not exist.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown if the component was modified by another process during update.</exception>
    Task UpdateInteractiveComponentAsync(InteractiveComponent component);

    /// <summary>
    /// Deletes a <see cref="Board"/> from the database by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if the board does not exist in the database.</exception>
    /// <exception cref="Exception">Thrown if a database error occurs during deletion.</exception>
    Task DeleteBoardAsync(string plateId);

    /// <summary>
    /// Retrieves all <see cref="Board"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all boards.</returns>
    Task<IEnumerable<Board>> ListAllBoardsAsync();

    /// <summary>
    /// Filters <see cref="Board"/> entities with pagination across multiple fields.
    /// </summary>
    /// <param name="searchTerm">The search term to filter boards. If null or empty, returns all boards.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>
    /// A tuple containing the filtered list of boards and the total count of matching boards.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    Task<(IEnumerable<Board> Boards, int TotalCount)> FilterBoardsAsync(string searchTerm, int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paginated list of <see cref="Board"/> entities from the database.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>
    /// A tuple containing the paginated list of boards (<see cref="IEnumerable{Board}"/>) and the total count of all boards.
    /// </returns>
    Task<(IEnumerable<Board> Boards, int TotalCount)> ListBoardsPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves all <see cref="Projector"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all projectors.</returns>
    Task<IEnumerable<Projector>> ListAllProjectorsAsync();

    /// <summary>
    /// Retrieves a paginated list of projectors along with the total count of available projectors.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of projectors to include in each page. Must be greater than 0.</param>
    /// <returns>
    /// A tuple containing the paginated list of projectors and the total count of projectors.
    /// </returns>
    Task<(IEnumerable<Projector> Projectors, int TotalCount)> ListProjectorsPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paginated list of projectors that match the specified search term.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. Only projectors containing this term will be included in the results.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of projectors to include in each page. Must be greater than or equal to 1.</param>
    /// <returns>
    /// A tuple containing the filtered list of projectors and the total count of matching projectors.
    /// </returns>
    Task<(IEnumerable<Projector> Projectors, int TotalCount)> FilterProjectorsAsync(string searchTerm, int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves all <see cref="InteractiveComponent"/> entities associated with a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId"> The unique identifier of the learning space.</param>
    /// <returns></returns>
    Task<IEnumerable<InteractiveComponent>> GetInteractiveComponentsByLearningSpaceAsync(int learningSpaceId);
}
