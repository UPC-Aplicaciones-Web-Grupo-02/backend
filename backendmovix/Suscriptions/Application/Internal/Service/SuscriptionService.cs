using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Suscriptions.Application.Internal.Service;

public class SuscriptionService : ISuscriptionService
{
    private readonly AppDbContext _context;

    public SuscriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Suscription>> ListAsync()
    {
        return await _context.Suscriptions.ToListAsync();
    }

    public async Task<Suscription> CreateAsync(Suscription suscription)
    {
        _context.Suscriptions.Add(suscription);
        await _context.SaveChangesAsync();
        return suscription;
    }
}