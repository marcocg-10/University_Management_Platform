using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Buildings.Entities;

/// <summary>
/// Contains unit tests for the <see cref="BuildingRenderInfo"/> class, verifying correct property assignment
/// and constructor behavior for building rendering information.
/// </summary>
public class BuildingRenderInfoTests
{
    private readonly int _inputBuildingId;
    private readonly Color _inputColor;
    private readonly BuildingTexture _inputTexture;
    private readonly Heigth _inputHeigth;
    private readonly Width _inputWidth;
    private readonly Depth _inputDepth;
    private readonly X _inputXCoordinate;
    private readonly Y _inputYCoordinate;
    private readonly Z _inputZCoordinate;

    /// <summary>
    /// Initializes test input data for <see cref="BuildingRenderInfo"/> unit tests.
    /// </summary>
    public BuildingRenderInfoTests()
    {
        _inputBuildingId = 1;
        _inputColor = Color.Create("#FFFFFF");
        _inputTexture = BuildingTexture.Create("brick_texture.png");
        _inputHeigth = Heigth.Create(12.34m);
        _inputWidth = Width.Create(45.67m);
        _inputDepth = Depth.Create(89.01m);
        _inputXCoordinate = X.Create(12.34m);
        _inputYCoordinate = Y.Create(34.56m);
        _inputZCoordinate = Z.Create(78.90m);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingIdProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.BuildingId.Should().Be(_inputBuildingId);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingColorProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.Color.Should().Be(_inputColor);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingTextureProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.Texture.Should().Be(_inputTexture);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingHeigthProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.Heigth.Should().Be(_inputHeigth);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingWidthProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.Width.Should().Be(_inputWidth);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingDepthProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.Depth.Should().Be(_inputDepth);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingXCoordinateProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.XCoodinate.Should().Be(_inputXCoordinate);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingYCoordinateProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.YCoodinate.Should().Be(_inputYCoordinate);
    }

    [Fact]
    public void Ctor_GivenValidArguments_CorrectSetsBuildingZCoordinateProperty()
    {
        var buildingRenderInfo = new BuildingRenderInfo(
            _inputBuildingId,
            _inputColor,
            _inputHeigth,
            _inputWidth,
            _inputDepth,
            _inputXCoordinate,
            _inputYCoordinate,
            _inputZCoordinate,
            _inputTexture);

        buildingRenderInfo.ZCoodinate.Should().Be(_inputZCoordinate);
    }
}
