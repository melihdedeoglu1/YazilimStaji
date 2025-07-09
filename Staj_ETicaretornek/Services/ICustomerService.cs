using Staj_ETicaretornek.Models;
using Staj_ETicaretornek.DTOs;

namespace Staj_ETicaretornek.Services
{
    public interface ICustomerService
    {
        Task<Customer> RegisterAsync(RegisterRequest registerRequest);
        Task<Customer> LoginAsync(LoginRequest loginRequest);


    }
}
