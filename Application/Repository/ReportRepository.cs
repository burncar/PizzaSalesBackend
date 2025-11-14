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
        public async Task<ApiResponse> TopSellingPizza(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {

            var topPizzas = _db.OrderDetails
                //.Include(od => od.Pizza)
                //.ThenInclude(p => p.PizzaType)
                .GroupBy(od => od.Pizza.PizzaType.Name)
                .Select(g => new
                {
                    PizzaName = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.Pizza.Price)
                })
                .OrderByDescending(x => x.TotalSales).AsQueryable();
                
            if (!string.IsNullOrWhiteSpace(SearchString))
            {

                topPizzas = topPizzas.Where(q => q.PizzaName.Contains(SearchString));
            }
            int count = await topPizzas.CountAsync();
            topPizzas = topPizzas
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);

            var res = new ApiResponse
            {
                TotalCount = count,
                IsSuccess = true,
                Result = await topPizzas.ToListAsync(),
                StatusCode = System.Net.HttpStatusCode.OK,
            };
            return res;
        }

        public async Task<ApiResponse> MonthlySales(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var monthlySales = _db.Orders
                .GroupBy(o => new { o.Date.Year, o.Date.Month })
                .Select(g => new {
                    PizzaNames = g.SelectMany(o => o.OrderDetails)
                      .Select(od => od.Pizza.PizzaType.Name)
                      .Distinct()
                      .ToList(),
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    TotalOrders = g.Count(),
                    TotalRevenue = g.SelectMany(o => o.OrderDetails)
                                    .Sum(od => od.Quantity * od.Pizza.Price),
                    TotalItemsSold = g.Sum(o => o.OrderDetails
                     .Sum(od => od.Quantity))

                }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {

                monthlySales = monthlySales.Where(q => q.PizzaNames.Contains(SearchString));
            }
            int count = await monthlySales.CountAsync();
            monthlySales = monthlySales
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);

            var res = new ApiResponse
            {
                TotalCount = count,
                IsSuccess = true,
                Result = await monthlySales.ToListAsync(),
                StatusCode = System.Net.HttpStatusCode.OK,
            };
            return res;


        }




    }
}
