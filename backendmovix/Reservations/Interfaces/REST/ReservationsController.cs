using backendmovix.Reservations.Applications.Internal.Service;
using backendmovix.Reservations.Domain.Model.Aggregate;
using backendmovix.Reservations.Interfaces.REST.Resources;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Reservations.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IReservationService _service;

        public ReservationsController(AppDbContext context, IReservationService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _service.ListAsync();
            var resources = reservations.Select(r => new ReservationResource
            {
                Id = r.Id,
                CantDate = r.CantDate,
                ScooterId = r.ScooterId,
                UserId = r.UserId,
                SuscriptionId = r.SuscriptionId
            });
            return Ok(resources);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener la suscripción y su tipo para determinar el mensaje
            var suscription = await _context.Suscriptions
                .Include(s => s.Type)
                .FirstOrDefaultAsync(s => s.Id == resource.SuscriptionId);

            if (suscription == null)
                return BadRequest("Suscription not found");

            string cantDate = suscription.Type.Name switch
            {
                "Plan Semanal" => "Esta reserva es válida por 7 días (Plan Semanal).",
                "Plan Mensual" => "Esta reserva es válida por 30 días (Plan Mensual).",
                "Plan Trimestral" => "Esta reserva es válida por 90 días (Plan Trimestral)."
            };

            var reservation = new Reservation
            {
                CantDate = cantDate,
                ScooterId = resource.ScooterId,
                UserId = resource.UserId,
                SuscriptionId = resource.SuscriptionId
            };

            await _service.CreateAsync(reservation);

            return Ok(new ReservationResource
            {
                Id = reservation.Id,
                CantDate = reservation.CantDate,
                ScooterId = reservation.ScooterId,
                UserId = reservation.UserId,
                SuscriptionId = reservation.SuscriptionId
            });
        }
    }
}
