// backendmovix/Scooter/Application/Internal/Service/IScooterService.cs
using backendmovix.Scooter.Interfaces.REST.Resources;

namespace backendmovix.Scooter.Application.Internal.Service;

public interface IScooterService
{
    Task<IEnumerable<Domain.Model.Aggregate.Scooter>> ListAsync();
    Task<Domain.Model.Aggregate.Scooter> GetByIdAsync(int id);
    Task<Domain.Model.Aggregate.Scooter> CreateAsync(CreateScooterResource resource);
    Task<Domain.Model.Aggregate.Scooter?> UpdateAsync(int id, CreateScooterResource resource); // <-- Añadido
    Task DeleteAsync(int id);
}