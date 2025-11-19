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
             DateTime fromDate = default,
            DateTime toDate = default,
            string SearchString = "",
            int PageNumber = 1,
            int PageSize = 10)
        {
            var result = await _service.GetTopSellingPizzas(fromDate, toDate, SearchString, PageNumber, PageSize);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("MonthlySales")]
        public async Task<IActionResult> GetMonthlySales(
             DateTime fromDate = default,
            DateTime toDate = default,
            int PageNumber = 1,
            int PageSize = 10)
        {
            var result = await _service.GetMonthlySales(fromDate,toDate, PageNumber, PageSize);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
