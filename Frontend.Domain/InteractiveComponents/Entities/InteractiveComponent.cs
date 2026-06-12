using UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Domain.InteractiveComponents.Entities;

/// <summary>
/// Represents a base interactive component within the Theme Park system.
/// Interactive components have visual properties, placement details, and are associated
/// with a specific learning space.
/// </summary>
public class InteractiveComponent
{
    /// <summary>
    /// Gets the color of the interactive component.
    /// </summary>
    public Color Color { get; }

    /// <summary>
    /// Gets the texture of the interactive component.
    /// </summary>
    public string Texture { get; }

    /// <summary>
    /// Gets the unique plate identifier of the interactive component.
    /// </summary>
    public PlateId PlateId { get; }

    /// <summary>
    /// Gets the coordinates representing the component's position.
    /// </summary>
    public Coordinates Coordinates { get; }

    /// <summary>
    /// Gets the dimensions of the interactive component.
    /// </summary>
    public Dimensions Dimensions { get; }

    /// <summary>
    /// Gets the current rotation of the interactive component.
    /// </summary>
    public Rotations Rotations { get; }

    /// <summary>
    /// Gets the type of the interactive component (e.g., Board).
    /// </summary>
    public InteractiveComponentType InteractiveComponentType { get; protected set; }

    /// <summary>
    /// Gets the identifier of the associated learning space.
    /// </summary>
    public int LearningSpaceId { get; }

    /// <summary>
    /// Creates a new interactive component with specified properties.
    /// </summary>
    /// <param name="color">The visual color of the component.</param>
    /// <param name="texture">The texture of the component.</param>
    /// <param name="plateId">The unique plate ID.</param>
    /// <param name="coordinates">The placement coordinates.</param>
    /// <param name="dimensions">The dimensions of the component.</param>
    /// <param name="learningSpaceId">The associated learning space ID.</param>
    public InteractiveComponent(
        Color color,
        string texture,
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

/// <summary>
/// Defines the types of interactive components available in the system.
/// </summary>
public enum InteractiveComponentType
{
    /// <summary>
    /// Represents a board type interactive component.
    /// </summary>
    Board,

    /// <summary>
    /// Represents a projector type interactive component.
    /// </summary>
    Projector
}
