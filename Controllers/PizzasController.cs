using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaSalesBackend.Application.Services;
using PizzaSalesBackend.Model.Dto;

namespace PizzaSalesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly PizzaService _service;
        public PizzasController(PizzaService service)
        {
            _service = service;
        }
        [HttpPost("import-file")]
        public async Task<IActionResult> AddMultiplePizza([FromQuery] string path)
        {
            var result = await _service.AddMultiplePizza(path);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> AddPizza([FromBody] PizzaDtoAdd dto)
        {
            var response = await _service.AddPizza(dto);

            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePizzaType(string id, [FromBody] PizzaDto dto)
        {
            var response = await _service.UpdatePizza(id, dto);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPizza(string SearchString = "", int PageNumber = 1, int PageSize = 20)
        {
            var result = await _service.GetAllPizzas(SearchString,PageNumber,PageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizza(string id)
        {
            var res = await _service.GetPizzaById(id);
            return Ok(res);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzaType(string id)
        {
            var response = await _service.RemovePizza(id);
            return Ok(response);
        }



    }
}
