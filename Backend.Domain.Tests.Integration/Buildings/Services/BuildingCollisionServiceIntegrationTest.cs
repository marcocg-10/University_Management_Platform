using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.CollisionDetector.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Integration.Buildings.Services;

public class BuildingCollisionServiceIntegrationTests
{
    // Create a instance of building entity
    private static Building Building(string id, string name, int floors, decimal x, decimal y) =>
        new Building(
            BuildingOfficialId.Create(id),
            BuildingName.Create(name),
            FloorCount.Create(floors),
            new BuildingRenderInfo(
                Color.Create("#FFFFFF"),
                Heigth.Create(100),
                Width.Create(50),
                Depth.Create(30),
                X.Create(x),
                Y.Create(y),
                Z.Create(5),
                BuildingTexture.Create("Default_texture.png")
            )
        );

    // Prepare dependencies and system under test
    private static (IBuildingCollisionService sut, Mock<IBuildingRepository> repoMock) CreateSut()
    {
        var repo = new Mock<IBuildingRepository>(MockBehavior.Strict);
        var detector = new BuildingCollisionDetector(); // real
        IBuildingCollisionService sut = new BuildingCollisionService(repo.Object, detector);
        return (sut, repo);
    }

    // In case of overlapping buildings, the service should return true
    [Fact]
    public async Task Overlapping_ReturnsTrue()
    {
        // Arrange
        var existing = Building("B001", "Existing", 3, x: 10, y: 20);
        var toCheck = Building("B002", "ToCheck", 3, x: 15, y: 25);

        var (sut, repo) = CreateSut();
        repo.Setup(r => r.GetBuildingsAsync()).ReturnsAsync(new[] { existing });

        // Act
        var result = await sut.HasCollisionAsync(toCheck);

        // Assert
        result.Should().BeTrue(because: "buildings overlap in position and dimensions");
        repo.Verify(r => r.GetBuildingsAsync(), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    //  In case of non-overlapping buildings, the service should return false
    [Fact]
    public async Task NonOverlapping_ReturnsFalse()
    {
        // Arrange
        var existing = Building("B001", "Existing", 3, x: 100, y: 100);
        var toCheck = Building("B002", "ToCheck", 3, x: 15, y: 25);

        var (sut, repo) = CreateSut();
        repo.Setup(r => r.GetBuildingsAsync()).ReturnsAsync(new[] { existing });

        // Act
        var result = await sut.HasCollisionAsync(toCheck);

        // Assert
        result.Should().BeFalse(because: "buildings are far apart and do not collide");
        repo.Verify(r => r.GetBuildingsAsync(), Times.Once);
        repo.VerifyNoOtherCalls();
    }

    // If any existing building overlaps, the whole service should return true
    [Fact]
    public async Task AnyExistingOverlaps_ReturnsTrue()
    {
        // Arrange: one far, one near enough to overlap with (15,25)
        var far = Building("B010", "Far", 3, x: 300, y: 300);
        var near = Building("B011", "Near", 3, x: 12, y: 22);
        var toCheck = Building("B020", "ToCheck", 3, x: 15, y: 25);

        var (sut, repo) = CreateSut();
        repo.Setup(r => r.GetBuildingsAsync()).ReturnsAsync(new[] { far, near });

        // Act
        var result = await sut.HasCollisionAsync(toCheck);

        // Assert
        result.Should().BeTrue(because: "collision with any existing building should return true");
        repo.Verify(r => r.GetBuildingsAsync(), Times.Once);
        repo.VerifyNoOtherCalls();
    }
}