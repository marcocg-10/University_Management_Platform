using System.Security.Claims;
using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Authentication.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Permissions.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Roles.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Authentication.Services.Implementations;

public class PermissionInjectorServiceTests
{
    [Fact]
    public async Task TransformAsync_WhenValid_AddsPermissionClaimsToPrincipal()
    {
        // Arrange
        var azureObjectId = "12345678-1234-1234-1234-123456789";
        var user = new User(
            UserId.Create("t12345"),
            UserName.Create("Test User"),
            isActive: true,
            Email.Create("testuser@email.com"),
            azureObjectId);

        var permissions = new List<Permission>
        {
            new Permission(PermissionName.Create("ListUsers")),
            new Permission(PermissionName.Create("EditUsers"))
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(r => r.GetUserByAzureObjectIdentifierAsync(azureObjectId))
            .ReturnsAsync(user);
        userRepositoryMock
            .Setup(p => p.GetCurrentUserPermissionsAsync(user.IdKey))
            .ReturnsAsync(permissions);

        var sut = new PermissionInjectorService(userRepositoryMock.Object);

        var claims = new List<Claim>
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", azureObjectId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var transformedPrincipal = await sut.TransformAsync(principal);

        // Assert
        var transformedIdentity = transformedPrincipal.Identity as ClaimsIdentity;
        transformedIdentity!
            .HasClaim(claim => claim.Type == "extension_Permissions" && claim.Value == "ListUsers")
            .Should().BeTrue(because: "the ListUsers permission claim should be added");
    }
    
    [Fact]
    public async Task TransformAsync_WhenUserNotFound_DoesNotAddPermissionClaims()
    {
        // Arrange
        var azureObjectId = "invalid-oid"; 
        User? nullUser = null;
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(r => r.GetUserByAzureObjectIdentifierAsync(azureObjectId))
            .ReturnsAsync(nullUser);
        var sut = new PermissionInjectorService(userRepositoryMock.Object);
        var claims = new List<Claim>
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", azureObjectId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        
        // Act
        var transformedPrincipal = await sut.TransformAsync(principal);
        
        // Assert
        var transformedIdentity = transformedPrincipal.Identity as ClaimsIdentity;
        transformedIdentity!
            .HasClaim(claim => claim.Type == "extension_Permissions")
            .Should().BeFalse(because: "no permission claims should be added when user is not found");
    }

    [Fact]
    public async Task TransformAsync_WhenExceptionTriggered_ThrowsException()
    {

        // Arrange
        var azureObjectId = "12345678";
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(r => r.GetUserByAzureObjectIdentifierAsync(azureObjectId))
            .ThrowsAsync(new Exception("Exception Mock"));
        var sut = new PermissionInjectorService(userRepositoryMock.Object);
        var claims = new List<Claim>
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", azureObjectId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        // Act
        Func<Task> act = async () => { await sut.TransformAsync(principal); };

        // Assert
        await act.Should().ThrowAsync<Exception>()
             .WithMessage("An error occurred while transforming claims: Exception Mock");
    }
}