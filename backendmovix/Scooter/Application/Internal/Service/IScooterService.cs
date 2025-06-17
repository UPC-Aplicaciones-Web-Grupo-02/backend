using backendmovix.Scooter.Interfaces.REST.Resources;

namespace backendmovix.Scooter.Application.Internal.Service;

public interface IScooterService
{
    Task<IEnumerable<Domain.Model.Aggregate.Scooter>> ListAsync();
    Task<Domain.Model.Aggregate.Scooter> GetByIdAsync(int id);
    Task<Domain.Model.Aggregate.Scooter> CreateAsync(CreateScooterResource resource);
    Task DeleteAsync(int id);
}