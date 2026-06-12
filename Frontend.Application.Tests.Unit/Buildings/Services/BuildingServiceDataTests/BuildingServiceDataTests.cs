using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Application.Tests.Unit.Buildings.BuildingServiceDataTests;

/// <summary>
/// Provides reusable test data for building service unit tests.
/// </summary>
public class BuildingServiceDataTests
{
    public Building ValidBuilding =>
        new Building("B001", "ECCI", 3, new BuildingRenderInfo("#FFF", 100, 50, 30, 10, 20, 5, "default_texture.png"));

    public IEnumerable<Building> MultipleBuildings => new[]
    {
        ValidBuilding,
        new Building("B002", "Letras", 2, new BuildingRenderInfo("#CCC", 80, 40, 25, 234, 342, 455, "default_texture.png"))
    };
}