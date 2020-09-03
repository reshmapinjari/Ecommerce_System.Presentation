using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Console_Ecommerce_System.Entities;

namespace Ecommerce_WebApi.Model
{
   
        public class EcommercedbContext : DbContext
        {
            public EcommercedbContext(DbContextOptions<EcommercedbContext> options)
                : base(options)
            {

            }
            public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCancel> OrderCancels { get; set; }
        public DbSet<OnlineReturn> OnlineReturns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerReport> CustomerReports { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<IUser> IUsers { get; set; }
    }
    }

