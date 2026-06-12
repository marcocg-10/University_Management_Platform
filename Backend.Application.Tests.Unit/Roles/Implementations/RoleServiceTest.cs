using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Roles.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Roles.Implementations;

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
           .ReturnsAsync((Role r) => r);

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var created = await sut.CreateRoleAsync(role);

        // Assert
        created.Should().NotBeNull(because: "a valid role should be returned");
        created.Should().BeEquivalentTo(role, because: "the created role should be the same as the input");

        repositoryMock.Verify(
            r => r.CreateRoleAsync(It.IsAny<Role>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the CreateRoleAsync method of RoleService to ensure it propagates repository exceptions.
    /// </summary>
    [Fact]
    public async Task CreateRoleAsync_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        var roleName = RoleName.Create("User");
        var role = new Role(roleName);

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
           .Setup(r => r.CreateRoleAsync(It.IsAny<Role>()))
           .ThrowsAsync(new RoleInvalidDataException("duplicate"));

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var act = async () => await sut.CreateRoleAsync(role);

        // Assert
        await act.Should().ThrowAsync<RoleInvalidDataException>();
        repositoryMock.Verify(
            r => r.CreateRoleAsync(It.IsAny<Role>()),
            Times.Once);
    }

    /// <summary>
    /// Tests AssociatePermissionAsync method of RoleService to ensure it saves the correct amount of changes
    /// </summary>
    [Fact]
    public async Task AssociatePermissionAsync_WhenGivenValidParameters_ShouldSaveChanges()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var permissionName = PermissionName.Create("Admin");
        var permission = new Permission(permissionName);

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()))
            .ReturnsAsync((Role r, Permission p) => (role, permission));

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var (roleResult, permissionResult) = await sut.AssociatePermissionAsync(role, permission);

        
        // Assert
        roleResult.Should().NotBeNull(because: "a change should be saved");
        permissionResult.Should().NotBeNull(because: "a change should be saved");
        roleResult.Should().BeEquivalentTo(role, because: "the returned role should match the input role");
        permissionResult.Should().BeEquivalentTo(permission, because: "the returned permission should match the input permission");

        repositoryMock.Verify(
            r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()),
            Times.Once);
    }

    /// <summary>
    /// Tests AssociatePermissionAsync method of RoleService to ensure it handles repository errors as expected
    /// </summary>
    [Fact]
    public async Task AssociatePermissionAsync__WhenRoleNotFound_ShouldReturnException()
    {
        // Arrange
        var roleName = RoleName.Create("Admin");
        var role = new Role(roleName);

        var permissionName = PermissionName.Create("Admin");
        var permission = new Permission(permissionName);

        var errorMessage = "Failed to associate the role with the permission";

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.AssociatePermissionAsync(It.IsAny<Role>(), It.IsAny<Permission>()))
            .ThrowsAsync(new RoleNotFoundException(role.Name));

        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = async() => await sut.AssociatePermissionAsync(role, permission);

        // Assert
        await result.Should().ThrowAsync<RoleNotFoundException>();
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
        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRolesAsync())
            .ReturnsAsync((new List<Role> { role1, role2 }));
        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRolesAsync();

        // Assert
        result.Should().NotBeNull(because: "there should be a list of roles");
        result.Should().HaveCount(2, because: "two roles were added to the repository");
        
        repositoryMock.Verify(
            r => r.GetRolesAsync(),
            Times.Once);
    }

    /// <summary>
    /// Tests the GetRolesAsync method of RoleService to ensure it handles repository errors as expected.
    /// </summary>
    [Fact]
    public async Task GetRolesAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var errorMessage = "Failed to retrieve roles";

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRolesAsync())
            .ThrowsAsync(new Exception(errorMessage));
        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = async () => await sut.GetRolesAsync();

        // Assert
        await result.Should().ThrowAsync<Exception>();
        repositoryMock.Verify(
            r => r.GetRolesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetRoleFromIdAsync_WhenCalled_ShouldReturnRole()
    {
        // Arrange
        var role = new Role(RoleName.Create("Admin"));
 
        var repositoryMock = new Mock<IRoleRepository>();

        repositoryMock
            .Setup(r => r.GetRoleFromIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => (role, null));
        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRoleFromIdAsync(1);

        // Assert
        result.maybeRole.Should().NotBeNull(because: "there should be a role");
        result.maybeRole.Should().Be(role, because: "there is one role with that id");
        result.errorMessage.Should().BeNull(because: "there should be no error message");

        repositoryMock.Verify(
            r => r.GetRoleFromIdAsync(It.IsAny<int>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the GetRolesAsync method of RoleService to ensure it handles repository errors as expected.
    /// </summary>
    [Fact]
    public async Task GetRoleFromIdAsync_WhenRepositoryReturnsError_ShouldReturnErrorMessage()
    {
        // Arrange
        var errorMessage = "Role with id 1 does not exist";

        var repositoryMock = new Mock<IRoleRepository>();
        repositoryMock
            .Setup(r => r.GetRoleFromIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => (null, errorMessage));
        var sut = new RoleService(repositoryMock.Object);

        // Act
        var result = await sut.GetRoleFromIdAsync(1);

        // Assert
        result.maybeRole.Should().BeNull(because: "no role should be returned upon failure");
        result.errorMessage.Should().NotBeNull(because: "there should be an error message for a failure");
        result.errorMessage.Should().Be(errorMessage, because: "the error message should match the repository's error");

        repositoryMock.Verify(
            r => r.GetRoleFromIdAsync(It.IsAny<int>()),
            Times.Once);
    }
}
