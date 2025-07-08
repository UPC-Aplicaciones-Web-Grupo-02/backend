using backendmovix.Scooter.Interfaces.REST.Resources;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Scooter.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] 
    public class DistrictsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DistrictsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var districts = await _context.Districts
                .Select(d => new DistrictResource
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();

            return Ok(districts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var district = await _context.Districts
                .Select(d => new DistrictResource
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            if (district == null)
                return NotFound();

            return Ok(district);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistrictResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var district = new Domain.Model.Aggregate.Districts { Name = resource.Name };
            _context.Districts.Add(district);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = district.Id }, district);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDistrictResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var district = await _context.Districts.FindAsync(id);
            if (district == null)
                return NotFound();

            district.Name = resource.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var district = await _context.Districts.FindAsync(id);
            if (district == null)
                return NotFound();

            var hasScooters = await _context.Scooters.AnyAsync(s => s.DistrictId == id);
            if (hasScooters)
                return BadRequest("No se puede eliminar el distrito porque hay scooters asociados.");

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
