using Staj_2gun.Models;

namespace Staj_2gun.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.Customers.Any())
            {
                var customer = new Customer { Name = "Ali Veli" };
                var product = new Product { Name = "Kalem", Stock = 100 };

                context.Customers.Add(customer);
                context.Products.Add(product);
                context.SaveChanges();

                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderItems = new List<OrderItem>
                    {       
                        new OrderItem       
                        {      
                            ProductId = product.Id,         
                            Quantity = 3        
                        } 
                    }
                };

                context.Orders.Add(order);
                context.SaveChanges(); // ✅ EF Core önce Order'ı ekler, sonra OrderId'yi otomatik verir ve OrderItem'lara yansıtır
            }
        }
    }
}
