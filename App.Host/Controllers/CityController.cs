using App.Manager.Cities;
using App.Manager.Cities.DTO;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController(ICityManager CityManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await CityManager.GetAllAsync();
            return Ok(cities);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var city = await CityManager.GetAsync(id);
                return Ok(city);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityRequestDTO request)
        {
            var dto = new CreateCityRequestDTO(request.Name, request.CountryId);
            var created = await CityManager.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCityRequestDTO request)
        {
            if (id != request.Id)
            {
                return BadRequest("Id in URL and request body must match");
            }

            try
            {
                var updated = await CityManager.UpdateAsync(request);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await CityManager.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
