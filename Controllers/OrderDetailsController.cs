using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaSalesBackend.Application.Services;
using PizzaSalesBackend.Model.Dto;
using OrderDetailSalesBackend.Application.Services;

namespace PizzaSalesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailService _service;
        public OrderDetailsController(OrderDetailService service)
        {
            _service = service;
        }
        [HttpPost("import-file")]
        public async Task<IActionResult> AddNultipleOrderDetail([FromQuery] string path)
        {
            var result = await _service.AddMultipleOrderDetail(path);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail([FromBody] OrderDetailDto dto)
        {
            var response = await _service.AddOrderDetail(dto);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailDto dto)
        {
            var response = await _service.UpdateOrderDetail(id, dto);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetail(string SearchString="", int PageNumber = 1, int PageSize = 20)
        {
            var result = await _service.GetAllOrderDetails(SearchString, PageNumber, PageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var res = await _service.GetOrderDetailById(id);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var response = await _service.RemoveOrderDetail(id);
            return Ok(response);
        }
    }
}
