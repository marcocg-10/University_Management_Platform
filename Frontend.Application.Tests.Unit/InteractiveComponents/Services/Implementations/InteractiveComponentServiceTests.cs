using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.InteractiveComponents.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Core.Metadata;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.InteractiveComponents.Services.Implementations;

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
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_CreateBoardAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
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
            .Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _service.CreateBoardAsync(colorValue, markerColorValue, texture, plateId, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        // Assert
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be(plateId);

        _repositoryMock.Verify(r => r.AddBoardAsync(result), Times.Once);
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if texture is null.
    /// </summary>
    [Fact]
    public async Task CreateBoardAsync_Should_Throw_When_Texture_Is_Null()
    {
        // Act
        Func<Task> act = () => _service.CreateBoardAsync("#FFF", "#000", null!, "123456", 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

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
        Func<Task> act = () => _service.CreateBoardAsync("#FFF", "#000", "", "123456", 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

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
            .Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
            .Callback<InteractiveComponent>(b => board = (Board)b)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateBoardAsync("#AAA", "#111", "Rough", "123456", 1, 2, 3, 4, 5, 6, 0, 0, 0, 2);

        // Assert
        board.Should().NotBeNull();
        board.PlateId.Value.Should().Be("123456");
        _repositoryMock.Verify(r => r.AddBoardAsync(It.IsAny<Board>()), Times.Once);
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

        _repositoryMock
            .Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
            .Returns(Task.CompletedTask);

        // Act
        var board = await _service.CreateBoardAsync("#123", "#456", "Glossy", "123456", x, y, z, width, height, depth, 0, 0, 0, 1);

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

        _repositoryMock
            .Setup(r => r.AddBoardAsync(It.IsAny<Board>()))
            .Returns(Task.CompletedTask);

        // Act
        var board = await _service.CreateBoardAsync("#123", "#456", "Glossy", "123456", x, y, z, width, height, depth, 0, 0, 0, 1);

        // Assert
        board.Dimensions.Width.Should().Be(width);
        board.Dimensions.Height.Should().Be(height);
        board.Dimensions.Depth.Should().Be(depth);
    }
}

/// <summary>
/// Unit tests for <see cref="InteractiveComponentService.DeleteBoardAsync"/>.
/// </summary>
public class InteractiveComponentService_DeleteBoardAsync_Tests
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly InteractiveComponentService _service;

    public InteractiveComponentService_DeleteBoardAsync_Tests()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
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
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListAllBoards()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
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
            new Rotations(0,0,0),
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
    private readonly InteractiveComponentService _service;

    public InteractiveComponentServiceTests_CreateProjectorAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
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
            .Setup(r => r.AddProjectorAsync(It.IsAny<Projector>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _service.CreateProjectorAsync(colorValue, texture, brightness, plateId, 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 1);

        // Assert
        result.Should().NotBeNull();
        result.PlateId.Value.Should().Be(plateId);

        _repositoryMock.Verify(r => r.AddProjectorAsync(result), Times.Once);
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
    /// Ensures <see cref="ArgumentException"/> is thrown if brightness is less than 0.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Brightness_Is_Less_Than_0()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", "Smooth", -1, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Brightness is out of valid range [0, 100]*");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown if brightness is greater than 100.
    /// </summary>
    [Fact]
    public async Task CreateProjectorAsync_Should_Throw_When_Brightness_Is_Greater_Than_100()
    {
        // Act
        Func<Task> act = () => _service.CreateProjectorAsync("#FFF", "Smooth", 101, "123456", 1920, 1080, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Brightness is out of valid range [0, 100]*");
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
            .Setup(r => r.AddProjectorAsync(It.IsAny<Projector>()))
            .Callback<InteractiveComponent>(p => projector = (Projector)p)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateProjectorAsync("#AAA", "Rough", 80, "123456", 1920, 1080, 1, 2, 3, 4, 5, 6, 0, 0, 0, 2);

        // Assert
        projector.Should().NotBeNull();
        projector.PlateId.Value.Should().Be("123456");
        _repositoryMock.Verify(r => r.AddProjectorAsync(It.IsAny<Projector>()), Times.Once);
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
            .Setup(r => r.AddProjectorAsync(It.IsAny<Projector>()))
            .Returns(Task.CompletedTask);
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
            .Setup(r => r.AddProjectorAsync(It.IsAny<Projector>()))
            .Returns(Task.CompletedTask);

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
            .Setup(r => r.AddProjectorAsync(It.IsAny<Projector>()))
            .Returns(Task.CompletedTask);

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
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListAllProjectors()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
    }

    /// <summary>
    /// Ensures that the service returns all projectors from the repository.
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
    /// Ensures the service returns an empty list when the repository has no projectors.
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
/// Unit tests for <see cref="InteractiveComponentService.ListBoardsPagedAsync"/>.
/// </summary>
public class InteractiveComponentServiceTests_ListBoardsPagedAsync
{
    private readonly Mock<IInteractiveComponentRepository> _repositoryMock;
    private readonly InteractiveComponentService _service;

    /// <summary>
    /// Initializes mocks and the service under test.
    /// </summary>
    public InteractiveComponentServiceTests_ListBoardsPagedAsync()
    {
        _repositoryMock = new Mock<IInteractiveComponentRepository>();
        _service = new InteractiveComponentService(_repositoryMock.Object);
    }


    /// <summary>
    /// Ensures the service returns an empty list with valid metadata when no boards exist in the requested page.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Empty_Boards_With_Metadata_When_No_Items()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;

        var boards = Enumerable.Empty<Board>();
        var metadata = new PaginationMetadata
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = 0,
            TotalPages = 0
        };

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((boards, metadata));

        // Act
        var (resultBoards, resultMetadata) = await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        resultBoards.Should().NotBeNull();
        resultBoards.Should().BeEmpty();

        resultMetadata.Should().NotBeNull();
        resultMetadata.TotalCount.Should().Be(0);
        resultMetadata.CurrentPage.Should().Be(pageNumber);
        resultMetadata.PageSize.Should().Be(pageSize);

        _repositoryMock.Verify(r => r.ListBoardsPagedAsync(pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures that exceptions from the repository are propagated by the service.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Propagate_Repository_Exception()
    {
        // Arrange
        var pageNumber = 3;
        var pageSize = 5;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ThrowsAsync(new Exception("Repository failure in pagination"));

        // Act
        Func<Task> act = () => _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Repository failure in pagination");

        _repositoryMock.Verify(r => r.ListBoardsPagedAsync(pageNumber, pageSize), Times.Once);
    }

    /// <summary>
    /// Ensures the repository is called with the exact provided pagination parameters.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Call_Repository_With_Correct_Params()
    {
        // Arrange
        var pageNumber = 5;
        var pageSize = 20;

        _repositoryMock
            .Setup(r => r.ListBoardsPagedAsync(pageNumber, pageSize))
            .ReturnsAsync((Enumerable.Empty<Board>(), new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = 0,
                TotalPages = 0
            }));

        // Act
        await _service.ListBoardsPagedAsync(pageNumber, pageSize);

        // Assert
        _repositoryMock.Verify(r => r.ListBoardsPagedAsync(pageNumber, pageSize), Times.Once);
    }
}
