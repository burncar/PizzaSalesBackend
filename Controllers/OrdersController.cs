using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaSalesBackend.Application.Services;
using PizzaSalesBackend.Model.Dto;
using OrderSalesBackend.Application.Services;

namespace PizzaSalesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        public OrdersController(OrderService service)
        {
            _service = service;
        }
        [HttpPost("import-file")]
        public async Task<IActionResult> AddNultipleOrder([FromQuery] string path)
        {
            var result = await _service.AddMultipleOrder(path);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDto dto)
        {
            var response = await _service.AddOrder(dto);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto dto)
        {
            var response = await _service.UpdateOrder(id, dto);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrder(DateTime FromDate = default, DateTime ToDate = default,int PageNumber = 1, int PageSize = 20)
        {
            var result = await _service.GetAllOrders(FromDate, ToDate,PageNumber, PageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var res = await _service.GetOrderById(id);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _service.RemoveOrder(id);
            return Ok(response);
        }
    }
}
