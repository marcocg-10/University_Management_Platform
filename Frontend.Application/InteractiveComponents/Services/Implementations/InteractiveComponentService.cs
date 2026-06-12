using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.InteractiveComponents.Services.Implementations;

/// <summary>
/// Service layer implementation responsible for managing <see cref="InteractiveComponent"/> entities,
/// specifically <see cref="Board"/> and <see cref="Projector"/> types.
/// </summary>
/// <remarks>
/// This service provides methods for creating, reading, updating, deleting, listing boards and the 
/// creation of boards and projectors.
/// It acts as a mediator between the application layer and the persistence layer, ensuring 
/// that domain rules are enforced when performing operations.
/// </remarks>
internal class InteractiveComponentService : IInteractiveComponentService
{
    private readonly IInteractiveComponentRepository _interactiveComponentRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="InteractiveComponentService"/>.
    /// </summary>
    /// <param name="interactiveComponentRepository">
    /// Repository responsible for persisting and retrieving interactive components.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentRepository"/> is null.</exception>
    public InteractiveComponentService(IInteractiveComponentRepository interactiveComponentRepository)
    {
        _interactiveComponentRepository = interactiveComponentRepository
            ?? throw new ArgumentNullException(nameof(interactiveComponentRepository));
    }

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
    public async Task<Board> CreateBoardAsync(
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
        int learningSpaceId)
    {
        var color = new Color(colorValue);
        var markerColor = new Color(markerColorValue);

        if (string.IsNullOrWhiteSpace(texture))
            throw new ArgumentException("Texture cannot be null, empty, or whitespace.");

        var coordinates = new Coordinates(x, y, z);
        var dimensions = new Dimensions(width, height, depth);
        var rotations = new Rotations(XAxisRotation, YAxisRotation, ZAxisRotation);
        var plateId = new PlateId(plateIdValue);

        var board = new Board(color, markerColor, texture, plateId, coordinates, dimensions, rotations, learningSpaceId);

        await _interactiveComponentRepository.AddBoardAsync(board);

        return board;
    }

    /// <summary>
    /// Retrieves all <see cref="Board"/> instances from the repository.
    /// </summary>
    /// <returns>A task containing a collection of all boards in the system.</returns>
    public async Task<IEnumerable<Board>> ListAllBoardsAsync()
    {
        return await _interactiveComponentRepository.ListAllBoardsAsync();
    }

    /// <summary>
    /// Creates a new Projector instance, wrapping the input attributes in domain value objects.
    /// </summary>
    /// <param name="colorValue">Hex or named string representing the main color of the projector.</param>
    /// <param name="texture">Surface texture or material pattern for the projector. Cannot be null,
    /// empty, or whitespace.</param>
    /// <param name="brightness">Value of the level of brightness of the projection, cannot be less than 0 or more
    /// than 100.</param>
    /// <param name="plateId">Unique identifier for the projector within the system.</param>
    /// <param name="resWidth">Width of the pixel resolution of the projection.</param>
    /// <param name="resHeight">Height of the pixel resolution of the projection.</param>
    /// <param name="x">X-coordinate of the projector in the learning space.</param>
    /// <param name="y">Y-coordinate of the projector in the learning space.</param>
    /// <param name="z">Z-coordinate of the projector in the learning space.</param>
    /// <param name="width">Width of the projector in meters.</param>
    /// <param name="height">Height of the projector in meters.</param>
    /// <param name="depth">Depth of the projector in meters.</param>
    /// <param name="XAxisRotation"> Rotation in X axis. </param>
    /// <param name="YAxisRotation"> Rotation in Y axis. </param>
    /// <param name="ZAxisRotation"> Rotation in Z axis. </param>
    /// <param name="learningSpaceId">Identifier of the learning space this projector belongs to.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the newly created <see cref="Projector"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="texture"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="brightness"/> is less than 0 or greater than 100.</exception>
    public async Task<Projector> CreateProjectorAsync(
        string colorValue,
        string texture,
        int brightness,
        string plateId,
        int resWidth,
        int resHeight,
        double x,
        double y,
        double z,
        double width,
        double height,
        double depth,
        double XAxisRotation,
        double YAxisRotation,
        double ZAxisRotation,
        int learningSpaceId)
    {
        var color = new Color(colorValue);

        if (string.IsNullOrWhiteSpace(texture))
            throw new ArgumentException("Texture cannot be null, empty, or whitespace.");

        if (brightness < 0 || brightness > 100)
            throw new ArgumentException("Brightness is out of valid range [0, 100].");

        var resolution = new Resolution(resWidth, resHeight);
        var coordinates = new Coordinates(x, y, z);
        var dimensions = new Dimensions(width, height, depth);
        var rotations = new Rotations(XAxisRotation, YAxisRotation, ZAxisRotation);
        var plateIdValueObject = new PlateId(plateId);

        var projector = new Projector(
            color,
            texture,
            brightness,
            plateIdValueObject,
            resolution,
            coordinates,
            dimensions,
            rotations,
            learningSpaceId);

        await _interactiveComponentRepository.AddProjectorAsync(projector);

        return projector;
    }

    /// <summary>
    /// Retrieves all <see cref="Projector"/> instances from the repository.
    /// </summary>
    /// <returns> A task containing a collection of all projectors in the system. </returns>
    public async Task<IEnumerable<Projector>> ListAllProjectorsAsync()
    {
        return await _interactiveComponentRepository.ListAllProjectorsAsync();
    }

    /// <summary>
    /// Deletes a board identified by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteBoardAsync(string plateId)
    {
        await _interactiveComponentRepository
            .DeleteBoardAsync(plateId);
    }

    /// <summary>
    /// Updates the specified board in the repository.
    /// </summary>
    /// <param name="board">The <see cref="Board"/> instance containing the updated data. Cannot be NULL>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateBoardAsync(Board board)
    {
        await _interactiveComponentRepository
            .UpdateBoardAsync(board);
    }

    /// <summary>
    /// Retrieves a paginated list of boards along with the total count of available boards.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a tuple containing: <list type="bullet">
    /// <item> <description><see cref="IEnumerable{Board}"/> representing the boards in the requested
    /// page.</description> </item> <item> <description>An <see cref="int"/> representing the total count of boards
    /// available.</description> </item> </list></returns>
    public async Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> ListBoardsPagedAsync(int pageNumber, int pageSize)
    {
        var boards = await _interactiveComponentRepository.ListBoardsPagedAsync(pageNumber, pageSize);

        return boards;
    }

    public async Task<(IEnumerable<Board> Boards, PaginationMetadata Metadata)> FilterBoardsAsync(string searchTerm, int pageNumber, int pageSize)
    {
        return await _interactiveComponentRepository.FilterBoardsAsync(searchTerm, pageNumber, pageSize);
    }

    public async Task<(IEnumerable<Projector> Projectors, PaginationMetadata Metadata)> FilterProjectorsAsync(string searchTerm, int pageNumber, int pageSize)
    {
         return await _interactiveComponentRepository.FilterProjectorsAsync(searchTerm, pageNumber, pageSize);
    }
}
