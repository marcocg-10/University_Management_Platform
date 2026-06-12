using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.LearningSpaces.Entities;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

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
    public string Texture { get; private set; }

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

    /// <summary>
    /// Parameterless constructor for ORM frameworks (e.g., Entity Framework).
    /// Should not be used directly.
    /// </summary>
    protected InteractiveComponent() { }

    /// <summary>
    /// Updates the <see cref="Coordinates"/> of this interactive component.
    /// </summary>
    /// <param name="newCoordinates">
    /// The new <see cref="Coordinates"/> instance representing the updated spatial position 
    /// of the component within its learning space.
    /// </param>
    /// <remarks>
    /// This method performs a controlled domain update of the component's position. 
    /// Domain validation rules (such as coordinate bounds or logical constraints) 
    /// may be added here if required by the business logic.
    /// </remarks>
    public void UpdateCoordinates(Coordinates newCoordinates)
    {
        Coordinates = newCoordinates;
    }

    /// <summary>
    /// Updates the <see cref="Dimensions"/> of this interactive component.
    /// </summary>
    /// <param name="newDimensions">
    /// The new <see cref="Dimensions"/> instance representing the updated width, height, 
    /// and depth of the component.
    /// </param>
    /// <remarks>
    /// This method allows controlled modification of the component's physical size 
    /// while preserving encapsulation. Domain-level validation (e.g., ensuring positive 
    /// or realistic dimension values) can be introduced here if required.
    /// </remarks>
    public void UpdateDimensions(Dimensions newDimensions)
    {
        Dimensions = newDimensions;
    }

    /// <summary>
    /// Updates the current rotations with the specified new values.
    /// </summary>
    /// <param name="newRotations">The new set of rotations to apply. Cannot be null.</param>
    public void UpdateRotations(Rotations newRotations)
    {
        Rotations = newRotations;
    }

    /// <summary>
    /// Updates the current <see cref="InteractiveComponent"/> with the values from another instance.
    /// </summary>
    /// <param name="other">The <see cref="InteractiveComponent"/> instance whose values will be copied to this instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
    /// <remarks>
    /// This method copies the following properties from the provided <paramref name="other"/> component:
    /// <list type="bullet">
    /// <item><description><see cref="Color"/> – the color of the component.</description></item>
    /// <item><description><see cref="Texture"/> – the texture of the component.</description></item>
    /// <item><description><see cref="Coordinates"/> – updates the coordinates via <see cref="UpdateCoordinates(Coordinates)"/>.</description></item>
    /// <item><description><see cref="Dimensions"/> – updates the dimensions via <see cref="UpdateDimensions(Dimensions)"/>.</description></item>
    /// <item><description><see cref="Rotations"/> – updates the rotations via <see cref="UpdateRotations(Rotations)"/>.</description></item>
    /// </list>
    /// This method does not modify the <see cref="PlateId"/> or other identifying properties.
    /// </remarks>
    public virtual void Update(InteractiveComponent other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));

        Color = other.Color;
        Texture = other.Texture;
        LearningSpaceId = other.LearningSpaceId;
        UpdateCoordinates(other.Coordinates);
        UpdateDimensions(other.Dimensions);
        UpdateRotations(other.Rotations);
    }
}
