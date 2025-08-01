﻿namespace Staj_2gun.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
