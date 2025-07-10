using Staj_2gun.Data;
using Staj_2gun.Models;
using Staj_2gun.Services;
using Microsoft.EntityFrameworkCore;

namespace Staj_2gun.Tests
{
    public class OrderServiceTests
    {
        private readonly AppDbContext _context;
        private readonly OrderService _orderService;

        [Fact]
        public void GetAll_ShouldReturnAllOrders()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllOrdersDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Orders.Add(new Order { Id = 1, CustomerId = 1 });
                context.Orders.Add(new Order { Id = 2, CustomerId = 2 });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new OrderService(context);

                // Act
                var result = service.GetAll();

                // Assert
                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public void GetById_ShouldReturnCorrectOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("GetOrderByIdDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Orders.Add(new Order { Id = 10, CustomerId = 1 });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var service = new OrderService(context);
                var order = service.GetById(10);
                Assert.NotNull(order);
                Assert.Equal(10, order.Id);
            }
        }

        [Fact]
        public void Add_ShouldAddOrder()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AddOrderTestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var orderService = new OrderService(context);

                var customer = new Customer { Id = 1, Name = "Test Müşteri" };
                var product = new Product { Id = 1, Name = "Kalem", Stock = 10 };

                context.Customers.Add(customer);
                context.Products.Add(product);
                context.SaveChanges();

                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = product.Id, Quantity = 2 }
            }
                };

                orderService.Add(order);

                var savedOrder = context.Orders.Include(o => o.OrderItems).FirstOrDefault();
                Assert.NotNull(savedOrder);
                Assert.Equal(customer.Id, savedOrder.CustomerId);
                Assert.Single(savedOrder.OrderItems);
            }
        }



    }
}
