using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VehicleManagementApi.Interfaces;
using VehicleManagementApi.Models;
using VehicleManagementApi.Options;

namespace VehicleManagementApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ILogger<VehicleService> _logger;
        private readonly IOptions<DbOptions> _options;

        public VehicleService(ILogger<VehicleService> logger, IOptions<DbOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<List<Vehicle>> GetAll()
        {
            _logger.LogInformation("Get All Vehicles");
            var dataTable = new DataTable(_options.Value.TableName);
            string query = $"Select * from {_options.Value.TableName}";
            SqlConnection conn = new SqlConnection(_options.Value.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            _logger.LogInformation("create data adapter");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            if (dataTable.Rows.Count == 0)
                return new List<Vehicle>();

            var vehicleList = new List<Vehicle>();
            vehicleList = (from DataRow data in dataTable.Rows
                           select new Vehicle
                           {
                               Id = Convert.ToInt32(data["Id"]),
                               Make = data["Make"].ToString(),
                               Model = data["Model"].ToString(),
                               EngineNo = data["EngineNo"].ToString(),
                               VinNo = data["VinNo"].ToString(),
                               RegNo = data["RegNo"].ToString(),
                               IssueDate = Convert.ToDateTime(data["IssueDate"]),
                               Financed = Convert.ToBoolean(data["Financed"])
                           }).ToList();

            await Task.CompletedTask;
            return vehicleList;
        }

        public async Task<Vehicle> GetSingle(int id)
        {
            _logger.LogInformation("Get Vehicle");
            var dataTable = new DataTable(_options.Value.TableName);
            string query = $"Select * from {_options.Value.TableName}";
            SqlConnection conn = new SqlConnection(_options.Value.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            _logger.LogInformation("create data adapter");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            if (dataTable.Rows.Count == 0)
                return new Vehicle();

            var vehicleList = new List<Vehicle>();
            vehicleList = (from DataRow data in dataTable.Rows
                           select new Vehicle
                           {
                               Id = Convert.ToInt32(data["Id"]),
                               Make = data["Make"].ToString(),
                               Model = data["Model"].ToString(),
                               EngineNo = data["EngineNo"].ToString(),
                               VinNo = data["VinNo"].ToString(),
                               RegNo = data["RegNo"].ToString(),
                               IssueDate = Convert.ToDateTime(data["IssueDate"]),
                               Financed = Convert.ToBoolean(data["Financed"])
                           }).ToList();

            await Task.CompletedTask;
            return vehicleList.First();
        }

        public async Task<bool> RemoveVehicle(int id)
        {
            _logger.LogInformation("Remove Vehicle");
            var conn = new SqlConnection(_options.Value.ConnectionString);
            if (id == 0)
                throw new Exception("Id property is Mandatory");

            _logger.LogInformation("Insert new record");
            string query = $"Delete From {_options.Value.TableName} where Id={id}";

            var cmd = new SqlCommand(query, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> UpsertVehicle(Vehicle vehicleModel)
        {
            _logger.LogInformation("Upsert Vehicles");

            _logger.LogInformation("If Id is zero or null Insert new record");
            var conn = new SqlConnection(_options.Value.ConnectionString);
            var cmd = new SqlCommand();
            string query = string.Empty;
            if (vehicleModel.Id == 0 || vehicleModel.Id == null)
            {
                _logger.LogInformation("Insert new record");
                 query = $"Insert Into {_options.Value.TableName} (Make,Model,EngineNo,VinNo,RegNo,IssueDate,Financed) VALUES ({vehicleModel.Make}, {vehicleModel.Model}," +
                        $"{vehicleModel.EngineNo}, {vehicleModel.VinNo}.{vehicleModel.RegNo}, {vehicleModel.IssueDate}, {vehicleModel.Financed})";
    
                 cmd = new SqlCommand(query, conn);
                conn.Open();
                int insertResult = cmd.ExecuteNonQuery();

                // Check Error
                if (insertResult < 0)
                {
                    _logger.LogError("Error inserting data into Database!");
                    throw new Exception("Error inserting data into Database!");
                }
                conn.Close();
                return true;
            }
            _logger.LogInformation("Update existing record");
            //Get All existing records
            var existingData = await GetSingle(vehicleModel.Id);

            if (existingData == null)
                throw new Exception("Id doesnot exist in current data");


            //execute an update statement
             query = $"Update {_options.Value.TableName} SET Make='{vehicleModel.Make}',Model = '{vehicleModel.Model}',EngineNo= '{vehicleModel.EngineNo}',VinNo='{vehicleModel.VinNo}',RegNo= '{vehicleModel.RegNo}',IssueDate = '{vehicleModel.IssueDate}',Financed = {vehicleModel.Financed} Where Id= {vehicleModel.Id}";

            conn = new SqlConnection(_options.Value.ConnectionString);
            cmd = new SqlCommand(query, conn);

            conn.Open();
            int updateResult = cmd.ExecuteNonQuery();

            // Check Error
            if (updateResult < 0)
            {
                _logger.LogError("Error inserting data into Database!");
                throw new Exception("Error inserting data into Database!");
            }

            conn.Close();
            return true;
        }

    }
}
