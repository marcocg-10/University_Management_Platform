using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Roles.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Entity;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.Roles.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="RoleService"/> class.
/// </summary>
public class RoleServiceTest
{
    /// <summary>
    /// Tests the CreateRoleAsync method of RoleService to ensure it creates a role successfully when given valid parameters.
    /// </summary>
    [Fact]
    public async Task CreateRoleAsync_WhenGivenValidParameters_ShouldCreateRole()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
           .Setup(r => r.CreateRoleAsync(It.IsAny<Role>()))
           .ReturnsAsync(role);

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.CreateRoleAsync(role);

        // Assert
        result.Should().NotBeNull(because: "a valid role should be returned");
        result.Should().BeEquivalentTo(role, because: "the created role should be the same as the input");
        
        repositoryMock.Verify(
            r => r.CreateRoleAsync(It.IsAny<Role>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the AssociatePermissionAsync method of RoleService to ensure it signals success (null error) when repository succeeds
    /// </summary>
    [Fact]
    public async Task AssociatePermissionAsync_WhenGivenValidParameters_ShouldReturnNullError()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var permissionName = PermissionName.Create("ManageRoles");
        var permission = new Permission(permissionName);

        var repositoryMock = new Mock<IRoleRepository>();
        // Repository returns null when successful (no error message)
        repositoryMock
            .Setup(r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()))
            .ReturnsAsync((string?)null);

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.AssociatePermissionAsync(role, permission);

        // Assert
        result.Should().BeNull(because: "there should be no error message for a successful association");

        repositoryMock.Verify(
            r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()),
            Times.Once);
    }

    /// <summary>
    /// Tests AssociatePermissionAsync method of RoleService to ensure it handles repository errors as expected
    /// </summary>
    [Fact]
    public async Task AssociatePermissionAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var permissionName = PermissionName.Create("ManageRoles");
        var permission = new Permission(permissionName);

        var errorMessage = "Failed to associate the role with the permission";

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()))
            .ReturnsAsync(errorMessage);

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.AssociatePermissionAsync(role, permission);

        // Assert
        result.Should().NotBeNull(because: "there should be an error message for a failure");
        result.Should().Be(errorMessage, because: "the error message should match the repository's error");

        repositoryMock.Verify(
            r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the GetRolesAsync method of RoleService to ensure it retrieves all roles correctly.
    /// </summary>
    [Fact]
    public async Task GetRolesAsync_WhenCalled_ShouldReturnAllRoles()
    {
        // Arrange
        var role1 = new Role(RoleName.Create("Admin"));
        var role2 = new Role(RoleName.Create("User"));
        var roles = new List<Role> { role1, role2 };

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRolesAsync())
            .ReturnsAsync(roles);

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRolesAsync();

        // Assert
        result.Should().NotBeNull(because: "there should be a list of roles");
        result.Should().HaveCount(2, because: "two roles were added to the repository");
        result.Should().BeEquivalentTo(roles, because: "the returned roles should match the repository's roles");

        repositoryMock.Verify(
            r => r.GetRolesAsync(),
            Times.Once);
    }

    /// <summary>
    /// Tests the GetRolePermissionsAsync method of RoleService to ensure it retrieves permissions correctly.
    /// </summary>
    [Fact]
    public async Task GetRolePermissionsAsync_WhenSuccessful_ShouldReturnPermissions()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var permission1 = new Permission(PermissionName.Create("ManageRoles"));
        var permission2 = new Permission(PermissionName.Create("ManageUsers"));
        var permissions = new List<Permission> { permission1, permission2 };

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRolePermissionsAsync(It.IsAny<Role>()))
            .ReturnsAsync(((IEnumerable<Permission>?)permissions, (string?)null));

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRolePermissionsAsync(role);

        // Assert
        result.permissions.Should().NotBeNull(because: "permissions should be returned");
        result.permissions.Should().HaveCount(2, because: "two permissions were set up");
        result.errorMessage.Should().BeNull(because: "there should be no error");

        repositoryMock.Verify(
            r => r.GetRolePermissionsAsync(It.IsAny<Role>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the GetRolePermissionsAsync method of RoleService to ensure it handles repository errors as expected.
    /// </summary>
    [Fact]
    public async Task GetRolePermissionsAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);
        var errorMessage = "Failed to retrieve role permissions";

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRolePermissionsAsync(It.IsAny<Role>()))
            .ReturnsAsync(((IEnumerable<Permission>?)null, errorMessage));

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRolePermissionsAsync(role);

        // Assert
        result.permissions.Should().BeNull(because: "no permissions should be returned upon failure");
        result.errorMessage.Should().NotBeNull(because: "there should be an error message");
        result.errorMessage.Should().Be(errorMessage, because: "the error message should match the repository's error");

        repositoryMock.Verify(
            r => r.GetRolePermissionsAsync(It.IsAny<Role>()),
            Times.Once);
    }
}
