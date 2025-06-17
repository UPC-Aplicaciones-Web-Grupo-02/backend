using backendmovix.Scooter.Application.Internal.Service;
using backendmovix.Scooter.Interfaces.REST.Resources;
using backendmovix.Scooter.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace backendmovix.Scooter.Interfaces.REST
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ScooterController : ControllerBase
    {
        private readonly IScooterService _scooterService;

        public ScooterController(IScooterService scooterService)
        {
            _scooterService = scooterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var scooters = await _scooterService.ListAsync();
            var resources = scooters.Select(ScooterResourceAssembler.ToResource);
            return Ok(resources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var scooter = await _scooterService.GetByIdAsync(id);
            if (scooter == null) return NotFound();
            return Ok(ScooterResourceAssembler.ToResource(scooter));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _scooterService.DeleteAsync(id);
            return NoContent();
        }
    }
}
