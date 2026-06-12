using UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Unity.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Unity.Domain.InteractiveComponents.Entities
{

    /// <summary>
    /// Represents a base interactive component within the Theme Park system.
    /// Interactive components have visual properties, placement details, and are associated
    /// with a specific learning space.
    /// </summary>
    public abstract class InteractiveComponent
    {
        /// <summary>
        /// Gets the color of the interactive component.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Gets the texture of the interactive component.
        /// </summary>
        public InteractiveComponentsTexture Texture { get; private set; }

        /// <summary>
        /// Gets the unique plate identifier of the interactive component.
        /// </summary>
        public PlateId PlateId { get; }

        /// <summary>
        /// Gets the coordinates representing the component's position.
        /// </summary>
        public Coordinates Coordinates { get; private set; }

        /// <summary>
        /// Gets the dimensions of the interactive component.
        /// </summary>
        public Dimensions Dimensions { get; private set; }

        /// <summary>
        /// Gets the current rotation of the interactive component.
        /// </summary>
        public Rotations Rotations { get; private set; }

        /// <summary>
        /// Gets the identifier of the associated learning space.
        /// </summary>
        public int LearningSpaceId { get; private set; }

        /// <summary>
        /// Gets the associated learning space entity.
        /// </summary>
        public LearningSpace LearningSpace { get; }

        /// <summary>
        /// Creates a new interactive component with specified properties.
        /// </summary>
        /// <param name="color">The visual color of the component.</param>
        /// <param name="texture">The texture of the component.</param>
        /// <param name="plateId">The unique plate ID.</param>
        /// <param name="coordinates">The placement coordinates.</param>
        /// <param name="dimensions">The dimensions of the component.</param>
        /// <param name="rotations">The rotations of the component.</param>
        /// <param name="learningSpaceId">The associated learning space ID.</param>
        protected InteractiveComponent(
            Color color,
            InteractiveComponentsTexture texture,
            PlateId plateId,
            Coordinates coordinates,
            Dimensions dimensions,
            Rotations rotations,
            int learningSpaceId)
        {
            Color = color;
            Texture = texture;
            PlateId = plateId;
            Coordinates = coordinates;
            Dimensions = dimensions;
            Rotations = rotations;
            LearningSpaceId = learningSpaceId;
        }
    }
}