using System.ComponentModel.DataAnnotations;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.LearningSpaces.Models;

public class CreateLaboratoryForm
{
    [Required(ErrorMessage = "A building must be selected.")]
    public int? BuildingId { get; set; }

    [Required(ErrorMessage = "A floor must be selected.")]
    public int? FloorLevel { get; set; }

    [Required(ErrorMessage = "Room ID is required.")]
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Room ID must be between 2 and 25 characters.")]
    public string? RoomId { get; set; }

    [Required(ErrorMessage = "Color is required.")]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Color must be a valid hex code (e.g., #F0F0F0).")]
    public string? Color { get; set; } = "#CDCECF";

    [Required(ErrorMessage = "A texture must be selected.")]
    public string? Texture { get; set; }

    [Required(ErrorMessage = "Width is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Width must be a positive number.")]
    public float? Width { get; set; }

    [Required(ErrorMessage = "Length is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Length must be a positive number.")]
    public float? Length { get; set; }

    [Required(ErrorMessage = "Height is required.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Height must be a positive number.")]
    public float? Height { get; set; }
}