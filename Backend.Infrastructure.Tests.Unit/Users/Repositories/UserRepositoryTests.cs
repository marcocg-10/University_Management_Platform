using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Core.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Provides unit tests for the <see cref="UserRepository"/> class, focusing on methods that interact with user data in
/// the database.
/// </summary>
public class UserRepositoryTests : IClassFixture<UserRepositoryTestData>
{
    private readonly UserRepositoryTestData _testData;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class with the specified test data.
    /// </summary>
    /// <param name="testData">The test data to be used for the user repository tests. Cannot be <see langword="null"/>.</param>
    public UserRepositoryTests(UserRepositoryTestData testData)
    {
        _testData = testData;
    }


    /// <summary>
    /// Tests that <see cref="UserRepository.GetActiveUsersAsync"/> returns an empty enumerable when the database
    /// contains no active users.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetActiveUsersAsync_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var usersDbSetMock = _testData.EmptyData.BuildMockDbSet(); 
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.GetActiveUsersAsync();

        // Assert
        users.Should().BeEmpty(because: "There are no active users in the database");
    }

    /// <summary>
    /// Tests that the <see cref="UserRepository.GetActiveUsersAsync"/> method returns the expected data when the
    /// database contains a single active user entry.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetActiveUsersAsync_WhenGivenSingleEntryData_ReturnsData()
    {
        // Arrange
        var usersDbSetMock = _testData.SingleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.GetActiveUsersAsync();

        // Assert
        users.Should().BeEquivalentTo(
            _testData.SingleEntryData,
            because: "There is one active user in the database");
    }

    /// <summary>
    /// Tests the <see cref="UserRepository.GetActiveUsersAsync"/> method to ensure it returns the expected data when
    /// the database contains multiple user entries.
    /// </summary>
    /// simulate the data and asserts that the returned users match the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetActiveUsersAsync_WhenGivenMultipleEntryData_ReturnsData()
    {
        // Arrange
        var usersDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.GetActiveUsersAsync();

        // Assert
        users.Should().BeEquivalentTo(
            _testData.MultipleEntryData,
            because: "There are multiple active users in the database");
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
        var usersDbSetMock = _testData.EmptyData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Provider)
            .Returns(usersDbSetData.AsQueryable().Provider);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Expression)
            .Returns(usersDbSetData.AsQueryable().Expression);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.ElementType)
            .Returns(usersDbSetData.AsQueryable().ElementType);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.GetEnumerator())
            .Returns(usersDbSetData.AsQueryable().GetEnumerator());

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
        var user = new User(
            UserId.Create("9edf-8ac-8b32-bda"),
            UserName.Create("John Doe"),
            isActive: true,
            Email.Create("john.doe@universitry.com"),
            "12345678-1234-1234-1234-123456789abc");

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
        var usersDbSetMock = _testData.SingleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Provider)
            .Returns(usersDbSetData.AsQueryable().Provider);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Expression)
            .Returns(usersDbSetData.AsQueryable().Expression);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.ElementType)
            .Returns(usersDbSetData.AsQueryable().ElementType);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.GetEnumerator())
            .Returns(usersDbSetData.AsQueryable().GetEnumerator());

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
        var user = new User(
            UserId.Create("9edf-8ac-8b32a"),
            UserName.Create("Jane Doe"),
            isActive: true,
            Email.Create("jane.doe@notuniversitry.com"),
            "87654321-4321-4321-4321-abcdef123456");

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
    public async Task AddUserAsync_WhenGivenDuplicateData_DoesntChange()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.SingleEntryData);
        var usersDbSetMock = _testData.SingleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();

        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Provider)
            .Returns(usersDbSetData.AsQueryable().Provider);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Expression)
            .Returns(usersDbSetData.AsQueryable().Expression);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.ElementType)
            .Returns(usersDbSetData.AsQueryable().ElementType);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.GetEnumerator())
            .Returns(usersDbSetData.AsQueryable().GetEnumerator());

        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        
        User? pendingUser = null;
        dbContextMock
            .Setup(dbContext => dbContext.Users.Add(It.IsAny<User>()))
            .Callback<User>(user => pendingUser = user);
        
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DuplicateValueInEntityException("User", "Id", "9edf-8ac7-4a45-8b32-bdabe"));

        var sut = new UserRepository(dbContextMock.Object);
        var user = new User(
            UserId.Create("9edf-8ac-8b32-bda"),
            UserName.Create("John Doe"),
            isActive:true,
            Email.Create("john.doe@universitry.com"),
            "12345678-1234-1234-1234-123456789abc");

        // Act
        var exception = await Assert.ThrowsAsync<DuplicateValueInEntityException>(
            () => sut.CreateUserAsync(user));

        // Assert

        exception.Message.Should().Contain(
            "Id",
            because: "the exception should indicate the duplicate Id");
        }

    /// <summary>
    /// Tests that duplicate Azure Object Identifier prevents user creation.
    /// </summary>
    [Fact]
    public async Task AddUserAsync_WhenGivenDuplicateAzureObjectIdentifier_DoesntChange()
    {
        // Arrange
        var usersDbSetData = new List<User>(_testData.SingleEntryData);
        var usersDbSetMock = _testData.SingleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        // Different user but SAME Azure Object Identifier as existing user
        var user = new User(
            UserId.Create("1234-5678-9abc"),
            UserName.Create("Different User"),
            isActive: true,
            Email.Create("different@email.com"),
            "12345678-1234-1234-1234-123456789abc"); // Same Azure OID as existing user

        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Provider)
            .Returns(usersDbSetData.AsQueryable().Provider);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.Expression)
            .Returns(usersDbSetData.AsQueryable().Expression);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.ElementType)
            .Returns(usersDbSetData.AsQueryable().ElementType);
        usersDbSetMock
            .As<IQueryable<User>>()
            .Setup(dbSet => dbSet.GetEnumerator())
            .Returns(usersDbSetData.AsQueryable().GetEnumerator());

        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        dbContextMock
            .Setup(dbContext => dbContext.Users.Add(It.IsAny<User>()))
            .Callback<User>(user => usersDbSetData.Add(user));
        dbContextMock
            .Setup(dbContext => dbContext.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DuplicateValueInEntityException("User", "Azure Object Identifier", user.AzureObjectIdentifier!));

            

        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<DuplicateValueInEntityException>(
            () => sut.CreateUserAsync(user));

        // Assert
        exception.Message.Should().Contain(
            "Azure Object Identifier",
            because: "the exception should indicate the duplicate Azure Object Identifier");
        }
}
