using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Building"/> class, verifying correct property assignment
/// and constructor behavior for building entities.
/// </summary>
public class BuildingTests
{
    private readonly BuildingOfficialId _inputId;
    private readonly BuildingName _inputName;
    private readonly FloorCount _inputFloorCount;
    private readonly int _inputBuildingId;
    private readonly Color _inputColor;
    private readonly BuildingTexture _inputTexture;
    private readonly Heigth _inputHeigth;
    private readonly Width _inputWidth;
    private readonly Depth _inputDepth;
    private readonly X _inputXCoordinate;
    private readonly Y _inputYCoordinate;
    private readonly Z _inputZCoordinate;
    private readonly BuildingRenderInfo _inputRenderInfo;

    /// <summary>
    /// Initializes test input data for <see cref="Building"/> unit tests.
    /// </summary>
    public BuildingTests()
    {
        _inputId = BuildingOfficialId.Create("EDCI2023");
        _inputName = BuildingName.Create("ECCI");
        _inputFloorCount = FloorCount.Create(3);
        _inputBuildingId = 1;
        _inputColor = Color.Create("#FFFFFF");
        _inputTexture = BuildingTexture.Create("brick_texture.png");
        _inputHeigth = Heigth.Create(12.34m);
        _inputWidth = Width.Create(45.67m);
        _inputDepth = Depth.Create(89.01m);
        _inputXCoordinate = X.Create(12.34m);
        _inputYCoordinate = Y.Create(34.56m);
        _inputZCoordinate = Z.Create(78.90m);
        _inputRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectlySetsIdProperty()
    {
        var building = new Building(
            _inputId,
            _inputName,
            _inputFloorCount,
            _inputRenderInfo);

        building.OfficialId.Should().Be(_inputId,
            because: "The constructor should set the Id property to the value passed as parameter");
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectlySetsNameProperty()
    {
        var building = new Building(
            _inputId,
            _inputName,
            _inputFloorCount,
            _inputRenderInfo);

        building.Name.Should().Be(_inputName,
            because: "The constructor should set the Name property to the value passed as parameter");
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectlySetsFloorCountProperty()
    {
        var building = new Building(
            _inputId,
            _inputName,
            _inputFloorCount,
            _inputRenderInfo);

        building.FloorCount.Should().Be(_inputFloorCount,
            because: "The constructor should set the FloorCount property to the value passed as parameter");
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectlySetsTextureProperty()
    {
        var building = new Building(
            _inputId,
            _inputName,
            _inputFloorCount,
            _inputRenderInfo);

        building.RenderInfo.Texture.Should().Be(_inputTexture,
            because: "The constructor should set the Texture property to the value passed as parameter");
    }
}
