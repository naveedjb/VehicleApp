using System.Text.RegularExpressions;
using VehicleApi.Dto;
using VehicleApp.Dto;
using VehicleApp.Models;

namespace VehicleApi.Services
{
    public class VehicleService : IVehicleService
    {
        private static readonly List<Vehicle> _vehicles = new()
        {
            new Vehicle { Id = 1, Make = "Toyota", Model = "Camry", Year = 2020, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Vehicle { Id = 2, Make = "Honda", Model = "Accord", Year = 2021, CreatedAt = DateTime.UtcNow.AddDays(-9) },
            new Vehicle { Id = 3, Make = "Ford", Model = "F-150", Year = 2022, CreatedAt = DateTime.UtcNow.AddDays(-8) },
            new Vehicle { Id = 4, Make = "Tesla", Model = "Model 3", Year = 2023, CreatedAt = DateTime.UtcNow.AddDays(-7) },
            new Vehicle { Id = 5, Make = "BMW", Model = "X5", Year = 2022, CreatedAt = DateTime.UtcNow.AddDays(-6) },
            new Vehicle { Id = 6, Make = "Mercedes", Model = "C-Class", Year = 2021, CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new Vehicle { Id = 7, Make = "Audi", Model = "A4", Year = 2023, CreatedAt = DateTime.UtcNow.AddDays(-4) },
            new Vehicle { Id = 8, Make = "Chevrolet", Model = "Silverado", Year = 2022, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new Vehicle { Id = 9, Make = "Nissan", Model = "Altima", Year = 2020, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Vehicle { Id = 10, Make = "Volkswagen", Model = "Jetta", Year = 2021, CreatedAt = DateTime.UtcNow.AddDays(-1) }
        };

        private static int _nextId = 11;
        private static readonly object _lock = new();

        public PagedResult<Vehicle> GetVehicles(int pageNumber, int pageSize)
        {
            lock (_lock)
            {
                var totalCount = _vehicles.Count;
                var items = _vehicles
                    .OrderByDescending(v => v.CreatedAt) 
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PagedResult<Vehicle>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public Vehicle AddVehicle(VehicleDto vehicleDto)
        {
            lock (_lock)
            {
                var vehicle = new Vehicle
                {
                    Id = _nextId++,
                    Make = SanitizeInput(vehicleDto.Make),
                    Model = SanitizeInput(vehicleDto.Model),
                    Year = vehicleDto.Year,
                    CreatedAt = DateTime.UtcNow
                };

                _vehicles.Add(vehicle);
                return vehicle;
            }
        }
               
        private static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var sanitized = input.Trim();

            sanitized = Regex.Replace(sanitized, @"<[^>]*>", string.Empty);

            sanitized = Regex.Replace(sanitized, @"<script.*?</script>", string.Empty, RegexOptions.IgnoreCase);

            sanitized = Regex.Replace(sanitized, @"\s+", " ");

            return sanitized;
        }


    }
}
