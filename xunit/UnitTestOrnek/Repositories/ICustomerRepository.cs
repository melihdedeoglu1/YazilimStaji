using UnitTestOrnek.Models;

namespace UnitTestOrnek.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task<bool> UpdateAsync(int id, Customer updatedCustomer);
        Task<bool> DeleteAsync(int id);
    }
}
