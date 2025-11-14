using Microsoft.EntityFrameworkCore;
using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Data;
using PizzaSalesBackend.Model.Entities;
using PizzaSalesBackend.Models.Response;



namespace PizzaSalesBackend.Application.Repository
{
    public class PizzaRepository
    {
        private readonly ApplicationDbContext _db;
       
        public PizzaRepository(ApplicationDbContext db)
        {
            _db = db;
          
        }
        public async Task<ApiResponse> GetAll(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var query = _db.Pizzas.Include(a => a.PizzaType).AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(q => q.Price.ToString().Contains(SearchString));
                query = query.Where(q => q.Size.ToString().Contains(SearchString));
                query = query.Where(q => q.PizzaType.Name.Contains(SearchString));
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
        public async Task<Pizza> GetById(string id)
        {
            var data = await _db.Pizzas
                 .Include(a => a.PizzaType)
                 .FirstOrDefaultAsync(e => e.PizzaId.Trim().ToLower() == id.Trim().ToLower());
            return data;
        }

        public async Task<Pizza> Add(Pizza entity)
        {
            if (entity != null)
            {
                try 
                {
                  await _db.Pizzas.AddAsync(entity);
                  return entity;
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
        public async Task<bool> AddRange(IEnumerable<Pizza> entity)
        {

            if (entity != null)
            {
                try
                {
                    await _db.Pizzas.AddRangeAsync(entity);
                }catch(Exception ex)
                {
                    return false;
                }

                return true;    
            }
            return false;
        }

        public async Task<bool> Remove(Pizza entity)
        {
            try
            {
               _db.Remove(entity);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }

     

    }
}
