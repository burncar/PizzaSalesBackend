using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaSalesBackend.Application.Services;
using PizzaSalesBackend.Model.Dto;
using PizzaTypeSalesBackend.Application.Services;

namespace PizzaSalesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaTypesController : ControllerBase
    {
        private readonly PizzaTypeService _service;
        public PizzaTypesController(PizzaTypeService service)
        {
            _service = service;
        }
        [HttpPost("import-file")]
        public async Task<IActionResult> AddNultiplePizzaType([FromQuery] string path)
        {
            var result = await _service.AddMultiplePizzaType(path);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPizzaType([FromBody] PizzaTypeDtoAdd dto)
        {
            var response = await _service.AddPizzaType(dto);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePizzaType(string id,[FromBody] PizzaTypeDto dto)
        {
            var response = await _service.UpdatePizzaType(id, dto);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPizzaType(string SearchString = "", int PageNumber = 1, int PageSize = 20)
        {
            var result = await _service.GetAllPizzaTypes(SearchString, PageNumber, PageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizzaType(string id)
        {
            var res = await _service.GetPizzaTypeById(id);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzaType(string id)
        {
            var response = await _service.RemovePizzaType(id);
            return Ok(response);
        }
    }
}
