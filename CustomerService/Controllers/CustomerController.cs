using CustomerService.DTOs;
using CustomerService.Models;
using CustomerService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerService;

        public CustomerController(ICustomerServices customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }




        [HttpPost("from-auth")]    
        public async Task<IActionResult> AddCustomer(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                return BadRequest("CreateCustomerDto cannot be null");
            }
            
            var customer = await _customerService.AddAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }


        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                userId,
                userName,
                email
            });
        }






        [HttpPatch("{id}/decrease-balance")]
        public async Task<IActionResult> DecreaseBalance(Guid id, [FromQuery] int amount)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound("Müşteri bulunamadı.");

            if (customer.Age < amount)
                return BadRequest("Yetersiz bakiye.");

            customer.Age -= amount; 
            await _customerService.UpdateAsync(customer);

            return Ok(customer);
        }

        [HttpPatch("{id}/increase-balance")]
        public async Task<IActionResult> IncreaseBalance(Guid id, [FromQuery] int amount)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound("Müşteri bulunamadı.");

            customer.Age += amount; 
            await _customerService.UpdateAsync(customer);

            return Ok(customer);
        }





    }
}
