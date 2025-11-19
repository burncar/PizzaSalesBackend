using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Data;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Models.Response;



namespace OrderDetailSalesBackend.Application.Repository
{
    public class OrderDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<ApiResponse> GetAll(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var query = _db.OrderDetails
                .Include(a => a.Pizza)
                .ThenInclude(p => p.PizzaType)
                .Include(a => a.Order)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(q => q.Pizza.PizzaType.Name.Contains(SearchString));
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
        public async Task<OrderDetail> GetById(int id)
        {
            var data = await _db.OrderDetails
                .Include(a => a.Pizza)
                .ThenInclude(p => p.PizzaType)
                .Include(a => a.Order)
                .FirstOrDefaultAsync(a => a.OrderDetailsId == id);
            return data;
        }

        public async Task<OrderDetail> Add(OrderDetail entity)
        {
            if (entity != null)
            {
                try
                {
                    await _db.OrderDetails.AddAsync(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<bool> AddRange(IEnumerable<OrderDetail> entity)
        {

            if (entity != null)
            {
                try
                {
                    await _db.OrderDetails.AddRangeAsync(entity);
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public async Task<bool> Remove(OrderDetail entity)
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
