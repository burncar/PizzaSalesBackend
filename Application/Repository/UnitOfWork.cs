using Microsoft.AspNetCore.Identity;
using OrderDetailSalesBackend.Application.Repository;
using OrderSalesBackend.Application.Repository;
using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Data;
using PizzaTypeSalesBackend.Application.Repository;

namespace PizzaSalesBackend.Application.Repository
{
  
        public class UnitOfWork : IUnitOfWork
        {
            private readonly ApplicationDbContext _context;
            public PizzaRepository Pizzas { get; private set; }
            public PizzaTypeRepository PizzaTypes { get; private set; }
            
            public OrderRepository Orders { get; private set; }
            public OrderDetailRepository OrderDetails { get; private set; }
            
        public ReportRepository Reports { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
                _context = context;
               Pizzas = new PizzaRepository(_context);
               PizzaTypes = new PizzaTypeRepository(_context);
                Orders = new OrderRepository(_context);
                OrderDetails = new OrderDetailRepository(_context);
            Reports = new ReportRepository(_context);


        }



            public async Task<int> Complete()
            {
                return await _context.SaveChangesAsync();
            }
        }
}

