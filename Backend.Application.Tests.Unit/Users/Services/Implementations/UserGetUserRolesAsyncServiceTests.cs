using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Users.Services.Implementations;

/// <summary>
/// Contains unit tests for the <see cref="UserService"/> class, verifying the correct behavior of its methods
/// and the interaction with the underlying repository.
/// </summary>
/// <remarks>This class includes tests to ensure that the <see cref="UserService"/> methods correctly delegate
/// to the repository and handle various scenarios including successful operations, edge cases, and error conditions.</remarks>
public class UserGetUserRolesAsyncServiceTests
{
    [Fact]
    public async Task GetUserRolesAsync_WhenCalled_ShouldReturnAllRolesForUser()
    {
        // Arrange
        const int userId = 1;
        var rolesFromRepo = new List<Role>
        {
            new Role(RoleName.Create("Admin")),
            new Role(RoleName.Create("User"))
        };

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(r => r.GetUserRolesAsync(userId))
            .ReturnsAsync(rolesFromRepo as IEnumerable<Role>);

        var sut = new UserService(repositoryMock.Object);

        // Act
        var roles = await sut.GetUserRolesAsync(userId);

        // Assert
        roles.Should().NotBeNull();
        roles!.Should().BeEquivalentTo(rolesFromRepo, because: "the repository returned the expected roles");
    }

    [Fact]
    public async Task GetUserRolesAsync_WhenNoRoles_ReturnsEmptyCollection()
    {
        // Arrange
        const int userId = 2;
        var emptyRoles = new List<Role>();

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(r => r.GetUserRolesAsync(userId))
            .ReturnsAsync(emptyRoles as IEnumerable<Role>);

        var sut = new UserService(repositoryMock.Object);

        // Act
        var roles = await sut.GetUserRolesAsync(userId);

        // Assert
        roles.Should().NotBeNull();
        roles!.Should().BeEmpty(because: "the repository returned an empty list of roles");
    }

    [Fact]
    public async Task GetUserRolesAsync_WhenRepositoryThrows_ExceptionBubblesUp()
    {
        // Arrange
        const int userId = 3;

        var repositoryMock = new Mock<IUserRepository>();
        repositoryMock
            .Setup(r => r.GetUserRolesAsync(userId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var sut = new UserService(repositoryMock.Object);

        // Act / Assert
        await sut.Invoking(s => s.GetUserRolesAsync(userId)).Should().ThrowAsync<InvalidOperationException>();
    }
}