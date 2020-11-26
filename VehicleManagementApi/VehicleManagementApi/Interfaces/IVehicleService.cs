using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagementApi.Models;

namespace VehicleManagementApi.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAll();
        Task<Vehicle> GetSingle(int id);
        Task<bool> UpsertVehicle(Vehicle vehicleModel);
        Task<bool> RemoveVehicle(int id);
    }
}
