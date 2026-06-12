using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Buildings.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Core;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Buildings.Repositories;

public class BuildingRepositoryIntegrationTests
{
    private static Building CreateBuilding(
        string officialId = "B001",
        string name = "ECCI",
        int floors = 3,
        string color = "#FFFFFF",
        decimal height = 100,
        decimal width = 50,
        decimal depth = 30,
        decimal x = 10,
        decimal y = 20,
        decimal z = 5)
    {
        return new Building(
            BuildingOfficialId.Create(officialId),
            BuildingName.Create(name),
            FloorCount.Create(floors),
            new BuildingRenderInfo(
                Color.Create(color),
                Heigth.Create(height),
                Width.Create(width),
                Depth.Create(depth),
                X.Create(x),
                Y.Create(y),
                Z.Create(z),
                BuildingTexture.Create("Default_texture.png")
            )
        );
    }

    [Fact]
    public async Task GetBuildingsAsync_WhenEmpty_ReturnsEmpty()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);

        // Act
        var result = await repo.GetBuildingsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddBuildingAsync_PersistsBuildingAndRenderInfo()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);
        var building = CreateBuilding("B100", "Anexo");

        // Act
        var saved = await repo.AddBuildingAsync(building);

        // Assert
        saved.Id.Should().BeGreaterThan(0);
        saved.RenderInfo.Should().NotBeNull();

        var fetched = await ctx.Building
            .Include(b => b.RenderInfo)
            .SingleAsync(b => b.Id == saved.Id);

        // VO conversions persisted correctly
        fetched.OfficialId.Value.Should().Be("B100");
        fetched.Name.Value.Should().Be("Anexo");
        fetched.FloorCount.Value.Should().Be(3);

        // 1:1 FK is set
        fetched.RenderInfo.BuildingId.Should().Be(fetched.Id);
    }

    [Fact]
    public async Task GetBuildingsAsync_IncludesRenderInfo()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);
        await repo.AddBuildingAsync(CreateBuilding("B200", "Tinoco Library"));

        // Act
        var list = await repo.GetBuildingsAsync();

        // Assert
        var single = list.Should().ContainSingle().Subject;
        single.RenderInfo.Should().NotBeNull();
        single.RenderInfo.Color.Value.Should().Be("#FFFFFF");
    }

    [Fact]
    public async Task UpdateBuildingAsync_UpdatesAggregateAndRenderInfo()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);

        await repo.AddBuildingAsync(CreateBuilding(
            officialId: "B300",
            name: "ECCI",
            floors: 2,
            color: "#111111",
            height: 80,
            width: 40,
            depth: 20,
            x: 5,
            y: 10,
            z: 2));

        var updated = CreateBuilding(
            officialId: "B300",
            name: "ECCI Anexo",
            floors: 4,
            color: "#222222",
            height: 90,
            width: 45,
            depth: 22,
            x: 6,
            y: 12,
            z: 3);

        // Act
        await repo.UpdateBuildingAsync(updated);

        // Assert
        var fetched = await ctx.Building.Include(b => b.RenderInfo).SingleAsync();
        fetched.Name.Value.Should().Be("ECCI Anexo");
        fetched.FloorCount.Value.Should().Be(4);
        fetched.RenderInfo.Color.Value.Should().Be("#222222");
        fetched.RenderInfo.Heigth.Value.Should().Be(90);
        fetched.RenderInfo.Width.Value.Should().Be(45);
        fetched.RenderInfo.Depth.Value.Should().Be(22);
        fetched.RenderInfo.XCoodinate.XValue.Should().Be(6);
        fetched.RenderInfo.YCoodinate.YValue.Should().Be(12);
        fetched.RenderInfo.ZCoodinate.ZValue.Should().Be(3);
    }

    [Fact]
    public async Task UpdateBuildingAsync_WhenNotFound_ThrowsBuildingDataException()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);
        var missingId = "B999";
        var nonExisting = CreateBuilding(missingId, "NASA");

        // Act
        var act = () => repo.UpdateBuildingAsync(nonExisting);

        // Assert
        await act.Should().ThrowAsync<BuildingDataException>()
            .WithMessage("An unexpected error occurred while updating the building.");
    }

    [Fact]
    public async Task DeleteBuildingAsync_RemovesBuildingAndRenderInfo()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);
        await repo.AddBuildingAsync(CreateBuilding("B400", "ToDelete"));

        // Act
        await repo.DeleteBuildingAsync("B400");

        // Assert
        (await ctx.Building.CountAsync()).Should().Be(0);
        (await ctx.BuildingRenderInfo.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteBuildingAsync_WhenNotFound_ThrowsBuildingDataException()
    {
        // Arrange
        var (ctx, conn) = SqliteDbContextFactory.Create();
        await using var _ = conn;
        await using var __ = ctx;
        var repo = new BuildingRepository(ctx);
        var missingId = "B404";

        // Act
        var act = () => repo.DeleteBuildingAsync(missingId);

        // Assert
        await act.Should().ThrowAsync<BuildingDataException>()
            .WithMessage("An unexpected error occurred while deleting the building.");
    }
}