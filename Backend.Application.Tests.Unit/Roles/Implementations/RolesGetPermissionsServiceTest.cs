using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Roles.Implementations;

/// <summary>
/// Contains unit tests for the method GetPermissionsFromRoleAsync <see cref="RoleService"/> class.
/// </summary>
public class RolesGetPermissionsServiceTest
{
    [Fact]
    public async Task GetPermissionsFromRoleAsync_WhenCalled_ShouldReturnAllPermissionsForRole()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);
        var permission1 = new Permission(PermissionName.Create("ManageUsers"));
        var permission2 = new Permission(PermissionName.Create("ViewReports"));
        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => (new List<Permission> { permission1, permission2 }, null));
        var sut = new RoleService(repositoryMock.Object);
        // Act
        var result = await sut.GetRolePermissionsAsync(role.Id);
        // Assert
        result.permissions.Should().NotBeNull(because: "there should be a list of permissions");
        result.permissions.Should().HaveCount(2, because: "two permissions were associated with the role");
        result.errorMessage.Should().BeNull(because: "there should be no error message");
        repositoryMock.Verify(
            r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPermissionsFromRoleAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var errorMessage = "Failed to retrieve permissions for role";
        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()))
            .ReturnsAsync((null, errorMessage));
        var sut = new RoleService(repositoryMock.Object);
        // Act
        var result = await sut.GetRolePermissionsAsync(1);
        // Assert
        result.permissions.Should().BeNull(because: "no permissions should be returned upon failure");
        result.errorMessage.Should().NotBeNull(because: "there should be an error message for a failure");
        result.errorMessage.Should().Be(errorMessage, because: "the error message should match the repository's error");
        repositoryMock.Verify(
            r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()),
            Times.Once);
    }
    [Fact]
    public async Task GetPermissionsFromRoleAsync_WhenRoleHasNoPermissions_ShouldReturnEmptyList()
    {
        // Arrange
        var roleName = RoleName.Create("Guest");
        var role = new Role(roleName);
        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()))
            .ReturnsAsync((new List<Permission>(), null));
        var sut = new RoleService(repositoryMock.Object);
        // Act
        var result = await sut.GetRolePermissionsAsync(role.Id);
        // Assert
        result.permissions.Should().NotBeNull(because: "there should be a list of permissions, even if empty");
        result.permissions.Should().BeEmpty(because: "the role has no associated permissions");
        result.errorMessage.Should().BeNull(because: "there should be no error message");
        repositoryMock.Verify(
            r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()),
            Times.Once);
    }
    [Fact]
    public async Task GetPermissionsFromRoleAsync_WhenRoleDoesNotExist_ShouldReturnErrorMessage()
    {
        // Arrange
        var errorMessage = "Role with id 999 does not exist";
        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()))
            .ReturnsAsync((null, errorMessage));
        var sut = new RoleService(repositoryMock.Object);
        // Act
        var result = await sut.GetRolePermissionsAsync(999);
        // Assert
        result.permissions.Should().BeNull(because: "no permissions should be returned for a non-existent role");
        result.errorMessage.Should().NotBeNull(because: "there should be an error message for a failure");
        result.errorMessage.Should().Be(errorMessage, because: "the error message should match the repository's error");
        repositoryMock.Verify(
            r => r.GetPermissionsFromRoleIdAsync(It.IsAny<int>()),
            Times.Once);
    }

}