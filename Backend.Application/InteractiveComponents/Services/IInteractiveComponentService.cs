using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services;

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
    /// <param name="plateId">Unique identifier for the board within the system.</param>
    /// <param name="x">X-coordinate of the board in the learning space.</param>
    /// <param name="y">Y-coordinate of the board in the learning space.</param>
    /// <param name="z">Z-coordinate of the board in the learning space.</param>
    /// <param name="width">Width of the board in meters (or unit of choice).</param>
    /// <param name="height">Height of the board in meters.</param>
    /// <param name="depth">Depth of the board in meters.</param>
    /// <param name="xAxisRotation">Rotation of the board around the X-axis in degrees.</param>
    /// <param name="yAxisRotation">Rotation of the board around the Y-axis in degrees.</param>
    /// <param name="zAxisRotation">Rotation of the board around the Z-axis in degrees.</param>
    /// <param name="learningSpaceId">Identifier of the learning space this board belongs to.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the newly created <see cref="Board"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="texture"/> is null, empty, or whitespace.</exception>
    Task<Board> CreateBoardAsync(
        string colorValue,
        string markerColorValue,
        string texture,
        string plateId,
        double x,
        double y,
        double z,
        double width,
        double height,
        double depth,
        double xAxisRotation,
        double yAxisRotation,
        double zAxisRotation,
        int learningSpaceId
    );

    /// <summary>
    /// Retrieves a <see cref="Board"/> by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to retrieve.</param>
    /// <returns>
    /// A task containing the <see cref="Board"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Board?> ReadBoardByPlateIdAsync(string plateId);

    /// <summary>
    /// Updates an existing board by creating a new <see cref="Board"/> instance with the provided attributes
    /// and delegating the update to the repository.
    /// </summary>
    /// <param name="colorValue">Hex or named string representing the main color of the board.</param>
    /// <param name="markerColorValue">Hex or named string representing the marker color of the board.</param>
    /// <param name="texture">Surface texture or material pattern for the board. Cannot be null, empty, or whitespace.</param>
    /// <param name="plateId">Unique identifier for the board to update.</param>
    /// <param name="x">X-coordinate of the board in the learning space.</param>
    /// <param name="y">Y-coordinate of the board in the learning space.</param>
    /// <param name="z">Z-coordinate of the board in the learning space.</param>
    /// <param name="width">Width of the board in meters.</param>
    /// <param name="height">Height of the board in meters.</param>
    /// <param name="depth">Depth of the board in meters.</param>
    /// <param name="xAxisRotation">Rotation of the board around the X-axis in degrees.</param>
    /// <param name="yAxisRotation">Rotation of the board around the Y-axis in degrees.</param>
    /// <param name="zAxisRotation">Rotation of the board around the Z-axis in degrees.</param>
    /// <param name="learningSpaceId">Identifier of the learning space this board belongs to.</param>
    /// <returns>
    /// A task containing the updated <see cref="Board"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="texture"/> is null, empty, or whitespace.</exception>
    Task<Board> UpdateBoardAsync(
        string colorValue,
        string markerColorValue,
        string texture,
        string plateId,
        double x,
        double y,
        double z,
        double width,
        double height,
        double depth,
        double xAxisRotation,
        double yAxisRotation,
        double zAxisRotation,
        int learningSpaceId
    );

    /// <summary>
    /// Deletes a board identified by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteBoardAsync(string plateId);

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
    /// <param name="xAxisRotation">Rotation of the projector around the X-axis in degrees.</param>
    /// <param name="yAxisRotation">Rotation of the projector around the Y-axis in degrees.</param>
    /// <param name="zAxisRotation">Rotation of the projector around the Z-axis in degrees.</param>
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
        int resWidth,   // resolution
        int resHeight,  // resolution
        double x,
        double y,
        double z,
        double width,  // dimensions
        double height, // dimensions
        double depth,  // dimensions
        double xAxisRotation,
        double yAxisRotation,
        double zAxisRotation,
        int learningSpaceId);

    /// <summary>
    /// Retrieves all <see cref="Projector"/> instances from the repository.
    /// </summary>
    /// <returns>A task containing a collection of all projectors in the system.</returns>
    Task<IEnumerable<Projector>> ListAllProjectorsAsync();

    /// <summary>
    /// Retrieves a paginated list of boards from the repository.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the boards for the current page and the total count of boards</returns>
    Task<(IEnumerable<Board> Boards, int TotalCount)> ListBoardsPagedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a filtered list of boards based on the specified search term, with support for pagination.
    /// </summary>
    /// <param name="searchTerm">The term used to filter boards. Only boards matching the search term will be included. Can be empty or null to
    /// retrieve all boards.</param>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include per page. Must be greater than 0.</param>
    /// <returns>A tuple containing the filtered list of boards and the total count of boards matching the search criteria. The
    /// <see cref="IEnumerable{Board}"/> represents the boards for the specified page, and <c>TotalCount</c> represents
    /// the total number of matching boards.</returns>
    Task<(IEnumerable<Board> Boards, int TotalCount)> FilterBoardsAsync(
        string searchTerm, 
        int pageNumber, 
        int pageSize);

    /// <summary>
    /// Retrieves a paginated list of projectors that match the specified search term.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. Only projectors whose properties match the term will be included. Can be
    /// empty or null to retrieve all projectors.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of projectors to include per page. Must be greater than 0.</param>
    /// <returns>A tuple containing the filtered list of projectors and the total count of projectors that match the search term.
    /// The first item in the tuple is an <see cref="IEnumerable{T}"/> of <see cref="Projector"/> representing the
    /// projectors on the requested page. The second item is an <see cref="int"/> representing the total number of
    /// projectors matching the search term across all pages.</returns>
    Task<(IEnumerable<Projector> Projectors, int TotalCount)> FilterProjectorsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize);
}

    
