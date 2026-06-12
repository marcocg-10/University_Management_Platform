using UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.InteractiveComponents.Entities;

/// <summary>
/// Represents a Projector interactive component within the Theme Park system.
/// A Projector is a specific type of InteractiveComponent.
/// </summary>
public class Projector : InteractiveComponent
{
    /// <summary>
    /// Gets the resolution (width, height) of the projector.
    /// </summary>
    public Resolution? ProjectionResolution { get; }

    /// <summary>
    /// Gets the brightness level of the projector.
    /// </summary>
    public int Brightness { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Projector"/> with specified properties.
    /// </summary>
    /// <param name="color">The visual color of the Projector.</param>
    /// <param name="texture">The texture of the Projector.</param>
    /// <param name="brightness">The brightness level of the Projector.</param>
    /// <param name="plateId">The unique plate identifier for the Projector.</param>
    /// <param name="coordinates">The coordinates representing the Projector's position.</param>
    /// <param name="resolution">The resolution of the Projector.</param>
    /// <param name="dimensions">The dimensions of the Projector.</param>
    /// <param name="rotations">The rotations of the Projector.</param>
    /// <param name="learningSpaceId">The ID of the associated learning space.</param>
    public Projector(
        Color color,
        string texture,
        int brightness,
        PlateId plateId,
        Resolution resolution,
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
        Brightness = brightness;
        ProjectionResolution = resolution;
    }

    /// <summary>
    /// Parameterless constructor for ORM frameworks (e.g., Entity Framework).
    /// Sets the component type to Projector.
    /// Should not be used directly in domain logic.
    /// </summary>
    protected Projector()
    {
    }
}
