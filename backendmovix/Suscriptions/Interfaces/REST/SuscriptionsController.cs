using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Suscriptions.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Suscriptions.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SuscriptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuscriptionsController(AppDbContext context)
        {
            _context = context;
        }

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
                    TypeId = s.TypeId
                })
                .ToListAsync();

            return Ok(suscriptions);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSuscriptionResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var suscription = new Suscription
            {
                Number = resource.Number,
                Date = resource.Date,
                Cvv = resource.Cvv,
                TypeId = resource.TypeId
            };

            _context.Suscriptions.Add(suscription);
            await _context.SaveChangesAsync();

            return Ok(new SuscriptionResource
            {
                Id = suscription.Id,
                Number = suscription.Number,
                Date = suscription.Date,
                Cvv = suscription.Cvv,
                TypeId = suscription.TypeId
            });
        }
    }
}
