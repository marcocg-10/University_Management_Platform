using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Repositories;

/// <summary>
/// Defines the contract for managing <see cref="InteractiveComponent"/> entities.
/// </summary>
/// <remarks>
/// Implementations of this interface are responsible for performing CRUD operations on interactive components.
/// This interface allows the service layer to interact with the data layer without being coupled to a specific persistence mechanism.
/// </remarks>
public interface IInteractiveComponentRepository
{
    /// <summary>
    /// Adds a new <see cref="Board"/> to the database asynchronously.
    /// </summary>
    /// <param name="board">The board to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="board"/> is null.</exception>
    Task AddBoardAsync(Board board);

    /// <summary>
    /// Retrieves all <see cref="Board"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all boards.</returns>
    Task<IEnumerable<Board>> ListAllBoardsAsync();

    /// <summary>
    /// Adds a new <see cref="Projector"/> to the database asynchronously.
    /// </summary>
    /// <param name="projector">The projector to add. Must not be null.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddProjectorAsync(Projector projector);

    /// <summary>
    /// Retrieves all <see cref="Projector"/> entities from the database asynchronously.
    /// </summary>
    /// <returns>A collection of all projectors.</returns>
    Task<IEnumerable<Projector>> ListAllProjectorsAsync();

    /// <summary>
    /// Deletes a <see cref="Board"/> from the database by its <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if the board does not exist in the database.</exception>
    Task DeleteBoardAsync(string plateId);

    /// <summary>
    /// Updates the specified board with the latest data.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> object containing the updated data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateBoardAsync(Board board);

    /// <summary>
    /// Retrieves a paginated list of boards along with pagination metadata.
    /// </summary>
    /// <remarks>Use this method to retrieve boards in a paginated manner. Ensure that <paramref
    /// name="pageNumber"/> and <paramref name="pageSize"/>  are within valid ranges to avoid unexpected
    /// results.</remarks>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Board}"/> representing the boards in the requested
    /// page.</description> </item> <item> <description><see cref="PaginationMetadata"/> containing information about
    /// the total number of items and pages.</description> </item> </list></returns>
    Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> ListBoardsPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a filtered list of boards based on the specified search term, with support for pagination.
    /// </summary>
    /// <remarks>Use this method to retrieve boards in a paginated manner, particularly when working with
    /// large datasets. If no boards match the search term, the returned collection will be empty.</remarks>
    /// <param name="searchTerm">The term used to filter boards. Only boards containing this term in their relevant fields will be included.</param>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include per page. Must be greater than or equal to 1.</param>
    /// <returns>A tuple containing the filtered list of boards and the associated pagination metadata. <list type="bullet">
    /// <item><description><c>Boards</c>: An enumerable collection of boards matching the search
    /// criteria.</description></item> <item><description><c>Metadata</c>: Information about the pagination, such as
    /// total items and total pages.</description></item> </list></returns>
    Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> FilterBoardsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
        );
    
    /// <summary>
    /// Retrieves a filtered list of projectors based on the specified search term, with support for pagination.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. Only projectors matching the term will be included in the results.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of projectors to include per page. Must be greater than 0.</param>
    /// <returns>A tuple containing the filtered list of projectors and the associated pagination metadata. The <see
    /// cref="PaginationMetadata"/> provides details about the total number of items and pages.</returns>
    Task<(IEnumerable<Projector> Projectors, PaginationMetadata Metadata)> FilterProjectorsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
        );
}
