using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace UCR.ECCI.PI.ThemePark.Backend.Api.Tests.Unit.ApiAuth;

public class EndpointAuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly EndpointTestHelper _apiTestHelper;

    public EndpointAuthTests(WebApplicationFactory<Program> factory)
    {
        _apiTestHelper = new EndpointTestHelper(factory);
    }

    [Fact]
    public async Task RequireAuthorization_WithValidToken_ShouldSucceed()
    {
        // Arrange
        var client = _apiTestHelper.CreateAuthenticatedClient();

        // Act
        var response = await client.GetAsync("/mock-auth-test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, because: "the access token is valid");
    }

    [Fact]
    public async Task RequireAuthorization_WithInvalidToken_ShouldBeUnauthorized()
    {
        // Arrange
        var client = _apiTestHelper.CreateUnauthenticatedClient();

        // Act
        var response = await client.GetAsync("/mock-auth-test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, because: "the access token is invalid");
    }

    [Fact]
    public async Task RequireAuthorization_WithMissingToken_ShouldBeUnauthorized()
    {
        // Arrange
        var client = _apiTestHelper.CreateUnauthenticatedClient();

        // Act
        var response = await client.GetAsync("/mock-auth-test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, because: "there is no access token");
    }

    [Fact]
    public async Task RequireAuthorization_WithExpiredToken_ShouldBeUnauthorized()
    {
        // Arrange
        var client = _apiTestHelper.CreateExpiredTokenClient();

        // Act
        var response = await client.GetAsync("/mock-auth-test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, because: "the access token is expired");
    }
}
