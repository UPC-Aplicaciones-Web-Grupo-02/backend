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
    }
}
