using System.Threading.Tasks;
using VehicleManagementApi.Models;

namespace VehicleManagementApi.Interfaces
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(Login loginModel);
    }
}
