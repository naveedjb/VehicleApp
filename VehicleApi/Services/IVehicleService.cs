using VehicleApi.Dto;
using VehicleApp.Dto;
using VehicleApp.Models;

namespace VehicleApi.Services
{
    public interface IVehicleService
    {
        PagedResult<Vehicle> GetVehicles(int pageNumber, int pageSize);
        Vehicle AddVehicle(VehicleDto vehicleDto);
    }
}
