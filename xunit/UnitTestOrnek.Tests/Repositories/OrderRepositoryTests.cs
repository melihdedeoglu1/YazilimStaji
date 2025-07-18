using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestOrnek.Data;
using UnitTestOrnek.Models;
using UnitTestOrnek.Repositories;

namespace UnitTestOrnek.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private Context GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new Context(options);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenExists() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var order = new Order { Id = 1, ProductName = "testurun", Total = 10.0, CustomerId = 1  };

            context.Orders.Add(order);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.GetByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.ProductName, result.ProductName);
            Assert.Equal(order.Total, result.Total);
            Assert.Equal(order.CustomerId, result.CustomerId);
            
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrders() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var orders = new List<Order>
            {
                new Order { Id = 1, ProductName = "testurun1", Total = 10.0, CustomerId = 1 },
                new Order { Id = 2, ProductName = "testurun2", Total = 20.0, CustomerId = 2 }
            };

            context.Orders.AddRange(orders);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.GetAllAsync();


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("testurun1", result[0].ProductName);
            Assert.Equal(10.0, result[0].Total);
            Assert.Equal(1, result[0].CustomerId);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("testurun2", result[1].ProductName);
            Assert.Equal(20.0, result[1].Total);
            Assert.Equal(2, result[1].CustomerId);

        }

        [Fact]
        public async Task GetOrderWithCustomerByIdAsync_ShouldReturnOrderWithCustomer_WhenExists() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var order = new Order { Id = 1, ProductName = "testurun", Total = 10.0, CustomerId = 1, Customer = customer };

            context.Customers.Add(customer);
            context.Orders.Add(order);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.GetOrderWithCustomerByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.ProductName, result.ProductName);
            Assert.Equal(order.Total, result.Total);
            Assert.Equal(order.CustomerId, result.CustomerId);
            Assert.NotNull(result.Customer);
            Assert.Equal(customer.Name, result.Customer.Name);

        }

        [Fact]
        public async Task CreateAsync_ShouldAddOrder() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var order = new Order { Id = 1, ProductName = "testurun", Total = 10.0, CustomerId = 1 };


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.CreateAsync(order);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.ProductName, result.ProductName);
            Assert.Equal(order.Total, result.Total);
            Assert.Equal(order.CustomerId, result.CustomerId);

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateOrder_WhenExists() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var order = new Order { Id = 1, ProductName = "testurun", Total = 10.0, CustomerId = 1 };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            order.ProductName = "updatedurun";
            order.Total = 15.0;
            order.CustomerId = 2;

            var updatedOrder = order;


            //act fonksiyonun çağrılma işlemleri
            
            var result = await repository.UpdateAsync(1, updatedOrder);


            //assert karşılaştırma işlemleri

            Assert.True(result);

            var updatedResult = await repository.GetByIdAsync(1);

            Assert.NotNull(updatedResult);
            Assert.Equal("updatedurun", updatedResult.ProductName);
            Assert.Equal(15.0, updatedResult.Total);
            Assert.Equal(2, updatedResult.CustomerId);

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteOrder_WhenExists() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new OrderRepository(context);

            var order = new Order { Id = 1, ProductName = "testurun", Total = 10.0, CustomerId = 1 };

            context.Orders.Add(order);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.DeleteAsync(1);


            //assert karşılaştırma işlemleri

            Assert.True(result);
            var deletedResult = await repository.GetByIdAsync(1);
            Assert.Null(deletedResult);

        }

    }
}
