using backendmovix.Users.Application.Internal.Service;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Users.Domain.Model.Aggregate;
using backendmovix.Users.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Users.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;

        public UsersController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users
                .Select(u => new UserResource
                {
                    Id = u.Id,
                    Name = u.Name,
                    Phone = u.Phone,
                    Dni = u.Dni,
                    Email = u.Email,
                    Photo = u.Photo,
                    Address = u.Address,
                    RoleId = u.RoleId
                })
                .ToListAsync();

            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResource
                {
                    Id = u.Id,
                    Name = u.Name,
                    Phone = u.Phone,
                    Dni = u.Dni,
                    Email = u.Email,
                    Photo = u.Photo,
                    Address = u.Address,
                    RoleId = u.RoleId
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.RoleId,
                user.Photo
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Asignar Id manualmente
            var maxId = _context.Users.Any() ? _context.Users.Max(u => u.Id) : 0;

            var user = new User
            {
                Id = maxId + 1,
                Name = resource.Name,
                Phone = resource.Phone,
                Dni = resource.Dni,
                Email = resource.Email,
                Password = _userService.HashPassword(resource.Password),
                Photo = resource.Photo,
                Address = resource.Address,
                RoleId = resource.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Devuelve solo los datos relevantes (sin id ni role)
            return Ok(new
            {
                user.Name,
                user.Phone,
                user.Dni,
                user.Email,
                user.Photo,
                user.Address,
                user.RoleId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
