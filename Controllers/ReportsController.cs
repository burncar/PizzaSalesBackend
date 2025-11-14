using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderSalesBackend.Application.Services;
using PizzaSalesBackend.Application.Services;

namespace PizzaSalesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _service;
        public ReportsController(ReportService service)
        {
            _service = service;
        }
        [HttpGet("TopSellingPizzas")]
        public async Task<IActionResult> GetTopSellingPizzas(
            string SearchString = "",
            int PageNumber = 1,
            int PageSize = 10)
        {
            var result = await _service.GetTopSellingPizzas(SearchString, PageNumber, PageSize);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("MonthlySales")]
        public async Task<IActionResult> GetMonthlySales(
            string SearchString = "",
            int PageNumber = 1,
            int PageSize = 10)
        {
            var result = await _service.GetMonthlySales(SearchString, PageNumber, PageSize);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
