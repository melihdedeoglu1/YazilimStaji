using AutoMapper;
using Castle.Core.Resource;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestOrnek.DTOs;
using UnitTestOrnek.Models;
using UnitTestOrnek.Repositories;
using UnitTestOrnek.Services;

namespace UnitTestOrnek.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly CustomerService _service;

        public CustomerServiceTests() 
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenIdExists() 
        {
            //arrange hazırlık işlemleri

            var customer = new Customer { Id = 1, Name = "test" };

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.GetCustomerByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test", result.Name);

            _customerRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);

        }

        [Fact]
        public async Task GetAllCustomerAsync_ShouldReturnAllCustomers() 
        {
            //arrange hazırlık işlemleri

            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "test1" },
                new Customer { Id = 2, Name = "test2" }
            };  

            _customerRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.GetAllCustomersAsync();


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("test1", result[0].Name);
            Assert.Equal(1,result[0].Id);
            Assert.Equal("test2", result[1].Name);
            Assert.Equal(2, result[1].Id);  

            _customerRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnCreatedCustomer()
        {
            //arrange hazırlık işlemleri

            var createCustomerDto = new CreateCustomerDto { Name = "test" };
            var mappedCustomer = new Customer { Id = 1, Name = "test" };

            _mapperMock.Setup(m => m.Map<Customer>(createCustomerDto)).Returns(mappedCustomer);

            _customerRepositoryMock.Setup(r => r.CreateAsync(mappedCustomer)).ReturnsAsync(mappedCustomer);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.CreateCustomerAsync(createCustomerDto); 


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal("test", result.Name);
            Assert.Equal(1, result.Id);

            _mapperMock.Verify(m => m.Map<Customer>(createCustomerDto), Times.Once);

            _customerRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Once);

        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnTrue_WhenUpdateIsSuccessful() 
        {
            //arrange hazırlık işlemleri

            var id = 1;
            var createCustomerDto = new CreateCustomerDto { Name = "updated" };
            var updatedCustomer = new Customer { Id = id, Name = createCustomerDto.Name };

            _mapperMock.Setup(m => m.Map<Customer>(createCustomerDto)).Returns(updatedCustomer);

            _customerRepositoryMock.Setup(r => r.UpdateAsync(id, updatedCustomer)).ReturnsAsync(true);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.UpdateCustomerAsync(id, createCustomerDto);


            //assert karşılaştırma işlemleri

            Assert.True(result);

            _mapperMock.Verify(m => m.Map<Customer>(createCustomerDto), Times.Once);
            _customerRepositoryMock.Verify(r => r.UpdateAsync(id, updatedCustomer), Times.Once);

        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnTrue_WhenDeleteIsSuccessful() 
        {
            //arrange hazırlık işlemleri

            var id = 1;

            _customerRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.DeleteCustomerAsync(id);


            //assert karşılaştırma işlemleri

            Assert.True(result);
            _customerRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);

        }






    }
}
