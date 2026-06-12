using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services.Implementations;

/// <summary>
/// Service layer implementation responsible for managing <see cref="InteractiveComponent"/> entities,
/// specifically <see cref="Board"/> and <see cref="Projector"/> types.
/// </summary>
/// <remarks>
/// This service provides methods for creating, reading, updating, deleting, listing boards and the 
/// creation of projectors.
/// It acts as a mediator between the application layer and the persistence layer, ensuring 
/// that domain rules are enforced when performing operations.
/// </remarks>
internal class InteractiveComponentService : IInteractiveComponentService
{
    private readonly IInteractiveComponentRepository _interactiveComponentRepository;
    private readonly IInteractiveComponentCollisionService _interactiveComponentCollisionService;
    private readonly IInteractiveComponentContainmentService _interactiveComponentContainmentService;
    private readonly ILearningSpaceRepository _learningSpaceRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="InteractiveComponentService"/>.
    /// </summary>
    /// <param name="interactiveComponentRepository">
    /// Repository responsible for persisting and retrieving interactive components.
    /// </param>
    /// <param name="interactiveComponentCollisionService">
    /// Repository responsible for collision detection of interactive components.
    /// </param>
    /// <param name="interactiveComponentContainmentService">
    /// Service responsible for containment detection of interactive components.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentRepository"/> is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentCollisionService"/> is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactiveComponentContainmentService"/> is null.</exception>
    public InteractiveComponentService(
        IInteractiveComponentRepository interactiveComponentRepository,
        IInteractiveComponentCollisionService interactiveComponentCollisionService,
        IInteractiveComponentContainmentService interactiveComponentContainmentService,
        ILearningSpaceRepository learningSpaceRepository)
    {
        _interactiveComponentRepository = interactiveComponentRepository;

        _interactiveComponentCollisionService = interactiveComponentCollisionService;

        _interactiveComponentContainmentService = interactiveComponentContainmentService;

        _learningSpaceRepository = learningSpaceRepository;
    }

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
    public async Task<Board> CreateBoardAsync(
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
        int learningSpaceId)
    {
        var color = new Color(colorValue);
        var markerColor = new Color(markerColorValue);

        if (string.IsNullOrWhiteSpace(texture))
            throw new ArgumentException("Texture cannot be null, empty, or whitespace.", nameof(texture));

        var coordinates = new Coordinates(x, y, z);
        var dimensions = new Dimensions(width, height, depth);
        var rotations = new Rotations(xAxisRotation, yAxisRotation, zAxisRotation);
        var plateIdValueObject = new PlateId(plateId);
        var existingLearningSpace = await _learningSpaceRepository.GetLearningSpaceByIdAsync(learningSpaceId);
        if (existingLearningSpace is null)
        {
            throw new LearningSpaceIdDoesNotExistException(learningSpaceId);
        }

        var board = new Board(color, markerColor, texture, plateIdValueObject, coordinates, dimensions, rotations, learningSpaceId);

        // Check if interactive component is fully contained inside a learning space.
        if (!await _interactiveComponentContainmentService.GetContainmentStatusAsync(board))
        {
            throw new InteractiveComponentContainmentException("The board is not fully contained within the learning space.");
        }

        // Check for collisions before adding the board.
        if (await _interactiveComponentCollisionService.DetectCollisionAsync(board))
        {
            throw new InteractiveComponentCollisionException("The board collides with an existing interactive component in the learning space.");
        }

        await _interactiveComponentRepository
            .AddInteractiveComponentAsync(board);

        return board;
    }

