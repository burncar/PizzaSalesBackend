using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Data;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Models.Response;



namespace PizzaTypeSalesBackend.Application.Repository
{
    public class PizzaTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public PizzaTypeRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<ApiResponse> GetAll(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var query = _db.PizzaTypes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
               
                query = query.Where(q => q.Name.Contains(SearchString));
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
        public async Task<PizzaType> GetById(string id)
        {
            var data = await _db.PizzaTypes
                 //.Include(a => a.PizzaTypeType)
                 .FirstOrDefaultAsync(e => e.PizzaTypeId.Trim().ToLower() == id.Trim().ToLower());
            return data;
        }

        public async Task<PizzaType> Add(PizzaType entity)
        {
            if (entity != null)
            {
                try
                {
                    await _db.PizzaTypes.AddAsync(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<bool> AddRange(IEnumerable<PizzaType> entity)
        {

            if (entity != null)
            {
                try
                {
                    await _db.PizzaTypes.AddRangeAsync(entity);
                }
                catch (Exception ex)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public async Task<bool> Remove(PizzaType entity)
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
