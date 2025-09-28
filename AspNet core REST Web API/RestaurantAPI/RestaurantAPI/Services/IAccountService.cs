using System.Threading;
using System.Threading.Tasks;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
    }
}