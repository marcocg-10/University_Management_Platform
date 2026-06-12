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
public class UserRepositorySearchUsersByIdTests : IClassFixture<UserRepositoryTestData>
{
    private readonly UserRepositoryTestData _testData;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class with the specified test data.
    /// </summary>
    /// <param Id="testData">The test data to be used for the user repository tests. Cannot be <see langword="null"/>.</param>
    public UserRepositorySearchUsersByIdTests(UserRepositoryTestData testData)
    {
        _testData = testData;
    }


    /// <summary>
    /// Tests that <see cref="UserRepository.SearchUsersByIdAsync(string)"/> returns an empty enumerable when the database
    /// contains no active users.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SearchUsersByIdAsyncc_WhenGivenNoData_ReturnsEmptyEnumerable()
    {
        // Arrange
        var usersDbSetMock = _testData.EmptyData.BuildMockDbSet(); 
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.SearchUsersAsync("Doesn't really matter, there is nothing!", 1, 10);

        // Assert
        users.Users.Should().BeEmpty(because: "There are no active users in the database");
    }

    /// <summary>
    /// Tests the <see cref="UserRepository.SearchUsersByIdAsync"/> method to ensure it returns the expected data when
    /// the database contains multiple user entries, one of which matches the query.
    /// </summary>
    /// simulate the data and asserts that the returned users match the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task SearchUsersByIdAsync_WhenOneMatch_ReturnsData()
    {
        // Arrange
        var usersDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.SearchUsersAsync("9edf-8ac-8b32-b", 1, 10);

        // Assert
        users.Users.Should().BeEquivalentTo(
            [_testData.MultipleEntryData[0]],
            because: "There are multiple active users in the database, but only one 9edf-8ac-8b32-bda");
    }

    /// <summary>
    /// Tests the <see cref="UserRepository.SearchUsersByIdAsync"/> method to ensure it returns the expected data when
    /// the database contains multiple user entries, one of which matches the query.
    /// </summary>
    /// simulate the data and asserts that the returned users match the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task SearchUsersByIdAsync_WhenSingleMatch_ReturnsData()
    {
        // Arrange
        var usersDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.SearchUsersAsync("9edf", 1, 10);

        // Assert
        users.Users.Should().BeEquivalentTo(
            _testData.MultipleEntryData,
            because: "There are multiple active users in the database, many of which have a 9edf");
    }

    /// <summary>
    /// Tests the <see cref="UserRepository.SearchUsersByIdAsync"/> method to ensure it returns the expected data when
    /// the database contains multiple user entries, one of which matches the query.
    /// </summary>
    /// simulate the data and asserts that the returned users match the expected data.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task SearchUsersByIdAsync_WhenNoMatch_ReturnsData()
    {
        // Arrange
        var usersDbSetMock = _testData.MultipleEntryData.BuildMockDbSet();
        var dbContextMock = new Mock<AppDbContext>();
        dbContextMock
            .Setup(dbContext => dbContext.Users)
            .Returns(usersDbSetMock.Object);
        var sut = new UserRepository(dbContextMock.Object);

        // Act
        var users = await sut.SearchUsersAsync("ඞ", 1, 10);

        // Assert
        users.Users.Should().BeEmpty(
            because: "There are multiple active users in the database, none of which ever will have a ඞ");
    }
}
