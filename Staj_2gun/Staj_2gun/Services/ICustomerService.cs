using Staj_2gun.Models;

namespace Staj_2gun.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer?> UpdateAsync(int id, Customer updatedCustomer);
        Task<bool> DeleteAsync(int id);
    }

}
