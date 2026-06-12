using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities
{

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
            InteractiveComponentsTexture texture,
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
    }
}