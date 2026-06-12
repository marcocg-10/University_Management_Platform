using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Permissions.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Permissions.Services.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="PermissionService"/> class, specifically focusing on the creation of
/// permissions.
/// </summary>
/// <remarks>This test class verifies the behavior of the <see cref="PermissionService.CreatePermissionAsync"/>
/// method under various conditions. It ensures that valid permissions are created correctly and that the repository is
/// called as expected.</remarks>
public class PermissionServiceTests
{
    /// <summary>
    /// Tests that the <see cref="PermissionService.CreatePermissionAsync"/> method creates a permission when provided
    /// with valid parameters.
    /// </summary>
    /// <remarks>This test verifies that the method returns a non-null permission object equivalent to the
    /// input and ensures that the repository's <see cref="IPermissionRepository.CreatePermissionAsync"/> method is
    /// called exactly once.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task CreatePermissionAsync_WhenGivenValidParameters_ShouldCreatePermission()
    {
        // Arrange
        var permission = new Permission(PermissionName.Create("CreateRole"));

        var repositoryMock = new Mock<IPermissionRepository>();
        repositoryMock
           .Setup(r => r.CreatePermissionAsync(It.IsAny<Permission>()))
           .ReturnsAsync((Permission p) => p);

        var sut = new PermissionService(repositoryMock.Object);

        // Act
        var createPermission = await sut.CreatePermissionAsync(permission);

        // Assert
        createPermission.Should().NotBeNull(because: "a valid permission should be returned");
        createPermission.Should().BeEquivalentTo(permission, because: "the created permission should be the same as the input");

        repositoryMock.Verify(
            r => r.CreatePermissionAsync(It.IsAny<Permission>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that the <see cref="PermissionService.GetAllPermissionsAsync"/> method returns the same set of
    /// permissions provided by the underlying repository.
    /// </summary>
    /// <remarks>This test ensures that the <see cref="IPermissionRepository.GetAllPermissionsAsync"/>
    /// implementation is correctly utilized by the <see cref="PermissionService"/> to retrieve all permissions without
    /// modification.</remarks>
    /// <returns></returns>
    [Fact]
    public async Task GetAllPermissionsAsync_WhenRepositoryGivesData_ReturnsSame()
    {
        var registeredPermssions = new List<Permission>
        {
            new Permission(PermissionName.Create("CreateRole")),
            new Permission(PermissionName.Create("DeleteRole")),
            new Permission(PermissionName.Create("UpdateRole")),
        };
        var repositoryMock = new Mock<IPermissionRepository>();
        repositoryMock
           .Setup(r => r.GetAllPermissionsAsync())
           .ReturnsAsync(registeredPermssions);

        var sut = new PermissionService(repositoryMock.Object);

        var permissions = await sut.GetAllPermissionsAsync();

        permissions.Should().BeEquivalentTo(registeredPermssions, because: "repository has data");
    }
}
