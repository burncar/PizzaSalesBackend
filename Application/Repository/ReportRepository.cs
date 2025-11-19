using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Data;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Models.Response;

namespace PizzaSalesBackend.Application.Repository
{
    public class ReportRepository
    {
        private readonly ApplicationDbContext _db;
        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ApiResponse> TopSellingPizza(
            DateTime fromDate=default,
            DateTime toDate=default,
            string SearchString = "", 
            int PageNumber = 1, 
            int PageSize = 10)
        {

            var topPizzas = _db.OrderDetails
                .Include(a => a.Order)
                .Include(a => a.Pizza)
                .ThenInclude(a => a.PizzaType)
                .AsQueryable();
            if (fromDate != default && toDate != default)
                topPizzas = topPizzas.Where(a => a.Order.Date >= fromDate.Date && a.Order.Date <= toDate.Date);

            if (!string.IsNullOrWhiteSpace(SearchString))
            {

                topPizzas = topPizzas.Where(q => q.Pizza.PizzaType.Name.Contains(SearchString));
            }

            var filteredTopPizzas = topPizzas.GroupBy(od => od.Pizza.PizzaType.Name)
                .Select(g => new
                {
                    
                    PizzaName = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.Pizza.Price),
                    FirstOrderDate = g.Min(x => x.Order.Date),
                    LastOrderDate = g.Max(x => x.Order.Date),
                })
                .OrderByDescending(x => x.TotalSales).AsQueryable();
                
           
            int count = await filteredTopPizzas.CountAsync();
            filteredTopPizzas = filteredTopPizzas
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);

            var res = new ApiResponse
            {
                TotalCount = count,
                IsSuccess = true,
                Result = await filteredTopPizzas.ToListAsync(),
                StatusCode = System.Net.HttpStatusCode.OK,
            };
            return res;
        }

        public async Task<ApiResponse> MonthlySales(
            DateTime fromDate = default,
            DateTime toDate = default,
           int PageNumber = 1, int PageSize = 10)
        {
            var query = _db.OrderDetails
                .Include(a => a.Order)
                .Include(a => a.Pizza)
                .ThenInclude(a => a.PizzaType)
                .OrderBy(a => a.Order.Date.Date)
                .AsQueryable();

            if (fromDate != default && toDate != default)
                query = query.Where(a => a.Order.Date >= fromDate.Date && a.Order.Date <= toDate.Date);
            var monthlySales = query
                
                .GroupBy(o => new { o.Order.Date.Year, o.Order.Date.Month })
                .Select(g => new
                {
                    OrderYear = g.Key.Year,
                    OrderMonth = g.Key.Month,
                    Date = $"{g.Key.Month}/{g.Key.Year}",
                    TotalOrders = g.Select(o => o.OrderId).Distinct().Count(), // count unique orders
                    TotalRevenue = g.Sum(o => o.Quantity * o.Pizza.Price),
                    TotalItemsSold = g.Sum(o => o.Quantity),
                }).OrderBy(a => a.OrderYear)
                .ThenBy(a => a.OrderMonth)
                .Select(a => new
                {
                    a.Date,
                    a.TotalOrders,
                    a.TotalRevenue,
                    a.TotalItemsSold
                })
                .AsQueryable();

            int count = await monthlySales.CountAsync();
            monthlySales = monthlySales
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);

            var res = new ApiResponse
            {
                TotalCount = 0,
                IsSuccess = true,
                Result = await monthlySales.ToListAsync(),
                StatusCode = System.Net.HttpStatusCode.OK,
            };
            return res;


        }




    }
}
