using backendmovix.Scooter.Interfaces.REST.Resources;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Scooter.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ModelsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var models = await _context.Models
                .Select(m => new ModelResource
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToListAsync();

            return Ok(models);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _context.Models
                .Select(m => new ModelResource
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModelResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var model = new Domain.Model.Aggregate.Models { Name = resource.Name };
            _context.Models.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateModelResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var model = await _context.Models.FindAsync(id);
            if (model == null)
                return NotFound();

            model.Name = resource.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
                return NotFound();

            var hasScooters = await _context.Scooters.AnyAsync(s => s.ModelId == id);
            if (hasScooters)
                return BadRequest("No se puede eliminar el modelo porque hay scooters asociados.");

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}