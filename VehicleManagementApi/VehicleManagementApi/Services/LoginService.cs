using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Threading.Tasks;
using VehicleManagementApi.Interfaces;
using VehicleManagementApi.Models;
using VehicleManagementApi.Options;

namespace VehicleManagementApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IOptions<DbOptions> _options;

        public LoginService(ILogger<LoginService> logger, IOptions<DbOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        public async Task<bool> LoginAsync(Login loginModel)
        {
            _logger.LogInformation("Get Vehicle");
            var dataTable = new DataTable(_options.Value.LoginTable);
            string query = $"Select * from {_options.Value.LoginTable} where Username = {loginModel.Username} and Password={loginModel.Password}";
            SqlConnection conn = new SqlConnection(_options.Value.ConnectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            _logger.LogInformation("create data adapter");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            if (dataTable.Rows.Count == 0)
                return false;

            await Task.CompletedTask;
            return true;
        }

    }
}
