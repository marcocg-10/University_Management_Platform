using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Integration.Buildings.Entities;

public class BuildingAggregateIntegrationTests
{
    private static Building BuildWithRenderInfo(
        string id,
        string name,
        int floors,
        string color,
        string texture,
        decimal h,
        decimal w,
        decimal d,
        decimal x,
        decimal y,
        decimal z) =>
        new Building(
            BuildingOfficialId.Create(id),
            BuildingName.Create(name),
            FloorCount.Create(floors),
            new BuildingRenderInfo(
                Color.Create(color),
                Heigth.Create(h),
                Width.Create(w),
                Depth.Create(d),
                X.Create(x),
                Y.Create(y),
                Z.Create(z),
                BuildingTexture.Create(texture)));

    [Fact]
    public void UpdateBuilding_UpdatesAllMutableStateExceptOfficialId()
    {
        var original = BuildWithRenderInfo("B500", "Original", 2, "#111111", "old_texture.png", 80, 40, 20, 5, 10, 2);
        var updated = BuildWithRenderInfo("B500", "Updated", 3, "#222222", "new_texture.png", 90, 45, 22, 6, 12, 3);

        original.UpdateBuilding(updated);

        original.OfficialId.Value.Should().Be("B500", because: "OfficialId must remain immutable");
        original.Name.Value.Should().Be("Updated");
        original.FloorCount.Value.Should().Be(3);
        original.RenderInfo.Color.Value.Should().Be("#222222");
        original.RenderInfo.Texture.Value.Should().Be("new_texture.png");
        original.RenderInfo.Heigth.Value.Should().Be(90);
        original.RenderInfo.Width.Value.Should().Be(45);
        original.RenderInfo.Depth.Value.Should().Be(22);
        original.RenderInfo.XCoodinate.XValue.Should().Be(6);
        original.RenderInfo.YCoodinate.YValue.Should().Be(12);
        original.RenderInfo.ZCoodinate.ZValue.Should().Be(3);
    }

    [Fact]
    public void UpdateBuilding_WhenUpdatedHasNullRenderInfo_KeepsOriginalRenderInfo()
    {
        var original = BuildWithRenderInfo("B501", "Original", 2, "#AAAAAA", "original_texture.png", 50, 25, 15, 1, 2, 3);
        var updatedWithoutRenderInfo = new Building(
            BuildingOfficialId.Create("B501"),
            BuildingName.Create("Renamed"),
            FloorCount.Create(4),
            buildingRenderInfo: null!);

        original.UpdateBuilding(updatedWithoutRenderInfo);

        original.Name.Value.Should().Be("Renamed");
        original.FloorCount.Value.Should().Be(4);
        original.RenderInfo.Should().NotBeNull();
        original.RenderInfo.Color.Value.Should().Be("#AAAAAA");
        original.RenderInfo.Texture.Value.Should().Be("original_texture.png");
    }

    [Fact]
    public void UpdateBuilding_WhenOriginalHasNoRenderInfo_DoesNotThrow()
    {
        var original = new Building(
            BuildingOfficialId.Create("B600"),
            BuildingName.Create("NoRender"),
            FloorCount.Create(1),
            buildingRenderInfo: null!);

        var updated = BuildWithRenderInfo("B600", "WithRenderNow", 2, "#00FF00", "texture_added.png", 60, 30, 18, 10, 12, 2);

        var act = () => original.UpdateBuilding(updated);

        act.Should().NotThrow();
        original.Name.Value.Should().Be("WithRenderNow");
        original.FloorCount.Value.Should().Be(2);
        original.RenderInfo.Should().BeNull(because: "UpdateBuilding only updates RenderInfo if both sides have one");
    }
}
