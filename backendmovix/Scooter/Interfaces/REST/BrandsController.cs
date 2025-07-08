using backendmovix.Scooter.Interfaces.REST.Resources;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Scooter.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BrandsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _context.Brands
                .Select(b => new BrandResource
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .ToListAsync();

            return Ok(brands);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _context.Brands
                .Select(b => new BrandResource
                {
                    Id = b.Id,
                    Name = b.Name
                })
                .FirstOrDefaultAsync(b => b.Id == id);

            if (brand == null)
                return NotFound();

            return Ok(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var brand = new Domain.Model.Aggregate.Brands { Name = resource.Name };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = brand.Id }, brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBrandResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Name))
                return BadRequest("Datos inválidos.");

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            brand.Name = resource.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            var hasScooters = await _context.Scooters.AnyAsync(s => s.BrandId == id);
            if (hasScooters)
                return BadRequest("No se puede eliminar la marca porque hay scooters asociados.");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}