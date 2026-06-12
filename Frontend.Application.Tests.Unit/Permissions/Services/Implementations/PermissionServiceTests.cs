using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Permissions.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Permissions.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.Permissions.Services.Implementations;

public class PermissionServiceTests
{
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

        var authStateProviderMock = new Mock<AuthenticationStateProvider>();
        var userServiceMock = new Mock<IUserService>();

        var sut = new PermissionService(repositoryMock.Object, authStateProviderMock.Object, userServiceMock.Object);

        var permissions = await sut.GetAllPermissionsAsync();
            
        permissions.Should().BeEquivalentTo(registeredPermssions, because: "repository has data");
    }
}

