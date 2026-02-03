using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleApi.Dto;
using VehicleApi.Services;
using VehicleApp.Dto;
using VehicleApp.Models;

namespace VehicleApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Vehicle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PagedResult<Vehicle>> GetVehicles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
           
            if (pageNumber < 1)
            {
                return BadRequest(new { error = "Page number must be at least 1" });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { error = "Page size must be between 1 and 100" });
            }

            _logger.LogInformation("Getting vehicles - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var result = _vehicleService.GetVehicles(pageNumber, pageSize);

           
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VehicleDto> AddVehicle([FromBody] VehicleDto vehicleDto)
        {
          
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid vehicle data: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

          
            if (string.IsNullOrWhiteSpace(vehicleDto.Make) ||
                string.IsNullOrWhiteSpace(vehicleDto.Model))
            {
                return BadRequest(new { error = "Make and Model cannot be empty or whitespace only" });
            }

            try
            {
                var createdVehicle = _vehicleService.AddVehicle(vehicleDto);

                _logger.LogInformation("Vehicle created - ID: {Id}, Make: {Make}, Model: {Model}, Year: {Year}",
                    createdVehicle.Id, createdVehicle.Make, createdVehicle.Model, createdVehicle.Year);

                return CreatedAtAction(nameof(GetVehicles), null, createdVehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vehicle");
                return StatusCode(500, new { error = "An error occurred while adding the vehicle" });
            }
        }
    }
}
