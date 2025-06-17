using backendmovix.Suscriptions.Domain.Model.Aggregate;

namespace backendmovix.Suscriptions.Application.Internal.Service;

public interface ISuscriptionService
{
    Task<IEnumerable<Suscription>> ListAsync();
    Task<Suscription> CreateAsync(Suscription suscription);
}