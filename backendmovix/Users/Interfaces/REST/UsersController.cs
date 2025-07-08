using backendmovix.Users.Application.Internal.Service;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Users.Domain.Model.Aggregate;
using backendmovix.Users.Interfaces.REST.Resources;
using backendmovix.Shared.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backendmovix.Users.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UsersController(AppDbContext context, IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        
        [HttpGet]
        [AllowAnonymous]
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
        [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var token = _jwtTokenGenerator.GenerateToken(user);

            return Ok(new
            {
                token,
                user.Id,
                user.Name,
                user.Email,
                user.RoleId,
                user.Photo
            });
        }
        
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "desconocido";
            return Ok(new { message = $"Estás autenticado como {email}" });
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar que RoleId exista para evitar error FK
            var roleExists = await _context.UserRoles.AnyAsync(r => r.Id == resource.RoleId);
            if (!roleExists)
                return BadRequest(new { message = "El RoleId asignado no existe." });

            var user = new User
            {
                // No asignar Id manualmente; dejar que la BD lo haga
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

            return Ok(new
            {
                user.Id,
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
        [Authorize]
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

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
