using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;

/// <summary>
/// Represents a Board interactive component within the Theme Park system.
/// A Board is a specific type of InteractiveComponent with its type set accordingly.
/// </summary>
public class Board : InteractiveComponent
{
    /// <summary>
    /// Gets the color used for markers on this board.
    /// </summary>
    public Color MarkerColor { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Board"/> with specified properties.
    /// </summary>
    /// <param name="color">The visual color of the board.</param>
    /// <param name="markerColor">The visual color of the marker color being used.</param>
    /// <param name="texture">The texture of the board.</param>
    /// <param name="plateId">The unique plate identifier for the board.</param>
    /// <param name="coordinates">The coordinates representing the board's position.</param>
    /// <param name="dimensions">The dimensions of the board.</param>
    /// <param name="rotations">The current rotation of the board.</param>
    /// <param name="learningSpaceId">The ID of the associated learning space.</param>
    public Board(
        Color color,
        Color markerColor,
        string texture,
        PlateId plateId,
        Coordinates coordinates,
        Dimensions dimensions,
        Rotations rotations,
        int learningSpaceId
    ) : base(
        color,
        texture,
        plateId,
        coordinates,
        dimensions,
        rotations,
        learningSpaceId
    )
    {
        MarkerColor = markerColor;
        InteractiveComponentType = InteractiveComponentType.Board;
    }
}
