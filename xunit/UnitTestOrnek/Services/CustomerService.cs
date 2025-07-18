using UnitTestOrnek.Repositories;
using AutoMapper;
using UnitTestOrnek.Models;
using UnitTestOrnek.DTOs;

namespace UnitTestOrnek.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(CreateCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            return await _customerRepository.CreateAsync(customer);

        }

        public async Task<bool> UpdateCustomerAsync(int id, CreateCustomerDto dto)
        {
            var updatedCustomer = _mapper.Map<Customer>(dto);
            updatedCustomer.Id = id;

            return await _customerRepository.UpdateAsync(id, updatedCustomer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        { 
            return await _customerRepository.DeleteAsync(id);
        }

    }
}
