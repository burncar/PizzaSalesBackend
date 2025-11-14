using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Data;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Models.Response;



namespace OrderSalesBackend.Application.Repository
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<ApiResponse> GetAll(
            DateTime FromDate = default,
            DateTime ToDate = default,

            int PageNumber = 1, 
            int PageSize = 20)
        {
            var fromTime = TimeOnly.FromDateTime(FromDate).ToTimeSpan();
            var toTime = TimeOnly.FromDateTime(ToDate).ToTimeSpan();
            var query = _db.Orders.AsQueryable();
            if (FromDate != default && ToDate != default)
            {
                query = query.Where(a => a.Date >= FromDate.Date 
                && a.Date <= ToDate.Date);
                query = query.Where(a => a.Time >= fromTime
                && a.Time <= toTime);
                
            }
     
            int count = await query.CountAsync();
            query = query
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);

            var res = new ApiResponse
            {
                TotalCount = count,
                IsSuccess = true,
                Result = await query.ToListAsync(),
                StatusCode = System.Net.HttpStatusCode.OK,
            };

            return res;
        }
        public async Task<Order> GetById(int id)
        {
            var data = await _db.Orders
                 //.Include(a => a.OrderType)
                 .FirstOrDefaultAsync(e => e.OrderId == id);
            return data;
        }

        public async Task<Order> Add(Order entity)
        {
            if (entity != null)
            {
                try
                {
                    await _db.Orders.AddAsync(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<bool> AddRange(IEnumerable<Order> entity)
        {

            if (entity != null)
            {
                try
                {
                    await _db.Orders.AddRangeAsync(entity);
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public async Task<bool> Remove(Order entity)
        {
            try
            {
                _db.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



    }
}
