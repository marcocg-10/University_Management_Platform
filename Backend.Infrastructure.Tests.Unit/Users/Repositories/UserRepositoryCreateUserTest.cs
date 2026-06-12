using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;
public class UserRepositoryCreateUserTest : IClassFixture<UserRepositoryTestData>
{
    private readonly UserRepositoryTestData _testData;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class with the specified test data.
    /// </summary>
    /// <param name="testData">The test data to be used for the user repository tests. Cannot be <see langword="null"/>.</param>
    public UserRepositoryCreateUserTest(UserRepositoryTestData testData)
    {
        _testData = testData;
    }

    /// <summary>
    /// Tests that the <see cref="UserRepository.CreateUserAsync(User)"/> method correctly adds a single user to the
    /// database when the database is initially empty.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddUserAsync_WhenGivenNoData_StoresSingleEntryData()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.EmptyData);
        var usersDbSetMock = GetMockDbSet(usersDbSetData); ;
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Users.Add(It.IsAny<User>()))
            .Callback<User>(user => usersDbSetData.Add(user));
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);
        var user = _testData.SingleEntryData[0];

        // Act
        _ = await sut.CreateUserAsync(user);

        // Assert
        usersDbSetData.Should().BeEquivalentTo(
            _testData.SingleEntryData,
            because: "We added one user to the database, which had none");
    }

    /// <summary>
    /// Tests that the <see cref="UserRepository.CreateUserAsync(User)"/> method correctly stores multiple user entries 
    /// in the database when initially provided with a single user entry.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddUserAsync_WhenGivenSingleEntryData_StoresMultipleEntryData()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.SingleEntryData);
        var usersDbSetMock = GetMockDbSet(usersDbSetData);
        var dbContextMock = new Mock<AppDbContext>();

        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Users.Add(It.IsAny<User>()))
            .Callback<User>(user => usersDbSetData.Add(user));
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var sut = new UserRepository(dbContextMock.Object);
        var user = _testData.MultipleEntryData[1];

        // Act
        _ = await sut.CreateUserAsync(user);

        // Assert
        usersDbSetData.Should().BeEquivalentTo(
            _testData.MultipleEntryData,
            because: "We added one user to the database, which had one");
    }

    /// <summary>
    /// Verifies that adding a duplicate user to the database does not modify the existing data.
    /// </summary>
    /// <remarks>This test ensures that when a user with duplicate data is added, the database remains
    /// unchanged.  It uses a mocked database context to simulate the behavior of the repository and validate that  no
    /// additional entries are created.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task AddUserAsync_WhenGivenDuplicateData_ThrowsException()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.SingleEntryData);
        var usersDbSetMock = usersDbSetData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        var user = _testData.SingleEntryData[0];
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
             .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
             .ThrowsAsync(new DuplicateValueInEntityException("User", "OfficialID", user.Id.Value));


        var sut = new UserRepository(dbContextMock.Object);


        // Act
        var exception = await Assert.ThrowsAsync<DuplicateValueInEntityException>(
            () => sut.CreateUserAsync(user));
        // Assert
        exception.Message.Should().Contain("already exists", because: "the exception should indicate a duplicate key violation");


    }

    private static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var dbSetMock = data.BuildMockDbSet();

        dbSetMock
            .As<IQueryable<T>>()
            .Setup(dbSet => dbSet.Provider)
            .Returns(queryableData.Provider);
        dbSetMock
            .As<IQueryable<T>>()
            .Setup(dbSet => dbSet.Expression)
            .Returns(queryableData.Expression);
        dbSetMock
            .As<IQueryable<T>>()
            .Setup(dbSet => dbSet.ElementType)
            .Returns(queryableData.ElementType);
        dbSetMock
            .As<IQueryable<T>>()
            .Setup(dbSet => dbSet.GetEnumerator())
            .Returns(queryableData.GetEnumerator());

        dbSetMock
            .As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryableData.GetEnumerator()));

        return dbSetMock;
    }
}
