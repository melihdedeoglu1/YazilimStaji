
using Microsoft.EntityFrameworkCore;
using Staj_ETicaretornek.Data;
using Staj_ETicaretornek.DTOs;
using Staj_ETicaretornek.Models;


namespace Staj_ETicaretornek.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly Context _context;

        public CustomerService(Context context) {

            _context = context;
        
        }

        public async Task<Customer> RegisterAsync(RegisterRequest registerRequest)
        {

            if (registerRequest == null)
            {
                throw new ArgumentNullException(nameof(registerRequest), "Register request cannot be null.");
            }
            Customer customer = new Customer
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
              
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        
        public async Task<Customer> LoginAsync(LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                throw new ArgumentNullException(nameof(loginRequest), "Register request cannot be null.");
            }
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserName == loginRequest.UserName && c.Password == loginRequest.Password);

            if (customer == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            return customer;
        }

    }
}
