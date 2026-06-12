using System.ComponentModel.DataAnnotations;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.InteractiveComponents.Models;

/// <summary>
/// Represents the input data model used by the <c>BoardCreateModal</c> form.
/// This model is bound to the Blazor form and provides values
/// to create or configure a <see cref="Board"/> entity.
/// </summary>
/// <remarks>
/// Each property corresponds to a user-input field in the "Create Board" modal.
/// </remarks>
public class BoardForm
{
    /// <summary>
    /// Gets or sets the unique identifier for the board plate.
    /// </summary>
    [Required(ErrorMessage = "The Plate ID field is required.")]
    public string PlateId { get; set; } =string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the room (learning space) where the board will be located.
    /// </summary>
    [Required]
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the board’s main color in hexadecimal format.
    /// </summary>
    [Required]
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the board’s surface texture description.
    /// </summary>
    [Required]
    public string Texture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the marker ink intended for use with the board.
    /// </summary>
    [Required(ErrorMessage = "The Marker Color field is required.")]
    public string MarkerColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the board in meters.
    /// </summary>
    [Required]
    public double? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the board in meters.
    /// </summary>
    [Required]
    public double? Height { get; set; }

    /// <summary>
    /// Gets or sets the depth (thickness) of the board in meters.
    /// </summary>
    [Required]
    public double? Depth { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the board’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The X Coordinate field is required.")]
    public double? X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the board’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The Y Coordinate field is required.")]
    public double? Y { get; set; }

    /// <summary>
    /// Gets or sets the Z coordinate of the board’s position in the learning space.
    /// </summary>
    [Required(ErrorMessage = "The Z Coordinate field is required.")]
    public double? Z { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the board around the X axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The X Axis Rotation field is required.")]
    public double? XAxisRotation { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the board around the Y axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The Y Axis Rotation field is required.")]
    public double? YAxisRotation { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the board around the Z axis in degrees.
    /// </summary>
    [Required(ErrorMessage = "The Z Axis Rotation field is required.")]
    public double? ZAxisRotation { get; set; }   
}
