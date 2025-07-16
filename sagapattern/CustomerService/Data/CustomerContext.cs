﻿using Microsoft.EntityFrameworkCore;
using CustomerService.Models; 

namespace CustomerService.Data
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
}