    /// <summary>
    /// Retrieves a <see cref="Board"/> by its unique <paramref name="plateId"/>.
    /// </summary>
    /// <param name="plateId">The unique identifier of the board to retrieve.</param>
    /// <returns>
    /// A task containing the <see cref="Board"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Board?> ReadBoardByPlateIdAsync(string plateId)
    {
        var interactiveComponent = await _interactiveComponentRepository
            .ReadBoardByPlateIdAsync(plateId);

        return interactiveComponent is null ? throw new BoardNotFoundException(plateId) : interactiveComponent;
    }

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
    public async Task<Board> UpdateBoardAsync(
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
        int learningSpaceId)
    {
        var color = new Color(colorValue);
        var markerColor = new Color(markerColorValue);

        if (string.IsNullOrWhiteSpace(texture))
            throw new ArgumentException("Texture cannot be null, empty, or whitespace.", nameof(texture));

        var coordinates = new Coordinates(x, y, z);
        var dimensions = new Dimensions(width, height, depth);
        var rotations = new Rotations(xAxisRotation, yAxisRotation, zAxisRotation);
        var plateIdValueObject = new PlateId(plateId);
        var existingLearningSpace = await _learningSpaceRepository.GetLearningSpaceByIdAsync(learningSpaceId);
        if (existingLearningSpace is null)
        {
            throw new LearningSpaceIdDoesNotExistException(learningSpaceId);
        }

        var updatedBoard = new Board(color, markerColor, texture, plateIdValueObject, coordinates, dimensions, rotations, learningSpaceId);

        // Check if interactive component is fully contained inside a learning space.
        if (!await _interactiveComponentContainmentService.GetContainmentStatusAsync(updatedBoard))
        {
            throw new InteractiveComponentContainmentException("The board is not fully contained within the learning space.");
        }

        // Check for collisions before updating the board.
        if (await _interactiveComponentCollisionService.DetectCollisionAsync(updatedBoard))
        {
            throw new InteractiveComponentCollisionException("The updated board collides with an existing interactive component in the learning space.");
        }

        await _interactiveComponentRepository
            .UpdateInteractiveComponentAsync(updatedBoard);

        return updatedBoard;
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
    /// Retrieves all <see cref="Board"/> instances from the repository.
    /// </summary>
    /// <returns>A task containing a collection of all boards in the system.</returns>
    public async Task<IEnumerable<Board>> ListAllBoardsAsync()
    {
        return await _interactiveComponentRepository
            .ListAllBoardsAsync();
    }

    /// <summary>
    /// Create a new Projector instance, wrapping the input attributes in domain value objects.
    /// </summary>
    /// <param name="colorValue">Hex or named string representing the main color of the projector.</param>
    /// <param name="texture">Surface texture or material pattern for the projector. Cannot be null,
    /// empty, or whitespace.</param>
    /// <param name="brightness">Value of the level of brightness of the projection, cannot be less than 0 or more
    /// than 100.</param>
    /// <param name="plateId">Unique identifier for the projector within the system.</param>
    /// <param name="resWidth">Width of the pixel resolution of the projection.</param>
    /// <param name="resHeight">Height of the pixel resolution of the projection.</param>
    /// <param name="x">X-Coordinate of the projector in the learning space.</param>
    /// <param name="y">Y-Coordinate of the projector in the learning space.</param>
    /// <param name="z">Z-Coordinate of the projector in the learning space.</param>
    /// <param name="width">Width of the projector in meters.</param>
    /// <param name="height">Height of the projector in meters.</param>
    /// <param name="depth">Depth of the projector in meters.</param>
    /// <param name="xAxisRotation">Rotation of the projector around the X-axis in degrees.</param>
    /// <param name="yAxisRotation">Rotation of the projector around the Y-axis in degrees.</param>
    /// <param name="zAxisRotation">Rotation of the projector around the Z-axis in degrees.</param>
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
        double xAxisRotation,
        double yAxisRotation,
        double zAxisRotation,
        int learningSpaceId)
    {
        var color = new Color(colorValue);

        if (string.IsNullOrWhiteSpace(texture))
            throw new ArgumentException("Texture cannot be null, empty, or whitespace.", nameof(texture));

        if (brightness < 0 || brightness > 100)
            throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0 and 100.");

        var resolution = new Resolution(resWidth, resHeight);
        var coordinates = new Coordinates(x, y, z);
        var dimensions = new Dimensions(width, height, depth);
        var rotations = new Rotations(xAxisRotation, yAxisRotation, zAxisRotation);
        var plateIdValueObject = new PlateId(plateId);
        var existingLearningSpace = await _learningSpaceRepository.GetLearningSpaceByIdAsync(learningSpaceId);
        if (existingLearningSpace is null)
        {
            throw new LearningSpaceIdDoesNotExistException(learningSpaceId);
        }

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

        // Check if interactive component is fully contained inside a learning space.
        if (!await _interactiveComponentContainmentService.GetContainmentStatusAsync(projector))
        {
            throw new InteractiveComponentContainmentException("The projector is not fully contained within the learning space.");
        }
        // Check for collisions before adding the projector.
        if (await _interactiveComponentCollisionService.DetectCollisionAsync(projector))
        {
            throw new InteractiveComponentCollisionException("The projector collides with an existing interactive component in the learning space.");
        }

        await _interactiveComponentRepository
            .AddInteractiveComponentAsync(projector);

        return projector;
    }

    /// <summary>
    /// Retrieves all <see cref="Projector"/> instances from the repository.
    /// </summary>
    /// <returns>A task containing a collection of all projectors in the system.</returns>
    public async Task<IEnumerable<Projector>> ListAllProjectorsAsync()
    {
        return await _interactiveComponentRepository
            .ListAllProjectorsAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of boards from the repository.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the boards for the current page and the total count of boards.</returns>
    public async Task<(IEnumerable<Board> Boards, int TotalCount)> ListBoardsPagedAsync(
        int pageNumber, 
        int pageSize)
    {
        return await _interactiveComponentRepository.ListBoardsPagedAsync(pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a paginated list of boards that match the specified search term.
    /// </summary>
    /// <param name="searchTerm">The term to search for in board names or descriptions. If null or whitespace, all boards are included.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of boards to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a tuple containing: <list type="bullet">
    /// <item> <description>An <see cref="IEnumerable{T}"/> of <see cref="Board"/> objects representing the boards in
    /// the current page.</description> </item> <item> <description>An <see cref="int"/> representing the total number
    /// of boards that match the search criteria.</description> </item> </list></returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> is less than 1 or <paramref name="pageSize"/> is less than 1.</exception>
    public Task<(IEnumerable<Board> Boards, int TotalCount)> FilterBoardsAsync(
        string searchTerm, 
        int pageNumber, 
        int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

        // Normalize the search term by trimming whitespace.
        var normalizedTerm = string.IsNullOrWhiteSpace(searchTerm) ? string.Empty : searchTerm.Trim();

        return _interactiveComponentRepository.FilterBoardsAsync(normalizedTerm, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a paginated list of projectors that match the specified search term.
    /// </summary>
    /// <param name="searchTerm">The term used to filter projectors. Can be a partial or full match on projector properties.</param>
    /// <param name="pageNumber">The page number of the results to retrieve. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of projectors to include in each page. Must be greater than 0.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with: <list type="bullet">
    /// <item> <description>An <see cref="IEnumerable{T}"/> of <see cref="Projector"/> objects that match the search
    /// criteria.</description> </item> <item> <description>The total count of projectors that match the search
    /// criteria, across all pages.</description> </item> </list></returns>
    public Task<(IEnumerable<Projector> Projectors, int TotalCount)> FilterProjectorsAsync(string searchTerm, int pageNumber, int pageSize)
    {
        return _interactiveComponentRepository.FilterProjectorsAsync(searchTerm, pageNumber, pageSize);
    }
}
