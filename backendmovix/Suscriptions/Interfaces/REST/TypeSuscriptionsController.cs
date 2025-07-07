using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Suscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Suscriptions.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TypeSuscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TypeSuscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.TypeSuscriptions
                .Select(t => new TypeSuscriptionResource
                {
                    Id = t.Id,
                    Name = t.Name,
                    Costo = t.Costo
                })
                .ToListAsync();

            return Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TypeSuscriptionDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Costo))
                return BadRequest("Datos inválidos.");

            var type = new Domain.Model.Aggregate.TypeSuscription
            {
                Name = dto.Name,
                Costo = dto.Costo
            };

            _context.TypeSuscriptions.Add(type);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = type.Id }, type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TypeSuscriptionDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Costo))
                return BadRequest("Datos inválidos.");

            var type = await _context.TypeSuscriptions.FindAsync(id);
            if (type == null)
                return NotFound();

            type.Name = dto.Name;
            type.Costo = dto.Costo;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.TypeSuscriptions.FindAsync(id);
            if (type == null)
                return NotFound();

            var hasSuscriptions = await _context.Suscriptions.AnyAsync(s => s.TypeId == id);
            if (hasSuscriptions)
                return BadRequest("No se puede eliminar el tipo porque hay suscripciones asociadas.");

            _context.TypeSuscriptions.Remove(type);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}