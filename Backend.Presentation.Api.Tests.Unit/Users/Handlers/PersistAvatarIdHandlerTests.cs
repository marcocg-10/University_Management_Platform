using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Backend.Application.Users.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Handlers;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Users.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Api.Tests.Unit.Users.Handlers;

public class PersistAvatarIdHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidDto_AndUserClaim_Should_Persist_And_Return_Ok()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var claims = new[] { new Claim("oid", "oid-123") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        var user = new User(
            UserId.Create("u1dfafafaf1"),
            UserName.Create("John"),
            true,
            Email.Create("john@doe.com"),
            "oid-123");
        // Initialize IdKey via repository test data conventions if needed
        typeof(User).GetProperty("IdKey")!.SetValue(user, 1);

        userService.Setup(s => s.GetUserByAzureObjectIdentifierAsync("oid-123")).ReturnsAsync(user);
        userService.Setup(s => s.SaveAvatarId(1, It.IsAny<AvatarId>())).Returns(Task.CompletedTask);

        var dto = new AvatarIdDto("rpm-123");

        // Act
        var result = await PersistAvatarIdHandler.HandleAsync(userService.Object, httpContext, dto);

        // Assert
        result.Result.Should().BeOfType<Ok<SuccesfulPersistAvatarIdResponse>>();
    }

    [Fact]
    public async Task HandleAsync_WithInvalidDto_Should_Return_BadRequest()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var httpContext = new DefaultHttpContext();
        var dto = new AvatarIdDto("");

        // Act
        var result = await PersistAvatarIdHandler.HandleAsync(userService.Object, httpContext, dto);

        // Assert
        result.Result.Should().BeOfType<BadRequest<ErrorPersistAvatarIdResponse>>();
    }
}
