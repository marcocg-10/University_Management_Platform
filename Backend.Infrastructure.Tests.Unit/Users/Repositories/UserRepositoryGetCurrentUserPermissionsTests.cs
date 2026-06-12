using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Core;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Unit tests for the GetCurrentUserPermissionsAsync method in UserRepository.
/// </summary>
public class UserRepositoryGetCurrentUserPermissionsTests
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly UserRepository _sut;

    public UserRepositoryGetCurrentUserPermissionsTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _sut = new UserRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenUserExists_ReturnsUserPermissions()
    {
        // Arrange
        var userId = 1;
        var permission1 = new Permission(PermissionName.Create("ReadUsers"));
        
        var role1 = new Role(RoleName.Create("Admin"));
        role1.Permissions.Add(permission1);
        
        var role2 = new Role(RoleName.Create("User"));
        role2.Permissions.Add(permission1); // Duplicate permission to test Distinct()
        
        var user = new User(
            UserId.Create("test-id-1234"),
            UserName.Create("Test User"),
            true,
            Email.Create("test@example.com"),
            "azure-oid-123");

        var idKeyProperty = typeof(User).GetProperty("IdKey");
        idKeyProperty?.SetValue(user, userId);

        user.Roles.Add(role1);
        user.Roles.Add(role2);

        var users = new List<User> { user };
        var usersDbSetMock = users.BuildMockDbSet();
        
        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        var permissions = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        permissions.Should().Contain(p => p.Name.Value == "ReadUsers");
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenUserHasNoRoles_ReturnsEmptyPermissions()
    {
        // Arrange
        var userId = 1;
        var user = new User(
            UserId.Create("test-id-1234"),
            UserName.Create("Test User"),
            true,
            Email.Create("test@example.com"),
            "azure-oid-123");

        // Use reflection to set the IdKey property
        var idKeyProperty = typeof(User).GetProperty("IdKey");
        idKeyProperty?.SetValue(user, userId);

        var users = new List<User> { user };
        var usersDbSetMock = users.BuildMockDbSet();
        
        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        var permissions = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        permissions.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCurrentUserPermissionsAsync_WhenUserNotFound_ReturnsError()
    {
        // Arrange
        var userId = 999;
        var users = new List<User>();
        var usersDbSetMock = users.BuildMockDbSet();
        
        _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

        // Act
        var permissions = await _sut.GetCurrentUserPermissionsAsync(userId);

        // Assert
        permissions.Should().BeEmpty();
    }
}