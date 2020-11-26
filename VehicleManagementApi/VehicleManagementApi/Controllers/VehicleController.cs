using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagementApi.Interfaces;
using VehicleManagementApi.Models;

namespace VehicleManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize()]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(ILogger<VehicleController> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<List<Vehicle>> GetVehicles()
        {
            try
            {
                _logger.LogInformation("GetVehicles");
                var data = await _vehicleService.GetAll();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error fetching Vehicle details {ex.Message}");
                return null;
            }
        }

        [HttpGet]
        public async Task<Vehicle> GetVehicle(int id)
        {
            try
            {
                _logger.LogInformation("GetVehicle");
                var data = await _vehicleService.GetSingle(id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error fetching Vehicle detail {ex.Message} for Id {id}");
                return null;
            }
        }

        [HttpPost]
        public async Task<bool> UpsertVehicle([FromBody] Vehicle vehicleModel)
        {
            try
            {
                _logger.LogInformation("UpsertVehicle");
                var data = await _vehicleService.UpsertVehicle(vehicleModel);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error fetching Vehicle details {ex.Message}");
                return false;
            }
        }

        [HttpGet]
        public async Task<bool> RemoveVehicle(int id)
        {
            try
            {
                _logger.LogInformation("RemoveVehicle details");
                var data = await _vehicleService.RemoveVehicle(id);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error removing Vehicle detail {ex.Message} for Id {id}");
                return false;
            }
        }

    }

}
