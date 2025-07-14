using CustomerService.Data;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using CustomerService.DTOs;

namespace CustomerService.Services
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hashBytes = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public class CustomerServices : ICustomerServices
    {
        private readonly CustomerContext _context;

        public CustomerServices(CustomerContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> AddAsync(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                throw new ArgumentNullException(nameof(createCustomerDto), "CreateCustomerDto cannot be null");
            }
            var customer = new Customer
            {
                Id = createCustomerDto.Id,
                UserName = createCustomerDto.UserName,
                Email = createCustomerDto.Email,
                Password = PasswordHelper.HashPassword(createCustomerDto.Password), 
                CreatedAt = DateTime.UtcNow,            
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }


        public async Task<Customer> UpdateAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "Customer cannot be null");
            }
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
