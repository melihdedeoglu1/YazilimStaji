using Microsoft.EntityFrameworkCore;
using UnitTestOrnek.Models;
using UnitTestOrnek.Data;

namespace UnitTestOrnek.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Context _context;

        public CustomerRepository(Context context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<List<Customer>> GetAllAsync() 
        {
            return await _context.Customers.ToListAsync();
        
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;               
        }

        public async Task<bool> UpdateAsync(int id, Customer updatedCustomer) 
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) return false;

            customer.Name = updatedCustomer.Name;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return true;
               
        }




    }
}
