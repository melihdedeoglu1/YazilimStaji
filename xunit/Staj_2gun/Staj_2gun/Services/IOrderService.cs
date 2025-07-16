using Microsoft.EntityFrameworkCore;
using Staj_2gun.Models;

namespace Staj_2gun.Services
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order? GetById(int id);
        void Add(Order order);
        List<object> GetCustomerOrderDetails(int customerId);
       
    }
}
