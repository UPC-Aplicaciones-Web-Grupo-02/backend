using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Suscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backendmovix.Suscriptions.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SuscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/Suscriptions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suscriptions = await _context.Suscriptions
                .Select(s => new SuscriptionResource
                {
                    Id = s.Id,
                    Number = s.Number,
                    Date = s.Date,
                    Cvv = s.Cvv,
                    TypeId = s.TypeId,
                    UserId = s.UserId
                })
                .ToListAsync();

            return Ok(suscriptions);
        }

        // GET: api/v1/Suscriptions/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMySuscription()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var suscription = await _context.Suscriptions
                .Where(s => s.UserId == userId)
                .Select(s => new SuscriptionResource
                {
                    Id = s.Id,
                    Number = s.Number,
                    Date = s.Date,
                    Cvv = s.Cvv,
                    TypeId = s.TypeId,
                    UserId = s.UserId
                })
                .FirstOrDefaultAsync();

            if (suscription == null)
                return NotFound();

            return Ok(suscription);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSuscriptionResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var suscription = new Suscription
            {
                Number = resource.Number,
                Date = resource.Date,
                Cvv = resource.Cvv,
                TypeId = resource.TypeId,
                UserId = userId
            };

            _context.Suscriptions.Add(suscription);
            await _context.SaveChangesAsync();

            return Ok(new SuscriptionResource
            {
                Id = suscription.Id,
                Number = suscription.Number,
                Date = suscription.Date,
                Cvv = suscription.Cvv,
                TypeId = suscription.TypeId,
                UserId = userId
            });
        }
    }
}
