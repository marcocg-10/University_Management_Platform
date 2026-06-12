using System.ComponentModel.DataAnnotations;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Models;

/// <summary>
/// Represents the input data model used by the <c>ProjectorCreateModal</c> form.
/// This model is bound to the Blazor form and provides values
/// to create or configure a <see cref="Projector"/> entity.
/// </summary>
/// <remarks>
/// Each property corresponds to a user-input field in the "Create Projector" modal.
/// </remarks>
public class ProjectorForm
{
    /// <summary>
    /// Gets or sets the unique identifier for the projector plate.
    /// </summary>
    [Required(ErrorMessage = "The Plate ID field is required.")]
    public string PlateId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the room (learning space) where the projector will be located.
    /// </summary>
    [Required]
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the projector’s main color in hexadecimal format.
    /// </summary> 
    [Required]
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the projector’s surface texture description.
    /// </summary>
    [Required]
    public string Texture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the brightness level of the projector.
    /// </summary>
    [Required]
    public int? Brightness { get; set; }

    /// <summary>
    /// Gets or sets the resolution (pixels) width of the projection.
    /// </summary>
    [Required(ErrorMessage = "The Resolution Width field is required.")]
    public int? ResWidth { get; set; }

    /// <summary>
    /// Gets or sets the resolution (pixels) height of the projection.
    /// </summary>
    [Required(ErrorMessage = "The Resolution Height field is required.")]
    public int? ResHeight { get; set; }

    /// <summary>
    /// Gets or sets the width of the projector in meters.
    /// </summary>
    [Required]
    public double? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the projector in meters.
    /// </summary>
    [Required]
    public double? Height { get; set; }

    /// <summary>
    /// Gets or sets the depth (thickness) of the projector in meters.
    /// </summary>
    [Required]
    public double? Depth { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the projector’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The X Coordinate field is required.")]
    public double? X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the projector’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The Y Coordinate field is required.")]
    public double? Y { get; set; }

    /// <summary>
    /// Gets or sets the Z coordinate of the projector’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The Z Coordinate field is required.")]
    public double? Z { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the projector around the X axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The X Axis Rotation field is required.")]
    public double? XAxisRotation { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the projector around the Y axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The Y Axis Rotation field is required.")]
    public double? YAxisRotation { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the projector around the Z axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The Z Axis Rotation field is required.")]
    public double? ZAxisRotation { get; set; }
}
