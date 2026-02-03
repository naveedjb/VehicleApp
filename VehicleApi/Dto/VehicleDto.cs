using System.ComponentModel.DataAnnotations;

namespace VehicleApp.Dto
{
    public class VehicleDto
    {
        [Required(ErrorMessage = "Make is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Make must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Make can only contain letters, numbers, spaces, and hyphens")]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Model must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Model can only contain letters, numbers, spaces, and hyphens")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required")]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100")]
        public int Year { get; set; }
    }
}
