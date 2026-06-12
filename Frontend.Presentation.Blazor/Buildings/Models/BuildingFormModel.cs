using System.ComponentModel.DataAnnotations;

namespace UCR.ECCI.PI.ThemePark.Frontend.Presentation.Blazor.Buildings.Models
{
    public class BuildingFormModel
    {
        [Required(ErrorMessage = "Building ID is required")]
        public string OfficialId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; } = "#000000";

        [Required(ErrorMessage = "Floor count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Floor count must be greater than 0")]
        public int? FloorCount { get; set; }

        [Required(ErrorMessage = "X coordinate is required")]
        public decimal? PositionX { get; set; } = 0.0m;

        [Required(ErrorMessage = "Y coordinate is required")]
        public decimal? PositionY { get; set; } = 0.0m;

        [Required(ErrorMessage = "Z coordinate is required")]
        public decimal? PositionZ { get; set; } = 0.0m;

        [Required(ErrorMessage = "Height is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Height must be greater than 0")]
        public decimal? Height { get; set; } = 10m;

        [Required(ErrorMessage = "Width is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Width must be greater than 0")]
        public decimal? Width { get; set; } = 10m;

        [Required(ErrorMessage = "Depth is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Depth must be greater than 0")]
        public decimal? Depth { get; set; } = 10m;

        [Required(ErrorMessage = "Texture is required")]
        public string Texture { get; set; } = string.Empty;

    }
}
