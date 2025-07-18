using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using UnitTestOrnek.DTOs;
using UnitTestOrnek.Models;
using UnitTestOrnek.Repositories;
using UnitTestOrnek.Services;

namespace UnitTestOrnek.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new OrderService(_orderRepositoryMock.Object, _mapperMock.Object);
        }


        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
        {
            //arrange hazırlık işlemleri

            var orders = new List<Order>
            {
                new Order { Id = 1, ProductName = "testurun1" , Total = 100, CustomerId = 1  },
                new Order { Id = 2, ProductName = "testurun2" , Total = 200, CustomerId = 2  }
            };

            _orderRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.GetAllOrdersAsync();


            //assert karşılaştırma işlemleri

            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("testurun1", result[0].ProductName);
            Assert.Equal(100, result[0].Total);
            Assert.Equal(1, result[0].CustomerId);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("testurun2", result[1].ProductName);
            Assert.Equal(200, result[1].Total);
            Assert.Equal(2, result[1].CustomerId);

            _orderRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);

        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenIdExists()
        {
            //arrange hazırlık işlemleri

            var order = new Order { Id = 1, ProductName = "testurun", Total = 100, CustomerId = 1 };

            _orderRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.GetOrderByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testurun", result.ProductName);
            Assert.Equal(100, result.Total);
            Assert.Equal(1, result.CustomerId);

            _orderRepositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetOrderWithCustomerByIdAsync_ShouldReturnOrderWithCustomer_WhenIdExists()
        {
            //arrange hazırlık işlemleri

            var order = new Order { Id = 1, ProductName = "testurun", Total = 100, CustomerId = 1 };
            order.Customer = new Customer { Id = 1, Name = "testcustomer" };

            _orderRepositoryMock.Setup(r => r.GetOrderWithCustomerByIdAsync(1)).ReturnsAsync(order);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.GetOrderWithCustomerByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testurun", result.ProductName);
            Assert.Equal(100, result.Total);
            Assert.Equal(1, result.CustomerId);
            Assert.NotNull(result.Customer);
            Assert.Equal(1, result.Customer.Id);
            Assert.Equal("testcustomer", result.Customer.Name);

            _orderRepositoryMock.Verify(r => r.GetOrderWithCustomerByIdAsync(1), Times.Once);

        }


        [Fact]
        public async Task CreateOrderAsync_ShouldReturnCreatedOrder()
        {
            //arrange hazırlık işlemleri

            var orderDto = new OrderDto { ProductName = "testurun", Total = 100, CustomerId = 1 };

            var mappedOrder = new Order { Id = 1, ProductName = "testurun", Total = 100, CustomerId = 1 };

            _mapperMock.Setup(m => m.Map<Order>(orderDto)).Returns(mappedOrder);

            _orderRepositoryMock.Setup(r => r.CreateAsync(mappedOrder)).ReturnsAsync(mappedOrder);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.CreateOrderAsync(orderDto);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("testurun", result.ProductName);
            Assert.Equal(100, result.Total);
            Assert.Equal(1, result.CustomerId);

            _mapperMock.Verify(m => m.Map<Order>(orderDto), Times.Once);

            _orderRepositoryMock.Verify(r => r.CreateAsync(mappedOrder), Times.Once);

        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldReturnTrue_WhenOrderUpdated()
        {
            //arrange hazırlık işlemleri

            var id = 1;
            var orderDto = new OrderDto { ProductName = "updatedurun", Total = 150, CustomerId = 2 };
            var updatedOrder = new Order { Id = id, ProductName = "updatedurun", Total = 150, CustomerId = 2 };

            _mapperMock.Setup(m => m.Map<Order>(orderDto)).Returns(updatedOrder);

            _orderRepositoryMock.Setup(r => r.UpdateAsync(id, updatedOrder)).ReturnsAsync(true);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.UpdateOrderAsync(id, orderDto);


            //assert karşılaştırma işlemleri

            Assert.True(result);

            _mapperMock.Verify(m => m.Map<Order>(orderDto), Times.Once);

            _orderRepositoryMock.Verify(r => r.UpdateAsync(id, updatedOrder), Times.Once);

        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldReturnTrue_WhenOrderDeleted()
        {
            //arrange hazırlık işlemleri

            var id = 1;
            _orderRepositoryMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);


            //act fonksiyonun çağrılma işlemleri

            var result = await _service.DeleteOrderAsync(id);


            //assert karşılaştırma işlemleri

            Assert.True(result);
            _orderRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);

        }

    }
}
