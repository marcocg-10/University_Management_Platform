using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.InteractiveComponents.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.InteractiveComponents.Services.Implementations;

/// <summary>
/// Contains unit tests for <see cref="InteractiveComponentService"/>.
/// </summary>
/// <remarks>
/// The tests use <see cref="Moq"/> for mocking dependencies and 
/// <see cref="FluentAssertions"/> for clear, expressive assertions.
/// </remarks>
public class InteractiveComponentServiceTests_CreateBoardAsync
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_CreateBoardAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _learningSpaceRepositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Laboratory(1, 1, "R1", default!, default!, default!, default!));

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that a board is created successfully with valid inputs.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Create_Board_Successfully()
    {
        // Arrange
        var colorValue = "#FFF";
        var markerColorValue = "#000";
        var texture = "Smooth";
        var plateId = "123456";

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateBoardAsync(colorValue, markerColorValue, texture, plateId, 1, 2, 3, 4, 5, 6, 180, 180, 0, 1);

        // Assert
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be(plateId);

        _repositoryMock.Verify(r => r.AddInteractiveComponentAsync(result), Times.Once);
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if texture is null.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Throw_When_Texture_Is_Null()
    {
        // Act
        Func<Task> act = () => _service.CreateBoardAsync("#FFF", "#000", null!, "123456", 0, 0, 0, 1, 1, 1, 0, 180, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if texture is empty.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Throw_When_Texture_Is_Empty()
    {
        // Act
        Func<Task> act = () => _service.CreateBoardAsync("#FFF", "#000", "", "123456", 0, 0, 0, 1, 1, 1, 0, 180, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null*");
    }

    /// <summary>
    /// Ensures that the repository is called exactly once with the created board.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Call_Repository_Once()
    {
        // Arrange
        var board = default(Board);

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Callback<InteractiveComponent>(b => board = (Board)b)
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateBoardAsync("#AAA", "#111", "Rough", "123456", 1, 2, 3, 4, 5, 6, 0, 180, 0, 2);

        // Assert
        board.Should().NotBeNull();
        board.PlateId.Value.Should().Be("123456");
        _repositoryMock.Verify(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()), Times.Once);
    }

    /// <summary>
    /// Ensures that coordinates are wrapped correctly in value objects.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Set_Coordinates_Correctly()
    {
        // Arrange
        var x = 10.0;
        var y = 20.0;
        var z = 30.0;
        var width = 2.0;
        var height = 3.0;
        var depth = 1.5;
        var xRotation = 0;
        var yRotation = 180;
        var zRotation = 0;

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var board = await _service.CreateBoardAsync("#123", "#456", "Glossy", "123456", x, y, z, width, height, depth,
            xRotation, yRotation, zRotation, 1);

        // Assert
        board.Coordinates.X.Should().Be(x);
        board.Coordinates.Y.Should().Be(y);
        board.Coordinates.Z.Should().Be(z);
    }

    /// <summary>
    /// Ensures that dimensions are wrapped correctly in value objects.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Set_Dimensions_Correctly()
    {
        // Arrange
        var x = 10.0;
        var y = 20.0;
        var z = 30.0;
        var width = 2.0;
        var height = 3.0;
        var depth = 1.5;
        var xRotation = 0;
        var yRotation = 180;
        var zRotation = 0;

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var board = await _service.CreateBoardAsync("#123", "#456", "Glossy", "123456", x, y, z, width, height, depth,
                    xRotation, yRotation, zRotation, 1);

        // Assert
        board.Dimensions.Width.Should().Be(width);
        board.Dimensions.Height.Should().Be(height);
        board.Dimensions.Depth.Should().Be(depth);
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.ReadBoardByPlateIdAsync"/>.
/// </summary>
public class InteractiveComponentServiceTests_ReadBoardByPlateId
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_ReadBoardByPlateId()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _learningSpaceRepositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Laboratory(1, 1, "R1", default!, default!, default!, default!));

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures the service returns the correct board when the repository finds it.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Return_Board_When_Exists()
    {
        // Arrange
        var plateId = "123456";
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId(plateId),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(0, 180, 0),
            1);

        _repositoryMock
            .Setup(r => r.ReadBoardByPlateIdAsync(plateId))
            .ReturnsAsync(board);

        // Act
        var result = await _service.ReadBoardByPlateIdAsync(plateId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(board);
    }

    /// <summary>
    /// Ensures the service propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_When_Repository_Fails()
    {
        // Arrange
        var plateId = "123456";

        _repositoryMock
            .Setup(r => r.ReadBoardByPlateIdAsync(plateId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _service.ReadBoardByPlateIdAsync(plateId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");
    }

    /// <summary>
    /// Ensures that an exception is thrown if the plateId is null.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_When_PlateId_Is_Null()
    {
        // Act
        Func<Task> act = () => _service.ReadBoardByPlateIdAsync(null!);

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("Board with Plate ID  was not found.");
    }

    /// <summary>
    /// Ensures that an exception is thrown if the plateId is empty.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_When_PlateId_Is_Empty()
    {
        // Act
        Func<Task> act = () => _service.ReadBoardByPlateIdAsync("");

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("Board with Plate ID  was not found.");
    }

    /// <summary>
    /// Ensures that an exception is thrown if the plateId is whitespace.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_When_PlateId_Is_Whitespace()
    {
        // Act
        Func<Task> act = () => _service.ReadBoardByPlateIdAsync("   ");

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("Board with Plate ID     was not found.");
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.UpdateBoardAsync"/>.
/// </summary>
public class InteractiveComponentServiceTests_UpdateBoardAsync
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_UpdateBoardAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();
        _learningSpaceRepositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Laboratory(1, 1, "R1", default!, default!, default!, default!));

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that a board is updated successfully when valid data is provided.
    /// </summary>
    [Fact]
    public async Task UpdateBoardAsync_Should_Call_Repository_With_Correct_Board()
    {
        // Arrange
        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.UpdateInteractiveComponentAsync(It.IsAny<Board>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateBoardAsync("#FFF", "#000", "Smooth", "123456", 1, 2, 3, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        _repositoryMock.Verify(r => r.UpdateInteractiveComponentAsync(
            It.Is<Board>(b =>
                b.PlateId.Value == "123456" &&
                b.Color.Value == "#FFF" &&
                b.MarkerColor.Value == "#000" &&
                b.Texture == "Smooth" &&
                b.Coordinates.X == 1 &&
                b.Coordinates.Y == 2 &&
                b.Coordinates.Z == 3 &&
                b.Dimensions.Width == 1 &&
                b.Dimensions.Height == 1 &&
                b.Dimensions.Depth == 1 &&
                b.Rotations.XAxisRotation == 0 &&
                b.Rotations.YAxisRotation == 0 &&
                b.Rotations.ZAxisRotation == 0 &&
                b.LearningSpaceId == 1)
        ), Times.Once);

        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be("123456");
    }

    /// <summary>
    /// Throws ArgumentException if texture is null.
    /// </summary>
    [Fact]
    public async Task UpdateBoardAsync_Should_Throw_When_Texture_Is_Null()
    {
        Func<Task> act = () => _service.UpdateBoardAsync("#FFF", "#000", null!, "123456", 1, 2, 3, 1, 1, 1, 0, 0, 0, 1);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null, empty, or whitespace*");
    }

    /// <summary>
    /// Throws ArgumentException if texture is empty.
    /// </summary>
    [Fact]
    public async Task UpdateBoardAsync_Should_Throw_When_Texture_Is_Empty()
    {
        Func<Task> act = () => _service.UpdateBoardAsync("#FFF", "#000", "", "123456", 1, 2, 3, 1, 1, 1, 0, 0, 0, 1);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null, empty, or whitespace*");
    }

    /// <summary>
    /// Throws ArgumentException if texture is whitespace.
    /// </summary>
    [Fact]
    public async Task UpdateBoardAsync_Should_Throw_When_Texture_Is_Whitespace()
    {
        Func<Task> act = () => _service.UpdateBoardAsync("#FFF", "#000", "   ", "123456", 1, 2, 3, 1, 1, 1, 0, 0, 0, 1);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null, empty, or whitespace*");
    }

    /// <summary>
    /// Ensures that an exception from the repository propagates correctly.
    /// </summary>
    [Fact]
    public async Task UpdateBoardAsync_Should_Propagate_Repository_Exception()
    {
        // Arrange
        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.UpdateInteractiveComponentAsync(It.IsAny<Board>()))
            .ThrowsAsync(new BoardNotFoundException("123456"));

        // Act
        Func<Task> act = () => _service.UpdateBoardAsync("#FFF", "#000", "Smooth", "123456", 1, 2, 3, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("Board with Plate ID 123456 was not found.");
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.DeleteBoardAsync"/>.
/// </summary>
public class InteractiveComponentService_DeleteBoardAsync_Tests
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentService_DeleteBoardAsync_Tests()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service correctly calls the repository to delete a board.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_Should_Call_Repository_With_Correct_PlateId()
    {
        // Arrange
        var plateId = "123456";

        _repositoryMock
            .Setup(r => r.DeleteBoardAsync(plateId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteBoardAsync(plateId);

        // Assert
        _repositoryMock.Verify(r => r.DeleteBoardAsync(plateId), Times.Once);
    }

    /// <summary>
    /// Ensures that the service propagates exceptions from the repository.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_Should_Propagate_Repository_Exception()
    {
        // Arrange
        var plateId = "654321";
        _repositoryMock
            .Setup(r => r.DeleteBoardAsync(plateId))
            .ThrowsAsync(new Exception("Repository error"));

        // Act
        Func<Task> act = () => _service.DeleteBoardAsync(plateId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Repository error");

        _repositoryMock.Verify(r => r.DeleteBoardAsync(plateId), Times.Once);
    }

    /// <summary>
    /// Ensures that the service handles empty plate IDs appropriately by propagating the repository’s own validation.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_Should_Throw_When_PlateId_Is_Empty()
    {
        // Arrange
        var invalidPlateId = string.Empty;

        _repositoryMock
            .Setup(r => r.DeleteBoardAsync(invalidPlateId))
            .ThrowsAsync(new ArgumentException("PlateId cannot be null or whitespace."));

        // Act
        Func<Task> act = () => _service.DeleteBoardAsync(invalidPlateId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("PlateId cannot be null or whitespace.");

        _repositoryMock.Verify(r => r.DeleteBoardAsync(invalidPlateId), Times.Once);
    }

    /// <summary>
    /// Ensures that calling delete with a valid ID and no repository issues completes successfully.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_Should_Complete_Successfully_When_No_Errors()
    {
        // Arrange
        var plateId = "123456";
        _repositoryMock
            .Setup(r => r.DeleteBoardAsync(plateId))
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> act = () => _service.DeleteBoardAsync(plateId);

        // Assert
        await act.Should().NotThrowAsync();
        _repositoryMock.Verify(r => r.DeleteBoardAsync(plateId), Times.Once);
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.ListAllBoardsAsync"/>.
/// </summary>
public class InteractiveComponentServiceTests_ListAllBoards
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListAllBoards()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service returns all boards from the repository.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Return_All_Boards()
    {
        // Arrange
        var boards = new List<Board>
        {
            new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(0,0,0),
            new Dimensions(1,1,1),
            new Rotations(0,0,0),
            1),

            new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(2, 2, 2),
            new Rotations(90, 0, 90),
            2)
        };

        _repositoryMock
            .Setup(r => r.ListAllBoardsAsync())
            .ReturnsAsync(boards);

        // Act
        var result = await _service.ListAllBoardsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(b => b.PlateId.Value == "123456");
        result.Should().ContainSingle(b => b.PlateId.Value == "654321");

        _repositoryMock.Verify(r => r.ListAllBoardsAsync(), Times.Once);
    }

    /// <summary>
    /// Ensures the service returns an empty list when the repository has no boards.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Return_Empty_When_No_Boards()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ListAllBoardsAsync())
            .ReturnsAsync(new List<Board>());

        // Act
        var result = await _service.ListAllBoardsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _repositoryMock.Verify(r => r.ListAllBoardsAsync(), Times.Once);
    }

    /// <summary>
    /// Ensures that exceptions from the repository are propagated.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Propagate_Repository_Exception()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ListAllBoardsAsync())
            .ThrowsAsync(new Exception("Repository failure"));

        // Act
        Func<Task> act = () => _service.ListAllBoardsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Repository failure");
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.CreateProjectorAsync"/>.
/// </summary>
public class InteractiveComponentServiceTests_CreateProjectorAsync
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_CreateProjectorAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _learningSpaceRepositoryMock
            .Setup(r => r.GetLearningSpaceByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Laboratory(1, 1, "R1", default!, default!, default!, default!));

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that a projector is created successfully with valid inputs.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Create_Projector_Successfully()
    {
        // Arrange
        var colorValue = "#FFF";
        var texture = "Smooth";
        var brightness = 75;
        var plateId = "123456";

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateProjectorAsync(colorValue, texture, brightness, plateId, 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        // Assert
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be(plateId);

        _repositoryMock.Verify(r => r.AddInteractiveComponentAsync(result), Times.Once);
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if texture is null.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Texture_Is_Null()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", null!, 75, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);
        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if texture is empty.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Texture_Is_Empty()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", "", 75, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);
        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Texture cannot be null*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentOutOfRangeException"/> is thrown if brightness is less than 0.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Brightness_Is_Less_Than_0()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", "Smooth", -1, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Brightness must be between 0 and 100*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentOutOfRangeException"/> is thrown if brightness is greater than 100.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Brightness_Is_Greater_Than_100()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", "Smooth", 101, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Brightness must be between 0 and 100*");
    }

    /// <summary>
    /// Ensures that the repository is called exactly once with the created projector.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Call_Repository_Once()
    {
        // Arrange
        var projector = default(Projector);

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Callback<InteractiveComponent>(p => projector = (Projector)p)
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateProjectorAsync("#AAA", "Rough", 80, "123456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 2);

        // Assert
        projector.Should().NotBeNull();
        projector.PlateId.Value.Should().Be("123456");
        _repositoryMock.Verify(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()), Times.Once);
    }

    /// <summary>
    /// Ensures that resolution dimensions are wrapped correctly in value objects.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Set_Resolution_Correctly()
    {
        // Arrange
        var resWidth = 1920;
        var resHeight = 1080;

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var projector = await _service.CreateProjectorAsync("#123", "Glossy", 90, "123456", resWidth, resHeight, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        // Assert
        projector.ProjectionResolution.Width.Should().Be(resWidth);
        projector.ProjectionResolution.Height.Should().Be(resHeight);
    }

    /// <summary>
    /// Ensures that coordinates are wrapped correctly in value objects.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Set_Coordinates_Correctly()
    {
        // Arrange
        var x = 10.0;
        var y = 20.0;
        var z = 30.0;

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var projector = await _service.CreateProjectorAsync("#123", "Glossy", 90, "123456", 1920, 1080, x, y, z, 4, 5, 6, 0, 0, 0, 1);

        // Assert
        projector.Coordinates.X.Should().Be(x);
        projector.Coordinates.Y.Should().Be(y);
        projector.Coordinates.Z.Should().Be(z);
    }

    /// <summary>
    /// Ensures that dimensions are wrapped correctly in value objects.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Set_Dimensions_Correctly()
    {
        // Arrange
        var width = 2.0;
        var height = 3.0;
        var depth = 1.5;

        _repositoryMock
            .Setup(r => r.AddInteractiveComponentAsync(It.IsAny<InteractiveComponent>()))
            .Returns(Task.CompletedTask);

        _containmentMock
            .Setup(x => x.GetContainmentStatusAsync(It.IsAny<InteractiveComponent>()))
            .ReturnsAsync(true);

        // Act
        var projector = await _service.CreateProjectorAsync("#123", "Glossy", 90, "123456", 1920, 1080, 1, 2, 3, width, height, depth, 0, 0, 0, 1);

        // Assert
        projector.Dimensions.Width.Should().Be(width);
        projector.Dimensions.Height.Should().Be(height);
        projector.Dimensions.Depth.Should().Be(depth);
    }
}

/// <summary>
/// Unit tests for <see cref= "InteractiveComponentService.ListAllProjectorsAsync" />.
/// </summary>
public class InteractiveComponentServiceTests_ListAllProjectors
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;

    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListAllProjectors()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _service = new InteractiveComponentService(_repositoryMock.Object, _collisionMock.Object, _containmentMock.Object, _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service returns all boards from the repository.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Return_All_Projectors()
    {
        // Arrange
        var projectors = new List<Projector>
        {
            new Projector (
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("654321"),
            new Resolution(1920, 1080),
            new Coordinates(1, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(0, 0, 0),
            1),
            new Projector(
            new Color("#FFF"),
            "Rough",
            100,
            new PlateId("123455"),
            new Resolution(1920, 1080),
            new Coordinates(1, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(0, 0, 0),
            2)
        };

        _repositoryMock
            .Setup(r => r.ListAllProjectorsAsync())
            .ReturnsAsync(projectors);

        // Act
        var result = await _service.ListAllProjectorsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(b => b.PlateId.Value == "654321");
        result.Should().ContainSingle(b => b.PlateId.Value == "123455");

        _repositoryMock.Verify(r => r.ListAllProjectorsAsync(), Times.Once);
    }

    /// <summary>
    /// Ensures the service returns an empty list when the repository has no boards.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Return_Empty_When_No_Projectors()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ListAllProjectorsAsync())
            .ReturnsAsync(new List<Projector>());

        // Act
        var result = await _service.ListAllProjectorsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _repositoryMock.Verify(r => r.ListAllProjectorsAsync(), Times.Once);
    }

    /// <summary>
    /// Ensures that exceptions from the repository are propagated.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Propagate_Repository_Exception()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ListAllProjectorsAsync())
            .ThrowsAsync(new Exception("Repository failure"));

        // Act
        Func<Task> act = () => _service.ListAllProjectorsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Repository failure");
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.ListBoardsPagedAsync"/>
/// </summary>
public class InteractiveComponentServiceTests_ListBoardsPaged
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListBoardsPaged()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _service = new InteractiveComponentService(
            _repositoryMock.Object,
            _collisionMock.Object,
            _containmentMock.Object,
            _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service returns the correct page of boards and total count.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Correct_Page_And_TotalCount()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 10;
        var expectedBoards = Enumerable.Range(11, 10)
            .Select(i => new Board(
                new Color("#FFF"),
                new Color("#000"),
                "Smooth",
                new PlateId($"{i:D6}"),
                new Coordinates(i, i, i),
                new Dimensions(1, 1, 1),
                new Rotations(0, 0, 0),
                1))
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((expectedBoards, expectedTotalCount));

        // Act
        var (boards, totalCount) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        boards.Should().NotBeNull();
        boards.Should().HaveCount(10);
        boards.Should().BeEquivalentTo(expectedBoards);
        totalCount.Should().Be(expectedTotalCount);

        _repositoryMock.Verify(r => r.ListBoardsPagedAsync(pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that the service returns the first page of boards correctly.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var expectedBoards = Enumerable.Range(1, 10)
            .Select(i => new Board(
                new Color("#FFF"),
                new Color("#000"),
                "Smooth",
                new PlateId($"{i:D6}"), // 6-digit format: 000001, 000002, etc.
                new Coordinates(i, i, i),
                new Dimensions(1, 1, 1),
                new Rotations(0, 0, 0),
                1))
            .ToList();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((expectedBoards, expectedTotalCount));

        // Act
        var (boards, totalCount) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        boards.Should().HaveCount(10);
        boards.First().PlateId.Value.Should().Be("000001");
        boards.Last().PlateId.Value.Should().Be("000010");
        totalCount.Should().Be(15);
    }

    /// <summary>
    /// Ensures that the service returns the last page of boards with remaining items.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var pageNumber = 3;
        var pageSize = 10;
        var expectedBoards = Enumerable.Range(21, 5)
            .Select(i => new Board(
                new Color("#FFF"),
                new Color("#000"),
                "Smooth",
                new PlateId($"{i:D6}"), // 6-digit format: 000021-000025
                new Coordinates(i, i, i),
                new Dimensions(1, 1, 1),
                new Rotations(0, 0, 0),
                1))
            .ToList();
        var expectedTotalCount = 25;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((expectedBoards, expectedTotalCount));

        // Act
        var (boards, totalCount) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        boards.Should().HaveCount(5);
        boards.First().PlateId.Value.Should().Be("000021");
        boards.Last().PlateId.Value.Should().Be("000025");
        totalCount.Should().Be(25);
    }

    /// <summary>
    /// Ensures that the service returns an empty list when requesting a page beyond the total count.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Empty_List_When_Page_Exceeds_Total()
    {
        // Arrange
        var pageNumber = 10;
        var pageSize = 10;
        var expectedBoards = new List<Board>();
        var expectedTotalCount = 15;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((expectedBoards, expectedTotalCount));

        // Act
        var (boards, totalCount) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        boards.Should().BeEmpty();
        totalCount.Should().Be(15);

        _repositoryMock.Verify(r => r.ListBoardsPagedAsync(pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that the service correctly handles different page sizes.
    /// </summary>
    [Theory]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    [InlineData(20, 20)]
    [InlineData(50, 25)] // Only 25 total items
    public async Task ListBoardsPagedAsync_Should_Handle_Different_Page_Sizes(int pageSize, int expectedCount)
    {
        // Arrange
        var pageNumber = 1;
        var totalBoards = 25;
        var expectedBoards = Enumerable.Range(1, expectedCount)
            .Select(i => new Board(
                new Color("#FFF"),
                new Color("#000"),
                "Smooth",
                new PlateId($"{i:D6}"), // 6-digit format
                new Coordinates(i, i, i),
                new Dimensions(1, 1, 1),
                new Rotations(0, 0, 0),
                1))
            .ToList();

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((expectedBoards, totalBoards));

        // Act
        var (boards, totalCount) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        boards.Should().HaveCount(expectedCount);
        totalCount.Should().Be(totalBoards);
    }

    /// <summary>
    /// Ensures that the service propagates ArgumentOutOfRangeException for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListBoardsPagedAsync_Should_Propagate_Exception_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Arrange
        var pageSize = 10;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(invalidPageNumber, pageSize))
            .ThrowsAsync(new ArgumentOutOfRangeException(nameof(invalidPageNumber), "Page number must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListBoardsPagedAsync(invalidPageNumber, pageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page number must be greater than 0*");
    }

    /// <summary>
    /// Ensures that the service propagates ArgumentOutOfRangeException for invalid page size.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListBoardsPagedAsync_Should_Propagate_Exception_For_Invalid_PageSize(int invalidPageSize)
    {
        // Arrange
        var pageNumber = 1;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, invalidPageSize))
            .ThrowsAsync(new ArgumentOutOfRangeException(nameof(invalidPageSize), "Page size must be greater than 0."));

        // Act
        Func<Task> act = () => _service.ListBoardsPagedAsync(pageNumber, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page size must be greater than 0*");
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.FilterBoardsAsync"/>.
/// </summary>
public class InteractiveComponentService_FilterBoards
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly Mock<IInteractiveComponentCollisionService> _collisionMock;
    private readonly Mock<IInteractiveComponentContainmentService> _containmentMock;
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentService_FilterBoards()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _collisionMock = new Mock<IInteractiveComponentCollisionService>();
        _containmentMock = new Mock<IInteractiveComponentContainmentService>();
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>();

        _service = new InteractiveComponentService(
            _repositoryMock.Object,
            _collisionMock.Object,
            _containmentMock.Object,
            _learningSpaceRepositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service returns matching boards based on the search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Matching_Results()
    {
        // Arrange
        var searchTerm = "Smooth";
        var pageNumber = 1;
        var pageSize = 10;

        var expectedBoards = new List<Board>
        {
            new Board(
                new Color("#FFF"),
                new Color("#000"),
                "Smooth",
                new PlateId("000001"),
                new Coordinates(0,0,0),
                new Dimensions(1,1,1),
                new Rotations(0,0,0),
                1)
        };

        var expectedTotal = 1;

        _repositoryMock
            .Setup(r => r.FilterBoardsAsync(
                It.Is<string>(s => s == searchTerm.Trim()),
                pageNumber,
                pageSize))
            .ReturnsAsync((expectedBoards.AsEnumerable(), expectedTotal));

        // Act
        var (boards, totalCount) = await _service.FilterBoardsAsync(searchTerm, pageNumber, pageSize);

        // Assert
        boards.Should().NotBeNull();
        boards.Should().HaveCount(1);
        boards.First().PlateId.Value.Should().Be("000001");
        totalCount.Should().Be(expectedTotal);

        _repositoryMock.Verify(r => r.FilterBoardsAsync(searchTerm.Trim(), pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that the service returns an empty list when no boards match the search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Empty_When_No_Matches()
    {
        // Arrange
        var searchTerm = "1234sdfasdf51434";
        var pageNumber = 1;
        var pageSize = 10;

        _repositoryMock
            .Setup(r => r.FilterBoardsAsync(
                It.Is<string>(s => s == searchTerm),
                pageNumber,
                pageSize))
            .ReturnsAsync((Enumerable.Empty<Board>(), 0));

        // Act
        var (boards, totalCount) = await _service.FilterBoardsAsync(searchTerm, pageNumber, pageSize);

        // Assert
        boards.Should().BeEmpty();
        totalCount.Should().Be(0);

        _repositoryMock.Verify(r => r.FilterBoardsAsync(searchTerm, pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that the service returns all boards when the search term is null or whitespace.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task FilterBoardsAsync_Should_Return_All_When_Search_Is_EmptyOrNull(string? input)
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;

        var fullBoards = Enumerable.Range(1, 3)
            .Select(i => new Board(
                new Color("#FFF"),
                new Color("#000"),
                $"Texture{i}",
                new PlateId($"{i:D6}"),
                new Coordinates(i, i, i),
                new Dimensions(1, 1, 1),
                new Rotations(0, 0, 0),
                1))
            .ToList();

        // Service normalizes null/whitespace to string.Empty
        _repositoryMock
            .Setup(r => r.FilterBoardsAsync(
                It.Is<string>(s => s == string.Empty),
                pageNumber,
                pageSize))
            .ReturnsAsync((fullBoards.AsEnumerable(), fullBoards.Count));

        // Act
        var (boards, totalCount) = await _service.FilterBoardsAsync(input!, pageNumber, pageSize);

        // Assert
        boards.Should().HaveCount(3);
        totalCount.Should().Be(3);

        _repositoryMock.Verify(r => r.FilterBoardsAsync(string.Empty, pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that the service throws ArgumentOutOfRangeException for invalid paging parameters.
    /// </summary>
    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    public async Task FilterBoardsAsync_Should_Throw_For_Invalid_Paging_Parameters(int pageNumber, int pageSize)
    {
        // Arrange
        var searchTerm = "whatever";

        // Act
        Func<Task> act = () => _service.FilterBoardsAsync(searchTerm, pageNumber, pageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        _repositoryMock.Verify(r => r.FilterBoardsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}

