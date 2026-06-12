using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Users.Services.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="UserService"/> class, verifying the correct behavior of its methods
/// and the interaction with the underlying repository.
/// </summary>
/// <remarks>This class includes tests to ensure that the <see cref="UserService"/> methods correctly delegate
/// to the repository and handle various scenarios including successful operations, edge cases, and error conditions.</remarks>
public class UserServiceTests
{
    /// <summary>
    /// Tests that <see cref="UserService.GetUserByAzureObjectIdentifierAsync"/> returns the expected user when
    /// the repository finds a matching Azure Object Identifier.
    /// </summary>
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _sut = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        const string azureObjectIdentifier = "12345678-1234-1234-1234-123456789abc";
        var expectedUser = new User(
            UserId.Create("t12345"),
            UserName.Create("Test User"),
            isActive: true,
            Email.Create("testuser@email.com"),
            azureObjectIdentifier);

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repository => repository.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync(expectedUser);

        var sut = new UserService(repositoryMock.Object);

        // Act
        var user = await sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        user.Should().BeEquivalentTo(expectedUser, because: "the repository returned the expected user");
    }

    /// <summary>
    /// Tests that <see cref="UserService.GetUserByAzureObjectIdentifierAsync"/> returns null when
    /// the repository does not find a user with the specified Azure Object Identifier.
    /// </summary>
    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        const string azureObjectIdentifier = "nonexistent-oid-1234-5678-9abc";

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(repository => repository.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync((User)null);

        var sut = new UserService(repositoryMock.Object);

        // Act
        var user = await sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        user.Should().BeNull(because: "no user exists with the specified Azure Object Identifier");
    }

    /// <summary>
    /// Tests that <see cref="UserService.GetUserByAzureObjectIdentifierAsync"/> correctly handles
    /// and passes through null or empty Azure Object Identifier values.
    /// </summary>
    [Theory]
    [InlineData(null)]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenGivenInvalidIdentifier_CheckCorrectly(string? invalidIdentifier)
    {
        // Arrange
        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
             .Setup(repository => repository.GetUserByAzureObjectIdentifierAsync(It.IsAny<string>()))
             .ReturnsAsync((User)null);


        var sut = new UserService(repositoryMock.Object);

        // Act
        var user = await sut.GetUserByAzureObjectIdentifierAsync(invalidIdentifier!);

        // Assert
        user.Should().BeNull(because: "invalid identifiers should not match any user");
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenRepositoryReturnsPermissions_ReturnsPermissions()
    {
        // Arrange
        var userId = 1;
        var permissions = new List<Permission>
        {
            new Permission(PermissionName.Create("ReadUsers")),
            new Permission(PermissionName.Create("WriteUsers"))
        };

        _userRepositoryMock
            .Setup(x => x.GetCurrentUserPermissionsAsync(userId))
            .ReturnsAsync(permissions);

        // Act
        var result = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(permissions);
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenRepositoryReturnsError_ReturnsError()
    {
        // Arrange
        var userId = 1;
        var expectedError = "Repository error occurred";

        _userRepositoryMock
            .Setup(x => x.GetCurrentUserPermissionsAsync(userId))
            .ReturnsAsync((IEnumerable<Permission>)null);

        // Act
        var result = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenRepositoryReturnsEmptyPermissions_ReturnsEmpty()
    {
        // Arrange
        var userId = 1;
        var emptyPermissions = new List<Permission>();

        _userRepositoryMock
            .Setup(x => x.GetCurrentUserPermissionsAsync(userId))
            .ReturnsAsync(emptyPermissions);

        // Act
        var result = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        result.Should().BeEmpty();
    }
}
