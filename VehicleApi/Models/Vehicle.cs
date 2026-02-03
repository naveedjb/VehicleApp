using System.ComponentModel.DataAnnotations;

namespace VehicleApp.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
