using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
namespace UCR.ECCI.PI.ThemePark.Frontend.Application.InteractiveComponents.Services;

/// <summary>
/// Defines the contract for operations related to interactive components within the Theme Park domain,
/// specifically for managing <see cref="Board"/> and <see cref="Projector"/> entities.
/// </summary>
public interface IInteractiveComponentService
{
    /// <summary>
    /// Creates a new <see cref="Board"/> instance, wrapping the input attributes in domain value objects.
    /// </summary>
    /// <param name="colorValue">Hex or named string representing the main color of the board.</param>
    /// <param name="markerColorValue">Hex or named string representing the marker color of the board.</param>
    /// <param name="texture">Surface texture or material pattern for the board. Cannot be null, empty, or whitespace.</param>
    /// <param name="plateIdValue">Unique identifier for the board within the system.</param>
    /// <param name="x">X-coordinate of the board in the learning space.</param>
    /// <param name="y">Y-coordinate of the board in the learning space.</param>
    /// <param name="z">Z-coordinate of the board in the learning space.</param>
    /// <param name="width">Width of the board in meters (or unit of choice).</param>
    /// <param name="height">Height of the board in meters.</param>
    /// <param name="depth">Depth of the board in meters.</param>
    /// <param name="XAxisRotation"> Rotation in X axis. </param>
    /// <param name="YAxisRotation"> Rotation in Y axis. </param>
    /// <param name="ZAxisRotation"> Rotation in Z axis. </param>
    /// <param name="learningSpaceId">Identifier of the learning space this board belongs to.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the newly created <see cref="Board"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="texture"/> is null, empty, or whitespace.</exception> 
    Task<Board> CreateBoardAsync(
        string colorValue,
        string markerColorValue,
        string texture,
        string plateIdValue,
        double x,
        double y,
        double z,
        double width,
        double height,
        double depth,
        double XAxisRotation,
        double YAxisRotation,
        double ZAxisRotation,
        int learningSpaceId
    );

    /// <summary>
    /// Retrieves all <see cref="Board"/> instances from the repository.
    /// </summary>
    /// <returns>A task containing a collection of all boards in the system.</returns>
    Task<IEnumerable<Board>> ListAllBoardsAsync();

    /// <summary>
    /// Creates a new <see cref="Projector"/> instance, wrapping the input attributes in domain value objects.
    /// </summary>
    /// <param name="colorValue">Hex or named string representing the main color of the projector</param>
    /// <param name="texture">Surface texture or material pattern for the projector. Cannot be null, empty, or whitespace</param>
    /// <param name="brightness">Value that indicates how bright the projection will be</param>
    /// <param name="plateId">Unique identifier for the projector within the system </param>
    /// <param name="resWidth">Width of the pixel resolution of the projection</param>
    /// <param name="resHeight">Height of the pixel resolution of the projection</param>
    /// <param name="x">X-Coordinate of the projector in the learning space</param>
    /// <param name="y">Y-Coordinate of the projector in the learning space</param>
    /// <param name="z">Z-Coordinate of the projector in the learning space</param>
    /// <param name="width">Width of the projector in meters</param>
    /// <param name="height">Height of the projector in meters</param>
    /// <param name="depth">Depth of the projector in meters</param>
    /// <param name="XAxisRotation"> Rotation in X axis. </param>
    /// <param name="YAxisRotation"> Rotation in Y axis. </param>
    /// <param name="ZAxisRotation"> Rotation in Z axis. </param>
    /// <param name="learningSpaceId">Identifier of the learning space this projector belongs to</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the newly created <see cref="Projector"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="texture"/> is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="brightness"/> is less than 0 or greater than 100</exception>
    Task<Projector> CreateProjectorAsync(
        string colorValue,
        string texture,
        int brightness,
        string plateId,
        int resWidth, // resolution
        int resHeight, // resolution
        double x,
        double y,
        double z,
        double width,   // dimensions
        double height,  // dimensions
        double depth,   // dimensions
        double XAxisRotation,
        double YAxisRotation,
        double ZAxisRotation,
        int learningSpaceId);

    /// <summary>
    /// Retrieves all <see cref="Projector"/> instances from the repository.
    /// </summary>
    /// <returns> A task containing a collection of all projectors in the system.</returns>
    Task<IEnumerable<Projector>> ListAllProjectorsAsync();

    /// <summary>
    /// Deletes a board identified by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="BoardNotFoundException">Thrown if no board with the specified <paramref name="plateId"/> exists.</exception>
    Task DeleteBoardAsync(string plateId);

    /// <summary>
    /// Updates the specified board with the latest data.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> object containing the updated data. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateBoardAsync(Board board);


    /// <summary>
    /// Retrieves a paginated list of boards along with pagination metadata.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Board}"/> representing the boards in the requested
    /// page.</description> </item> <item> <description><see cref="PaginationMetadata"/> containing information about
    /// the total number of items and pages.</description> </item> </list></returns>
    Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> ListBoardsPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paginated list of boards that match the specified search term.
    /// </summary>
    /// <param name="searchTerm">The term used to filter boards. Only boards containing this term in searchable fields (PlateId, Color, RoomId, Building Name, coordinates, rotations) will be included.
    /// Can be empty or null to retrieve all boards.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include per page. Must be greater than 0.</param>
    /// <returns>A tuple containing a collection of <see cref="Board"/> objects that match the search criteria and  a <see
    /// cref="PaginationMetadata"/> object providing details about the pagination state.</returns>
    Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> FilterBoardsAsync(
        string searchTerm, 
        int pageNumber, 
        int pageSize);

    Task<(IEnumerable<Projector> Projectors, PaginationMetadata Metadata)> FilterProjectorsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize);
}
