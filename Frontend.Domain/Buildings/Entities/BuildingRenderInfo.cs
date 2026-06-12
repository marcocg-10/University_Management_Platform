namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.Buildings.Entities;

/// <summary>
/// Contains the visual and positional information required of a building in the frontend.
/// </summary>
public class BuildingRenderInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingRenderInfo"/> class.
    /// </summary>
    /// <param name="color">The color used to visually represent the building.</param>
    /// <param name="height">The height of the building.</param>
    /// <param name="width">The width of the building.</param>
    /// <param name="depth">The depth of the building.</param>
    /// <param name="x">The X-coordinate of the building’s position.</param>
    /// <param name="y">The Y-coordinate of the building’s position.</param>
    /// <param name="z">The Z-coordinate of the building’s position.</param>
    public BuildingRenderInfo(
        string color,
        decimal height,
        decimal width,
        decimal depth,
        decimal x,
        decimal y,
        decimal z,
        string texture)
    {
        Color = color;
        Height = height;
        Width = width;
        Depth = depth;
        X = x;
        Y = y;
        Z = z;
        Texture = texture;
    }

    /// <summary>
    /// Gets the color used to visually represent the building.
    /// </summary>
    public string Color { get; }

    /// <summary>
    /// Gets the height of the building in rendering units.
    /// </summary>
    public decimal Height { get; }

    /// <summary>
    /// Gets the width of the building in rendering units.
    /// </summary>
    public decimal Width { get; }

    /// <summary>
    /// Gets the depth (z-axis thickness) of the building in rendering units.
    /// </summary>
    public decimal Depth { get; }

    /// <summary>
    /// Gets the X-coordinate of the building’s position.
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the building’s position.
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the Z-coordinate of the building’s position.
    /// </summary>
    public decimal Z { get; }

    /// <summary>
    /// Gets the texture file name or path used for the building's surface rendering.
    /// </summary>
    public string Texture { get; }
}