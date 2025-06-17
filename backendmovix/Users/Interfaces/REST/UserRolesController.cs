using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Users.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _context.UserRoles
                .Select(r => new
                {
                    Id = r.Id,
                    Role = r.Role
                })
                .ToListAsync();

            return Ok(roles);
        }
    }
}
