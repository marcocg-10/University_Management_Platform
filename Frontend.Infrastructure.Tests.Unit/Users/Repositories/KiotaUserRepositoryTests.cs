using FluentAssertions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Users.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Users;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Users.Oid;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Users.Oid.Item;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Users.Repositories;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Tests.Unit.Users.Repositories;

/// <summary>
/// Unit tests for the KiotaUserRepository class.
/// </summary>
public class KiotaUserRepositoryTests
{
    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenAzureObjectIdentifierIsNull_ReturnsError()
    {
        // Arrange
        var mockRequestAdapter = new Mock<IRequestAdapter>();
        var apiClient = new ApiClient(mockRequestAdapter.Object);
        var sut = new KiotaUserRepository(apiClient);

        // Act
        var (user, errorMessage) = await sut.GetUserByAzureObjectIdentifierAsync(null!);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenAzureObjectIdentifierIsEmpty_ReturnsError()
    {
        // Arrange
        var mockRequestAdapter = new Mock<IRequestAdapter>();
        var apiClient = new ApiClient(mockRequestAdapter.Object);
        var sut = new KiotaUserRepository(apiClient);

        // Act
        var (user, errorMessage) = await sut.GetUserByAzureObjectIdentifierAsync(string.Empty);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByAzureObjectIdentifierAsync_WhenApiReturnsUser_ReturnsUser()
    {
        // Arrange
        var azureObjectIdentifier = "12345678-1234-1234-1234-123456789abc";
        var mockRequestAdapter = new Mock<IRequestAdapter>();
        
        var mockResponse = new GetUserByAzureObjectIdentifierResponse
        {
            User = new UserIdDto
            {
                IdKey = 1,
                Id = "test-id-1234",
                Name = "Test User",
                IsActive = true,
                Email = "test@example.com"
            }
        };

        mockRequestAdapter
            .Setup(x => x.SendAsync<GetUserByAzureObjectIdentifierResponse>(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<GetUserByAzureObjectIdentifierResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResponse);

        var apiClient = new ApiClient(mockRequestAdapter.Object);
        var sut = new KiotaUserRepository(apiClient);

        // Act
        var (user, errorMessage) = await sut.GetUserByAzureObjectIdentifierAsync(azureObjectIdentifier);

        // Assert
        user!.Name.Value.Should().Be("Test User");
    }
}