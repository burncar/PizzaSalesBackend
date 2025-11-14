using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Model.Entities;
using System;

namespace PizzaSalesBackend.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PizzaType> PizzaTypes { get; set; }


    }
}
