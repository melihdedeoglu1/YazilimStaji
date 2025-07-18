using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTestOrnek.Data;
using UnitTestOrnek.Models;
using UnitTestOrnek.Repositories;


namespace UnitTestOrnek.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private Context GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new Context(options);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenExists() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new CustomerRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.GetByIdAsync(1);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.Name, result.Name);

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers() 
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new CustomerRepository(context);

            var customer = new List<Customer>
            {
                new Customer { Id = 1, Name = "Test Customer 1" },
                new Customer { Id = 2, Name = "Test Customer 2" }
            };

            context.Customers.AddRange(customer);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.GetAllAsync();


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Test Customer 1", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("Test Customer 2", result[1].Name);

        }

        [Fact]
        public async Task CreateAsync_ShouldAddCustomer() 
        { 
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();   
            
            var repository = new CustomerRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.CreateAsync(customer);


            //assert karşılaştırma işlemleri

            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.Name, result.Name);

            Assert.Equal(1, context.Customers.Count());

        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer_WhenExists()
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new CustomerRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Name" };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            customer.Name = "Updated Name";
            var updatedCustomer = customer;


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.UpdateAsync(customer.Id, updatedCustomer);


            //assert karşılaştırma işlemleri

            Assert.True(result);

            var customerfromDb = await context.Customers.FindAsync(customer.Id);
            Assert.NotNull(customerfromDb);
            Assert.Equal("Updated Name", customerfromDb.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCustomer_WhenIdExists()
        {
            //arrange hazırlık işlemleri

            var context = GetInMemoryDbContext();

            var repository = new CustomerRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();


            //act fonksiyonun çağrılma işlemleri

            var result = await repository.DeleteAsync(customer.Id);


            //assert karşılaştırma işlemleri

            Assert.True(result);

            var customerfromDb = await context.Customers.FindAsync(customer.Id);
            Assert.Null(customerfromDb); 

        }



    }
}
