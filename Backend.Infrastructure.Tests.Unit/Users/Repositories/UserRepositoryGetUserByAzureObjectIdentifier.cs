using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Unit tests for the GetUserByAzureObjectIdentifierAsync method in UserRepository.
/// </summary>
public class UserRepositoryGetUserByAzureObjectIdentifierTests
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly UserRepository _sut;

    public UserRepositoryGetUserByAzureObjectIdentifierTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _sut = new UserRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var azureObjectIdentifier = "12345678-1234-1234-1234-123456789abc";
        var user = new User(
            UserId.Create("test-id-1234"),
            UserName.Create("Test User"),
            true,
            Email.Create("test@example.com"),
            azureObjectIdentifier);

        var users = new List<User> { user };
        var usersDbSetMock = users.BuildMockDbSet();

        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        var foundUser = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        foundUser.Name.Value.Should().Be("Test User");
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenUserNotFound_ReturnsNull()
    {
        // Arrange
        var azureObjectIdentifier = "non-existent-oid";
        var users = new List<User>();
        var usersDbSetMock = users.BuildMockDbSet();

        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        var foundUser = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        foundUser.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenAzureObjectIdentifierIsNull_ReturnsError()
    {
        // Arrange
        var azureObjectIdentifier = "non-existent-oid";
        var users = new List<User>();
        var usersDbSetMock = users.BuildMockDbSet();

        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        User? foundUser = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier!);

        // Assert
        foundUser.Should().BeNull(because: "the Azure Object Identifier provided is null");
    }
}