using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

/// <summary>
/// Represents a Board interactive component within the Theme Park system.
/// A Board is a specific type of InteractiveComponent.
/// </summary>
public class Board : InteractiveComponent
{
    /// <summary>
    /// Gets the color used for markers on this board.
    /// </summary>
    public Color MarkerColor { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="Board"/> with specified properties.
    /// </summary>
    /// <param name="color">The visual color of the board.</param>
    /// <param name="markerColor">The visual color of the marker color being used.</param>
    /// <param name="texture">The texture of the board.</param>
    /// <param name="plateId">The unique plate identifier for the board.</param>
    /// <param name="coordinates">The coordinates representing the board's position.</param>
    /// <param name="dimensions">The dimensions of the board.</param>
    /// <param name="rotations">The rotations of the board.</param>
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
    }

    /// <summary>
    /// Parameterless constructor for ORM frameworks (e.g., Entity Framework).
    /// Sets the component type to Board.
    /// Should not be used directly in domain logic.
    /// </summary>
    protected Board()
    {
    }

    /// <summary>
    /// Updates the current <see cref="Board"/> instance with values from another <see cref="InteractiveComponent"/>.
    /// </summary>
    /// <param name="other">The <see cref="InteractiveComponent"/> instance whose values will be copied to this instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method first calls the base <see cref="InteractiveComponent.Update(InteractiveComponent)"/> method
    /// to update common properties such as <see cref="Color"/>, <see cref="Texture"/>, <see cref="Coordinates"/>,
    /// and <see cref="Dimensions"/>.  
    /// 
    /// This ensures that all board-specific properties are updated when a <see cref="Board"/> instance is provided.
    /// </remarks>
    public override void Update(InteractiveComponent other)
    {
        base.Update(other);

        if (other is Board board)
        {
            MarkerColor = board.MarkerColor;
        }
    }

}
