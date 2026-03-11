using App.Manager.Countries;
using App.Manager.Countries.DTO;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController(ICountryManager CountryManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await CountryManager.GetAllAsync();
            return Ok(countries);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var country = await CountryManager.GetAsync(id);
                return Ok(country);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryRequestDTO request)
        {
            var dto = new CreateCountryRequestDTO(request.Name, request.Code, request.Latitude, request.Longitude);
            var created = await CountryManager.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryRequestDTO request)
        {
            if (id != request.Id)
            {
                return BadRequest("Id in URL and request body must match");
            }

            try
            {
                var updated = await CountryManager.UpdateAsync(request);
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
                await CountryManager.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
