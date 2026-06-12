using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Frontend.Application.Users.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.Users.Services;

/// <summary>
/// Unit tests for the UserService class.
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<AuthenticationStateProvider> _authenticationStateProviderMock;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _authenticationStateProviderMock = new Mock<AuthenticationStateProvider>();
        _sut = new UserService(_userRepositoryMock.Object, _authenticationStateProviderMock.Object);
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenRepositoryReturnsUser_ReturnsUser()
    {
        // Arrange
        var azureObjectIdentifier = "12345678-1234-1234-1234-123456789abc";
        var user = new User(
            UserId.Create("test-id-1234"),
            UserName.Create("Test User"),
            true,
            Email.Create("test@example.com"));

        _userRepositoryMock
            .Setup(x => x.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync((user, null));

        // Act
        var (result, errorMessage) = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        // Arrange
        var azureObjectIdentifier = "non-existent-oid";

        _userRepositoryMock
            .Setup(x => x.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync((null, null));

        // Act
        var (result, errorMessage) = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenRepositoryReturnsError_ReturnsError()
    {
        // Arrange
        var azureObjectIdentifier = "test-oid";
        var expectedError = "Repository error occurred";

        _userRepositoryMock
            .Setup(x => x.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync((null, expectedError));

        // Act
        var (result, errorMessage) = await _sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUserAsync_WhenUserIsAuthenticated_ReturnsUser()
    {
        // Arrange
        var azureObjectIdentifier = "12345678-1234-1234-1234-123456789abc";
        var user = new User(
            UserId.Create("test-id-1234"),
            UserName.Create("Test User"),
            true,
            Email.Create("test@example.com"));

        var claims = new[]
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", azureObjectIdentifier)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(principal);

        _authenticationStateProviderMock
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        _userRepositoryMock
            .Setup(x => x.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier))
            .ReturnsAsync((user, null));

        // Act
        var result = await _sut.GetCurrentUserAsync();

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetCurrentUserAsync_WhenUserIsNotAuthenticated_ReturnsNull()
    {
        // Arrange
        var identity = new ClaimsIdentity(); // Not authenticated
        var principal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(principal);

        _authenticationStateProviderMock
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        // Act
        var result = await _sut.GetCurrentUserAsync();

        // Assert
        result.Should().BeNull();
        _userRepositoryMock.Verify(x => x.GetUserByAzureObjectIdentifierAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetCurrentUserAsync_WhenAzureObjectIdentifierMissing_ReturnsNull()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test User")
            // No Azure Object Identifier claim
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(principal);

        _authenticationStateProviderMock
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        // Act
        var result = await _sut.GetCurrentUserAsync();

        // Assert
        result.Should().BeNull();
        _userRepositoryMock.Verify(x => x.GetUserByAzureObjectIdentifierAsync(It.IsAny<string>()), Times.Never);
    }
}
