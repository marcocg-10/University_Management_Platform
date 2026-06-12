using FluentAssertions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Infrastructure.Tests.Unit.Buildings.Repositories;

/// <summary>
/// Unit tests for <see cref="KiotaBuildingRepository"/>.
/// </summary>
public class KiotaBuildingRepositoryTests
{
    private readonly Mock<IRequestAdapter> _mockRequestAdapter;
    private readonly ApiClient _apiClient;
    private readonly KiotaBuildingRepository _sut;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaBuildingRepositoryTests"/> class.
    /// </summary>
    public KiotaBuildingRepositoryTests()
    {
        _mockRequestAdapter = new Mock<IRequestAdapter>();
        _apiClient = new ApiClient(_mockRequestAdapter.Object);
        _sut = new KiotaBuildingRepository(_apiClient);
    }

    /// <summary>
    /// Verifies that <see cref="KiotaBuildingRepository.GetBuildingsAsync"/> returns a list of buildings when the API responds with data.
    /// </summary>
    [Fact]
    public async Task BuildingsAsync_ReturnsBuildings()
    {
        // Arrange
        var mockResponse = new GetBuildingsResponse
        {
            Buildings = new List<BuildingDtoWithId>
            {
                new BuildingDtoWithId
                {
                    OfficialId = "A001",
                    Name = "Building A",
                    Id = 1,
                    FloorCount = 5,
                    BuildingRenderInfo = new BuildingRenderInfoDto
                    {
                        Color = "Red",
                        Height = 50.0,
                        Width = 30.0,
                        Depth = 20.0,
                        XCoordinate = 10.0,
                        YCoordinate = 0.0,
                        ZCoordinate = 5.0,
                        Texture = "brick_texture.png"
                    }
                },
                new BuildingDtoWithId
                {
                    OfficialId = "B002",
                    Name = "Building B",
                    Id = 2,
                    FloorCount = 6,
                    BuildingRenderInfo = new BuildingRenderInfoDto
                    {
                        Color = "Blue",
                        Height = 60.0,
                        Width = 40.0,
                        Depth = 25.0,
                        XCoordinate = 15.0,
                        YCoordinate = 0.0,
                        ZCoordinate = 10.0,
                        Texture = "glass_wall.png"
                    }
                }
            }
        };

        _mockRequestAdapter
            .Setup(x => x.SendAsync<GetBuildingsResponse>(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<GetBuildingsResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResponse);

        // Act
        var buildings = await _sut.GetBuildingsAsync();

        // Assert
        buildings.Should().NotBeNull();
        buildings.Should().HaveCount(2);
        buildings.First().Name.Should().Be("Building A");
        buildings.First().RenderInfo.Texture.Should().Be("brick_texture.png", because: "the texture should match the DTO");
        buildings.Last().Name.Should().Be("Building B");
        buildings.Last().RenderInfo.Texture.Should().Be("glass_wall.png", because: "the texture should match the DTO");
    }

    /// <summary>
    /// Verifies that <see cref="KiotaBuildingRepository.GetBuildingsAsync"/> returns an empty collection when the API responds with no buildings.
    /// </summary>
    [Fact]
    public async Task BuildingsAsync_WhenApiReturnsEmptyList_ReturnsEmptyCollection()
    {
        // Arrange
        var mockResponse = new GetBuildingsResponse
        {
            Buildings = new List<BuildingDtoWithId>()
        };

        _mockRequestAdapter
            .Setup(x => x.SendAsync<GetBuildingsResponse>(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<GetBuildingsResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockResponse);

        // Act
        var buildings = await _sut.GetBuildingsAsync();

        // Assert
        buildings.Should().NotBeNull();
        buildings.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that <see cref="KiotaBuildingRepository.GetBuildingsAsync"/> throws an exception when the API call fails.
    /// </summary>
    [Fact]
    public async Task BuildingsAsync_WhenApiThrowsException_ThrowsException()
    {
        // Arrange
        _mockRequestAdapter
            .Setup(x => x.SendAsync<GetBuildingsResponse>(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<GetBuildingsResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("API Error"));

        // Act & Assert
        await FluentActions
            .Awaiting(() => _sut.GetBuildingsAsync())
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage("API Error");
    }
}
