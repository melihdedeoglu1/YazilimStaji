using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Staj_2gun.Data;
using Staj_2gun.Models;

namespace Staj_2gun.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll() => _context.Orders.ToList();

        public Order? GetById(int id) => _context.Orders.FirstOrDefault(o => o.Id == id);

        public void Add(Order order)
        {
            IDbContextTransaction? transaction = null;

            try
            {
                // Sadece ilişkisel veritabanında transaction başlat
                if (_context.Database.IsRelational())
                {
                    transaction = _context.Database.BeginTransaction();
                }

                foreach (var item in order.OrderItems)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);

                    if (product == null)
                        throw new Exception($"Ürün bulunamadı: ID = {item.ProductId}");

                    if (product.Stock < item.Quantity)
                        throw new Exception($"Yetersiz stok: {product.Name} için {product.Stock} adet kaldı");

                    product.Stock -= item.Quantity;
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Transaction varsa commit et
                transaction?.Commit();
            }
            catch (Exception)
            {
                // Transaction varsa rollback et
                transaction?.Rollback();
                throw;
            }
        }


        public List<object> GetCustomerOrderDetails(int customerId)
        {
            var result = (from o in _context.Orders
                          join c in _context.Customers on o.CustomerId equals c.Id
                          join oi in _context.OrderItems on o.Id equals oi.OrderId
                          join p in _context.Products on oi.ProductId equals p.Id
                          where o.CustomerId == customerId
                          select new
                          {
                              OrderId = o.Id,
                              CustomerName = c.Name,
                              ProductName = p.Name,
                              Quantity = oi.Quantity
                          }).ToList<object>(); // anonymous type olduğu için List<object>

            return result;
        }



    }
}