using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.InteractiveComponents.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.InteractiveComponents.Repositories;

/// <summary>
/// Unit tests for <see cref="InteractiveComponentRepository"/>.
/// </summary>
public class InteractiveComponentRepositoryTests
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly InteractiveComponentRepository _repository;

    public InteractiveComponentRepositoryTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _repository = new InteractiveComponentRepository(_dbContextMock.Object);
    }

    /// <summary>
    /// Ensures an interactive component is added and persisted in the database.
    /// </summary>
    [Fact]
    public async Task AddInteractiveComponentAsync_Should_Add_Component_To_Database()
    {
        // Arrange
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90,0,0),
            1);

        // Act
        await _repository.AddInteractiveComponentAsync(board);

        // Assert
        _dbContextMock.Verify(db => db.InteractiveComponents.AddAsync(board, It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    ///<summary>
    /// Ensures a Projector is added and persisted in the database.
    /// </summary>
    [Fact]
    public async Task AddInteractiveComponentAsync_Should_Add_Projector_To_Database()
    {
        // Arrange
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var projector = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("123456"),
            new Resolution(1920, 1080),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1
        );

        // Act
        await _repository.AddInteractiveComponentAsync(projector);

        // Assert
        _dbContextMock.Verify(db => db.InteractiveComponents.AddAsync(projector, It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Ensures that adding a component does not modify other database state accidentally
    /// and that the new component is correctly added to the DbSet.
    /// </summary>
    [Fact]
    public async Task AddInteractiveComponentAsync_Should_Not_Affect_Other_Components()
    {
        // Arrange
        var existingBoard = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123456"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 0),
            1);

        var interactiveComponentsList = new List<InteractiveComponent> { existingBoard };
        var interactiveComponents = interactiveComponentsList.BuildMockDbSet();

        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);

        // Mock AddAsync to add to the underlying list and return dummy ValueTask
        _dbContextMock
            .Setup(db => db.InteractiveComponents.AddAsync(It.IsAny<InteractiveComponent>(), It.IsAny<CancellationToken>()))
            .Callback<InteractiveComponent, CancellationToken>((entity, ct) => interactiveComponentsList.Add(entity))
            .Returns((InteractiveComponent entity, CancellationToken ct) =>
                new ValueTask<EntityEntry<InteractiveComponent>>(Task.FromResult<EntityEntry<InteractiveComponent>>(null!)));

        _dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var newBoard = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);

        // Act
        await _repository.AddInteractiveComponentAsync(newBoard);

        // Assert
        _dbContextMock.Verify(db => db.InteractiveComponents.AddAsync(newBoard, It.IsAny<CancellationToken>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Both boards should be present in the list
        interactiveComponentsList.Should().ContainSingle(c => c.PlateId == newBoard.PlateId);
        interactiveComponentsList.Should().Contain(c => c.PlateId == existingBoard.PlateId);
    }

    /// <summary>
    /// Ensures <see cref="ReadBoardByPlateIdAsync"/> returns the correct board when it exists.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Return_Board_When_Exists()
    {
        // Arrange
        var board = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(0, 180, 0),
            1
        );

        var boardsDbSet = new List<InteractiveComponent> { board }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var result = await _repository.ReadBoardByPlateIdAsync("123456");

        // Assert
        result.Should().NotBeNull();
        result!.PlateId.Should().Be(board.PlateId);
    }

    /// <summary>
    /// Ensures <see cref="ReadBoardByPlateIdAsync"/> returns null when board does not exist.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        var boardsDbSet = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var result = await _repository.ReadBoardByPlateIdAsync("654321");

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown when plateId is null.
    /// </summary>
    [Fact]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_ArgumentException_When_PlateId_Is_Null()
    {
        // Act
        Func<Task> act = () => _repository.ReadBoardByPlateIdAsync(null!);

        // Assert
        await act.Should().ThrowAsync<InvalidPlateIdException>()
            .WithMessage("Plate ID cannot be null.");
    }

    /// <summary>
    /// Ensures <see cref="ArgumentException"/> is thrown when plateId is empty or whitespace.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task ReadBoardByPlateIdAsync_Should_Throw_ArgumentException_When_PlateId_Is_Whitespace(string invalidPlateId)
    {
        // Act
        Func<Task> act = () => _repository.ReadBoardByPlateIdAsync(invalidPlateId);

        // Assert
        await act.Should().ThrowAsync<InvalidPlateIdException>()
            .WithMessage("Plate ID cannot be empty or whitespace.");
    }

    /// <summary>
    /// Ensures <see cref="BoardNotFoundException"/> is thrown when component does not exist in database.
    /// </summary>
    [Fact]
    public async Task UpdateInteractiveComponentAsync_Should_Throw_BoardNotFoundException_When_Component_Not_Exists()
    {
        // Arrange
        var component = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1
        );

        var dbSet = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(dbSet.Object);

        // Act
        Func<Task> act = () => _repository.UpdateInteractiveComponentAsync(component);

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("*was not found*");
    }

    /// <summary>
    /// Ensures the component is updated and changes are saved successfully.
    /// </summary>
    [Fact]
    public async Task UpdateInteractiveComponentAsync_Should_Update_Component_When_Exists()
    {
        // Arrange
        var existingComponent = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123456"),
            new Coordinates(0, 0, 0),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1
        );

        var updatedComponent = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 2, 3),
            new Dimensions(2, 2, 2),
            new Rotations(0, 0, 0),
            1
        );

        var dbSet = new List<InteractiveComponent> { existingComponent }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(dbSet.Object);
        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        await _repository.UpdateInteractiveComponentAsync(updatedComponent);

        // Assert
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        existingComponent.Color.Should().Be(updatedComponent.Color);
        existingComponent.Texture.Should().Be(updatedComponent.Texture);
        existingComponent.Dimensions.Should().BeEquivalentTo(updatedComponent.Dimensions);
    }

    /// <summary>
    /// Ensures <see cref="DbUpdateConcurrencyException"/> is propagated correctly.
    /// </summary>
    [Fact]
    public async Task UpdateInteractiveComponentAsync_Should_Throw_DbUpdateConcurrencyException_On_ConcurrencyViolation()  
    {
        // Arrange
        var component = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(0, 0, 180),
            1
        );

        var dbSet = new List<InteractiveComponent> { component }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(dbSet.Object);

        _dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateConcurrencyException());

        // Act
        Func<Task> act = () => _repository.UpdateInteractiveComponentAsync(component);

        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }
 
    /// <summary>
    /// Ensures that when a board exists in the database,
    /// <see cref="InteractiveComponentRepository.DeleteBoardAsync"/> successfully removes it
    /// and persists the change.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_WhenBoardExists_DeletesSuccessfully()
    {
        // Arrange
        var existingBoard = new Board(
            color: new Color("#FFF"),
            markerColor: new Color("#000"),
            texture: "Smooth",
            plateId: new PlateId("123456"),
            coordinates: new Coordinates(1, 2, 3),
            dimensions: new Dimensions(4, 5, 6),
            new Rotations(90, 0, 0),
            learningSpaceId: 1
        );

        var componentsDbSetMock = new List<InteractiveComponent> { existingBoard }.BuildMockDbSet();

        _dbContextMock.Setup(c => c.InteractiveComponents)
            .Returns(componentsDbSetMock.Object);

        _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new InteractiveComponentRepository(_dbContextMock.Object);

        // Act
        await sut.DeleteBoardAsync("123456");

        // Assert
        _dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        componentsDbSetMock.Verify(ds => ds.Remove(It.Is<Board>(b => b == existingBoard)), Times.Once);
    }

    /// <summary>
    /// Verifies that attempting to delete a board that does not exist
    /// results in a <see cref="BoardNotFoundException"/> with a clear message.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_WhenBoardDoesNotExist_ThrowsBoardNotFoundException()
    {
        // Arrange
        var componentsDbSetMock = new List<InteractiveComponent>().BuildMockDbSet();

        _dbContextMock.Setup(c => c.InteractiveComponents)
            .Returns(componentsDbSetMock.Object);

        var sut = new InteractiveComponentRepository(_dbContextMock.Object);

        // Act
        Func<Task> act = async () => await sut.DeleteBoardAsync("999999");

        // Assert
        await act.Should().ThrowAsync<BoardNotFoundException>()
            .WithMessage("Board with Plate ID 999999 was not found.");
    }

    /// <summary>
    /// Ensures that when multiple boards exist in the system,
    /// only the board matching the given PlateId is deleted.
    /// </summary>
    [Fact]
    public async Task DeleteBoardAsync_WhenMultipleBoardsExist_DeletesCorrectBoard()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("123456"),
            new Coordinates(1, 1, 1),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);

        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("654321"),
            new Coordinates(2, 2, 2),
            new Dimensions(2, 2, 2),
            new Rotations(90, 0, 0),
            1);

        var componentsDbSetMock = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();

        _dbContextMock.Setup(c => c.InteractiveComponents)
            .Returns(componentsDbSetMock.Object);

        _dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new InteractiveComponentRepository(_dbContextMock.Object);

        // Act
        await sut.DeleteBoardAsync("123456");

        // Assert
        _dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        componentsDbSetMock.Verify(ds => ds.Remove(It.Is<Board>(b => b == board1)), Times.Once);
        componentsDbSetMock.Verify(ds => ds.Remove(It.Is<Board>(b => b == board2)), Times.Never);
    }

    /// <summary>
    /// Ensures that all boards are retrieved successfully from the database.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Return_All_Boards()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);

        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var result = await _repository.ListAllBoardsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(board1);
        result.Should().Contain(board2);
    }

    /// <summary>
    /// Ensures that an empty list is returned when there are no boards in the database.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Return_Empty_List_When_No_Boards()
    {
        // Arrange
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);

        // Act
        var result = await _repository.ListAllBoardsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures that exceptions thrown by the database are propagated.
    /// </summary>
    [Fact]
    public async Task ListAllBoardsAsync_Should_Throw_When_Db_Exception_Occurs()
    {
        // Arrange
        _dbContextMock.Setup(db => db.InteractiveComponents)
            .Throws(new Exception("Database error"));

        // Act
        Func<Task> act = () => _repository.ListAllBoardsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");
    }

    /// <summary>
    /// Ensures that all projectors are retrieved successfully from the database.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Return_All_Boards()
    {
        // Arrange
        var projector1 = new Projector (
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("654321"),
            new Resolution(1920, 1080),
            new Coordinates(1, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(90, 0, 0),
            1);
        var projector2 = new Projector(
            new Color("#FFF"),
            "Rough",
            100,
            new PlateId("123455"),
            new Resolution(1920, 1080),
            new Coordinates(1, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(90, 0, 0),
            2);

        var projectors = new List<InteractiveComponent> { projector1, projector2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(projectors.Object);

        // Act
        var result = await _repository.ListAllProjectorsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(projector1);
        result.Should().Contain(projector2);
    }

    /// <summary>
    /// Ensures that an empty list is returned when there are no projectors in the database.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Return_Empty_List_When_No_Projectors()
    {
        // Arrange
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);

        // Act
        var result = await _repository.ListAllProjectorsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures that exceptions thrown by the database are propagated.
    /// </summary>
    [Fact]
    public async Task ListAllProjectorsAsync_Should_Throw_When_Db_Exception_Occurs()
    {
        // Arrange
        _dbContextMock.Setup(db => db.InteractiveComponents)
            .Throws(new Exception("Database error"));

        // Act
        Func<Task> act = () => _repository.ListAllProjectorsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");
    }

    /// <summary>
    /// Ensures that all interactive component from a specific learning space
    /// are retrieved successfully from the database.
    /// </summary>
    [Fact]
    public async Task ListAllInteractiveComponentsInRoomAsync_Should_Return_All_InteractiveComponentsInRoom()
    {
        // Arrange
        var learningSpaceId = 1;
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            learningSpaceId);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("654322"),
            new Resolution(1920, 1080),
            new Coordinates(2, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(90, 0, 0),
            learningSpaceId);

        var allInteractiveComponentsInRoom = new List<InteractiveComponent> { board1, projector1 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(allInteractiveComponentsInRoom.Object);

        // Act
        var result = await _repository.GetInteractiveComponentsByLearningSpaceAsync(learningSpaceId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(board1);
        result.Should().Contain(projector1);
    }

    /// <summary>
    /// Ensures that an empty list is returned when there are no interactive components in the database.
    /// </summary>
    [Fact]
    public async Task ListAllInteractiveComponentsInRoomAsync_Should_Return_Empty_List_When_No_InteractiveComponentsInBd()
    {
        // Arrange
        var learningSpaceId = 1;
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);

        // Act
        var result = await _repository.GetInteractiveComponentsByLearningSpaceAsync(learningSpaceId);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures that an empty list is returned when there are no interactive components in the specified learning space.
    /// </summary>
    [Fact]
    public async Task ListAllInteractiveComponentsInRoomAsync_Should_Return_Empty_List_When_No_InteractiveComponentsInRoom()
    {
        // Arrange
        var learningSpaceId = 2;
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var projector1 = new Projector(
            new Color("#FFF"),
            "Smooth",
            100,
            new PlateId("654322"),
            new Resolution(1920, 1080),
            new Coordinates(2, 2, 3),
            new Dimensions(10, 5, 1),
            new Rotations(90, 0, 0),
            1);
        var interactiveComponents = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(interactiveComponents.Object);

        // Act
        var result = await _repository.GetInteractiveComponentsByLearningSpaceAsync(learningSpaceId);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures that exceptions thrown by the database are propagated.
    /// </summary>
    [Fact]
    public async Task ListAllInteractiveComponentsInRoomAsync_Should_Throw_When_Db_Exception_Occurs()
    {
        // Arrange
        var learningSpaceId = 1;
        _dbContextMock.Setup(db => db.InteractiveComponents)
            .Throws(new Exception("Database error"));

        // Act
        Func<Task> act = () => _repository.GetInteractiveComponentsByLearningSpaceAsync(learningSpaceId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");
    }

    /// <summary>
    /// Ensures that paginated boards are retrieved correctly with valid page number and size
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Correct_Page()
    {
        // Arrange
        var boards = Enumerable.Range(1, 25)
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

        var boardsDbSet = boards.Cast<InteractiveComponent>().ToList().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var (pagedBoards, totalCount) = await _repository.ListBoardsPagedAsync(2, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedBoards.Should().HaveCount(10);
        pagedBoards.First().PlateId.ToString().Should().Be("000011");
        pagedBoards.Last().PlateId.ToString().Should().Be("000020");

    }

    /// <summary>
    /// Ensures that the first page returns the correct boards.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var boards = Enumerable.Range(1, 15)
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
        var boardsDbSet = boards.Cast<InteractiveComponent>().ToList().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var (pagedBoards, totalCount) = await _repository.ListBoardsPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(15);
        pagedBoards.Should().HaveCount(10);
        pagedBoards.First().PlateId.ToString().Should().Be("000001");
        pagedBoards.Last().PlateId.ToString().Should().Be("000010");
    }

    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var boards = Enumerable.Range(1, 25)
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
        
        var boardsDbSet = boards.Cast<InteractiveComponent>().ToList().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var (pagedBoards, totalCount) = await _repository.ListBoardsPagedAsync(3, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedBoards.Should().HaveCount(5);
        pagedBoards.First().PlateId.ToString().Should().Be("000021");
        pagedBoards.Last().PlateId.ToString().Should().Be("000025");
    }

    /// <summary>
    /// Ensures empty list is returned when page number exceeds available pages.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Empty_When_Page_Exceeds_Total()
    {
        // Arrange
        var boards = Enumerable.Range(1, 10)
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

        var boardsDbSet = boards.Cast<InteractiveComponent>().ToList().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var (pagedBoards, totalCount) = await _repository.ListBoardsPagedAsync(5, 10);

        // Assert
        totalCount.Should().Be(10);
        pagedBoards.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures ArgumentOutOfRangeException is thrown for invalid page number.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListBoardsPagedAsync_Should_Throw_For_Invalid_PageNumber(int invalidPageNumber)
    {
        // Act
        Func<Task> act = () => _repository.ListBoardsPagedAsync(invalidPageNumber, 10);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page number must be greater than zero*");
    }

    /// <summary>
    /// Ensures ArgumentOutOfRangeException is thrown for invalid page size.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task ListBoardsPagedAsync_Should_Throw_For_Invalid_PageSize(int invalidPageSize)
    {
        // Act
        Func<Task> act = () => _repository.ListBoardsPagedAsync(1, invalidPageSize);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Page size must be greater than zero*");
    }

    /// <summary>
    /// Ensures correct total count is returned even when boards list is empty.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Zero_TotalCount_When_No_Boards()
    {
        // Arrange
        var boardsDbSet = new List<InteractiveComponent>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boardsDbSet.Object);

        // Act
        var (pagedBoards, totalCount) = await _repository.ListBoardsPagedAsync(1, 10);

        // Assert
        totalCount.Should().Be(0);
        pagedBoards.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns boards matching the PlateId search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Boards_Matching_PlateId()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);

        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("654", 1, 2);

        // Assert
        totalCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().Contain(board1);
        result.Should().Contain(b => b.PlateId.ToString().Contains("654"));

    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns boards matching the Color search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Boards_Matching_Color()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("FFF", 1, 2);

        // Assert
        totalCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().Contain(board1);

        // Use the value object property for the assertion
        result.Should().Contain(b => b.Color.Value.Contains("FFF", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns boards matching the MarkerColor search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Boards_Matching_MarkerColor()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 22),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("000", 1, 2);

        // Assert
        totalCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().Contain(board1);

        // Use the value object property for the assertion
        result.Should().Contain(b => b.MarkerColor.Value.Contains("000", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns boards matching the Texture search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Matching_Texture()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("Smooth", 1, 2);
        // Assert
        totalCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().Contain(board1);
        result.Should().Contain(b => b.Texture.Contains("Smooth"));
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns boards matching the Coordinate X search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Boards_Matching_CoordinateX()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654324"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#222"),
            "Rough",
            new PlateId("723465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 0, 0),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);

        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("1", 1, 2);

        // Assert
        totalCount.Should().Be(1);
        result.Should().HaveCount(1);
        result.Should().Contain(board1);
        result.Should().Contain(b => b.Coordinates.X.ToString().Contains("1"));
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns all boards when search term is empty.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_All_Boards_When_SearchTerm_Is_Empty()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);
        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("", 1, 2);
        // Assert
        totalCount.Should().Be(2);
        result.Should().HaveCount(2);
        result.Should().Contain(board1);
        result.Should().Contain(board2);
    }

    /// <summary>
    /// Ensures FilterBoardsAsync returns an empty list when no boards match the search term.
    /// </summary>
    [Fact]
    public async Task FilterBoardsAsync_Should_Return_Empty_When_No_Match()
    {
        // Arrange
        var board1 = new Board(
            new Color("#FFF"),
            new Color("#000"),
            "Smooth",
            new PlateId("654321"),
            new Coordinates(1, 2, 3),
            new Dimensions(1, 1, 1),
            new Rotations(90, 0, 0),
            1);
        var board2 = new Board(
            new Color("#AAA"),
            new Color("#111"),
            "Rough",
            new PlateId("123465"),
            new Coordinates(4, 5, 6),
            new Dimensions(2, 2, 2),
            new Rotations(90, 180, 0),
            2);
        var boards = new List<InteractiveComponent> { board1, board2 }.BuildMockDbSet();
        _dbContextMock.Setup(db => db.InteractiveComponents).Returns(boards.Object);
        // Act
        var (result, totalCount) = await _repository.FilterBoardsAsync("999", 1, 2);
        // Assert
        totalCount.Should().Be(0);
        result.Should().BeEmpty();
    }

}
