using PizzaSalesBackend.Application.Intreface;
using PizzaSalesBackend.Models.Response;

namespace PizzaSalesBackend.Application.Services
{
    public class ReportService
    {
        private readonly IUnitOfWork _unitOfWork;
    
        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse> GetTopSellingPizzas(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var result = await _unitOfWork.Reports.TopSellingPizza(SearchString, PageNumber, PageSize);
            return result;
        }
        public async Task<ApiResponse> GetMonthlySales(string SearchString = "", int PageNumber = 1, int PageSize = 10)
        {
            var result = await _unitOfWork.Reports.MonthlySales(SearchString, PageNumber, PageSize);
            return result;
        }


    }
}
