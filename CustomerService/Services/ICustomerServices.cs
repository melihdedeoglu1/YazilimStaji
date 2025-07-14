using CustomerService.Models;
using CustomerService.DTOs;
namespace CustomerService.Services
{
    public interface ICustomerServices
    {
        Task<Customer> GetCustomerByIdAsync(Guid id);

        Task<List<Customer>> GetAllCustomersAsync();

        Task<Customer> AddAsync(CreateCustomerDto createCustomerDto);

        Task<Customer> UpdateAsync(Customer customer);

    }
}
