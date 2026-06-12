using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Provides unit tests for the <see cref="UserRepository"/> class, focusing on methods that interact with user data in
/// the database.
/// </summary>
public class UserRepositorySearchUsersTests
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly UserRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class with the specified test data.
    /// </summary>
    /// <param name="testData">The test data to be used for the user repository tests. Cannot be <see langword="null"/>.</param>
    public UserRepositorySearchUsersTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _repository = new UserRepository(_dbContextMock.Object);
    }


    /// <summary>
    /// Ensures that paginated boards are retrieved correctly with valid page number and size
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Correct_Page()
    {
        // Arrange
        var users = Enumerable.Range(1, 25)
            .Select(i => new User(
                            UserId.Create($"{i:D5}"),
                            UserName.Create("FooBar"),
                            true,
                            Email.Create($"Foo{i}@foo.foo")))
            .ToList();

        var usersDbSet = users.BuildMockDbSet();
        _dbContextMock.Setup(db => db.Users).Returns(usersDbSet.Object);

        // Act
        var (pagedUsers, totalCount) = await _repository.SearchUsersAsync("", 2, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedUsers.Should().HaveCount(10);
        pagedUsers.First().Id.Value.Should().Be("00011");
        pagedUsers.Last().Id.Value.Should().Be("00020");

    }

    /// <summary>
    /// Ensures that the first page returns the correct boards.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_First_Page_Correctly()
    {
        // Arrange
        var users = Enumerable.Range(1, 15)
            .Select(i => new User(
                            UserId.Create($"{i:D5}"),
                            UserName.Create("FooBar"),
                            true,
                            Email.Create($"Foo{i}@foo.foo")))
            .ToList();
        var usersDbSet = users.BuildMockDbSet();
        _dbContextMock.Setup(db => db.Users).Returns(usersDbSet.Object);

        // Act
        var (pagedUsers, totalCount) = await _repository.SearchUsersAsync("", 1, 10);

        // Assert
        totalCount.Should().Be(15);
        pagedUsers.Should().HaveCount(10);
        pagedUsers.First().Id.Value.Should().Be("00001");
        pagedUsers.Last().Id.Value.Should().Be("00010");
    }

    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Last_Page_With_Remaining_Items()
    {
        // Arrange
        var users = Enumerable.Range(1, 25)
            .Select(i => new User(
                            UserId.Create($"{i:D5}"),
                            UserName.Create("FooBar"),
                            true,
                            Email.Create($"Foo{i}@foo.foo")))
            .ToList();

        var usersDbSet = users.BuildMockDbSet();
        _dbContextMock.Setup(db => db.Users).Returns(usersDbSet.Object);

        // Act
        var (pagedUsers, totalCount) = await _repository.SearchUsersAsync("", 3, 10);

        // Assert
        totalCount.Should().Be(25);
        pagedUsers.Should().HaveCount(5);
        pagedUsers.First().Id.Value.Should().Be("00021");
        pagedUsers.Last().Id.Value.Should().Be("00025");
    }

    /// <summary>
    /// Ensures empty list is returned when page number exceeds available pages.
    /// </summary>
    [Fact]
    public async Task ListBoardsPagedAsync_Should_Return_Empty_When_Page_Exceeds_Total()
    {
        // Arrange
        var users = Enumerable.Range(1, 10)
            .Select(i => new User(
                            UserId.Create($"{i:D5}"),
                            UserName.Create("FooBar"),
                            true,
                            Email.Create($"Foo{i}@foo.foo")))
            .ToList();

        var usersDbSet = users.BuildMockDbSet();
        _dbContextMock.Setup(db => db.Users).Returns(usersDbSet.Object);

        // Act
        var (pagedUsers, totalCount) = await _repository.SearchUsersAsync("", 5, 10);

        // Assert
        totalCount.Should().Be(10);
        pagedUsers.Should().BeEmpty();
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
        Func<Task> act = () => _repository.SearchUsersAsync("", invalidPageNumber, 10);

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
        Func<Task> act = () => _repository.SearchUsersAsync("", 1, invalidPageSize);

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
        var usersDbSet = new List<User>().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Users).Returns(usersDbSet.Object);

        // Act
        var (pagedUsers, totalCount) = await _repository.SearchUsersAsync("", 1, 10);

        // Assert
        totalCount.Should().Be(0);
        pagedUsers.Should().BeEmpty();
    }
}
