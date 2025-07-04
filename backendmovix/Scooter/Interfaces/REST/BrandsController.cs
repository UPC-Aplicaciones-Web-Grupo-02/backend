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
    }
}
