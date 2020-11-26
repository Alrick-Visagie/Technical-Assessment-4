using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleManagementApi.Interfaces;
using VehicleManagementApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleManagementApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }


        [HttpPost]
        public async Task<bool> LoginAsync([FromBody] Login loginModel)
        {
            try
            {
                _logger.LogInformation("LoginAsync");
                var data = await _loginService.LoginAsync(loginModel);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error trying to login {ex.Message}");
                return false;
            }
        }
    }
}
