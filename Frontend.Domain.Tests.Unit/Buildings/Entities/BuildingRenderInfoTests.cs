using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Tests.Unit.Buildings.Entities;

/// <summary>
/// Contains unit tests for the <see cref="BuildingRenderInfo"/> class,
/// verifying correct property assignment and constructor behavior.
/// </summary>
public class BuildingRenderInfoTests
{
    private readonly string _inputColor;
    private readonly decimal _inputHeight;
    private readonly decimal _inputWidth;
    private readonly decimal _inputDepth;
    private readonly decimal _inputX;
    private readonly decimal _inputY;
    private readonly decimal _inputZ;
    private readonly string _inputTexture;

    /// <summary>
    /// Initializes test input data for <see cref="BuildingRenderInfo"/> unit tests.
    /// </summary>
    public BuildingRenderInfoTests()
    {
        _inputColor = "#FFFFFF";
        _inputHeight = 12.34m;
        _inputWidth = 45.67m;
        _inputDepth = 89.01m;
        _inputX = 12.34m;
        _inputY = 34.56m;
        _inputZ = 78.90m;
        _inputTexture = "brick_texture.png";
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsColorProperty()
    {
        // Act
        var renderInfo = new BuildingRenderInfo(
            _inputColor, 
            _inputHeight, 
            _inputWidth, 
            _inputDepth, 
            _inputX, 
            _inputY, 
            _inputZ, 
            _inputTexture);

        // Assert
        renderInfo.Color.Should().Be(_inputColor);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsTextureProperty()
    {
        // Act
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        // Assert
        renderInfo.Texture.Should().Be(_inputTexture);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsHeightProperty()
    {
        var renderInfo = new BuildingRenderInfo(
           _inputColor, _inputHeight, _inputWidth, _inputDepth, _inputX, _inputY, _inputZ, _inputTexture);

        renderInfo.Height.Should().Be(_inputHeight);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsWidthProperty()
    {
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        renderInfo.Width.Should().Be(_inputWidth);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsDepthProperty()
    {
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        renderInfo.Depth.Should().Be(_inputDepth);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsXProperty()
    {
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        renderInfo.X.Should().Be(_inputX);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsYProperty()
    {
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        renderInfo.Y.Should().Be(_inputY);
    }

    [Fact]
    public void Ctor_GivenValidArguments_SetsZProperty()
    {
        var renderInfo = new BuildingRenderInfo(
            _inputColor,
            _inputHeight,
            _inputWidth,
            _inputDepth,
            _inputX,
            _inputY,
            _inputZ,
            _inputTexture);

        renderInfo.Z.Should().Be(_inputZ);
    }
}
