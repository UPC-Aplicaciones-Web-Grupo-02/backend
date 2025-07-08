using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Users.Domain.Model.Aggregate;
using backendmovix.Users.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Users.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous] 
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreaterUserRoleResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Role))
                return BadRequest("Datos inválidos.");

            var role = new UserRole { Role = resource.Role };
            _context.UserRoles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRoleResource resource)
        {
            if (resource == null || string.IsNullOrWhiteSpace(resource.Role))
                return BadRequest("Datos inválidos.");

            var role = await _context.UserRoles.FindAsync(id);
            if (role == null)
                return NotFound();

            role.Role = resource.Role;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.UserRoles.FindAsync(id);
            if (role == null)
                return NotFound();

            var hasUsers = await _context.Users.AnyAsync(u => u.RoleId == id);
            if (hasUsers)
                return BadRequest("No se puede eliminar el rol porque hay usuarios asociados.");

            _context.UserRoles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
