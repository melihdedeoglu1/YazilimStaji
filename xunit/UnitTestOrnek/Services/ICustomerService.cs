using UnitTestOrnek.DTOs;
using UnitTestOrnek.Models;

namespace UnitTestOrnek.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(CreateCustomerDto dto);
        Task<bool> UpdateCustomerAsync(int id, CreateCustomerDto dto);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
