using UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.ValueObjects.BuildingRenderInfo;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Buildings.Entities;

/// <summary>
/// Entity that represents the rendering information of a building,
/// including its dimensions, appearance, and position in the theme park.
/// </summary>
public class BuildingRenderInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingRenderInfo"/> class without a building ID.
    /// </summary>
    /// <param name="color">Color of the building.</param>
    /// <param name="heigth">Height of the building.</param>
    /// <param name="width">Width of the building.</param>
    /// <param name="depth">Depth of the building.</param>
    /// <param name="xCoodinate">X coordinate of the building’s position.</param>
    /// <param name="yCoodinate">Y coordinate of the building’s position.</param>
    /// <param name="zCoodinate">Z coordinate of the building’s position.</param>
    public BuildingRenderInfo(
        Color color,
        Heigth heigth,
        Width width,
        Depth depth,
        X xCoodinate,
        Y yCoodinate,
        Z zCoodinate,
        BuildingTexture texture)
    {
        Color = color;
        Heigth = heigth;
        Width = width;
        Depth = depth;
        XCoodinate = xCoodinate;
        YCoodinate = yCoodinate;
        ZCoodinate = zCoodinate;
        Texture = texture;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingRenderInfo"/> class with a building ID.
    /// </summary>
    /// <param name="buildingId">Unique identifier of the building.</param>
    /// <param name="color">Color of the building.</param>
    /// <param name="heigth">Height of the building.</param>
    /// <param name="width">Width of the building.</param>
    /// <param name="depth">Depth of the building.</param>
    /// <param name="xCoodinate">X coordinate of the building’s position.</param>
    /// <param name="yCoodinate">Y coordinate of the building’s position.</param>
    /// <param name="zCoodinate">Z coordinate of the building’s position.</param>
    public BuildingRenderInfo(
        int buildingId,
        Color color,
        Heigth heigth,
        Width width,
        Depth depth,
        X xCoodinate,
        Y yCoodinate,
        Z zCoodinate,
        BuildingTexture texture)
    {
        BuildingId = buildingId;
        Color = color;
        Heigth = heigth;
        Width = width;
        Depth = depth;
        XCoodinate = xCoodinate;
        YCoodinate = yCoodinate;
        ZCoodinate = zCoodinate;
        Texture = texture;
    }

    /// <summary>
    /// Gets the system unique identifier for the building render info.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the unique identifier of the building associated with this render info.
    /// </summary>
    /// <remarks>Must be unique.</remarks>
    public int BuildingId { get; }

    /// <summary>
    /// Gets the color of the building.
    /// </summary>
    public Color Color { get; private set; }

    /// <summary>
    /// Gets the height of the building.
    /// </summary>
    public Heigth Heigth { get; private set; }

    /// <summary>
    /// Gets the width of the building.
    /// </summary>
    public Width Width { get; private set; }

    /// <summary>
    /// Gets the depth of the building.
    /// </summary>
    public Depth Depth { get; private set; }

    /// <summary>
    /// Gets the X coordinate of the building’s position.
    /// </summary>
    public X XCoodinate { get; private set; }

    /// <summary>
    /// Gets the Y coordinate of the building’s position.
    /// </summary>
    public Y YCoodinate { get; private set; }

    /// <summary>
    /// Gets the Z coordinate of the building’s position.
    /// </summary>
    public Z ZCoodinate { get; private set; }

    /// <summary>
    /// Gets the building entity associated with this render info.
    /// </summary>
    public Building Building { get; private set; }

    /// <summary>
    /// Gets the texture of the building.
    /// </summary>
    public BuildingTexture Texture { get; private set; }

    /// <summary>
    /// Updates the render info's properties with those from another render info instance.
    /// </summary>
    /// <param name="buildingRenderInfo">The <see cref="BuildingRenderInfo"/> instance to copy properties from.</param>
    public void UpdateRenderInfo(BuildingRenderInfo buildingRenderInfo)
    {
        Color = buildingRenderInfo.Color;
        Heigth = buildingRenderInfo.Heigth;
        Width = buildingRenderInfo.Width;
        Depth = buildingRenderInfo.Depth;
        XCoodinate = buildingRenderInfo.XCoodinate;
        YCoodinate = buildingRenderInfo.YCoodinate;
        ZCoodinate = buildingRenderInfo.ZCoodinate;
        Texture = buildingRenderInfo.Texture;
    }
}