using backend01.Scooter.Interfaces.REST.Resources;
using backend01.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace backend01.Scooter.Application.Internal.Service;

public class ScooterService : IScooterService
{
    private readonly AppDbContext _context;

    public ScooterService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Model.Aggregate.Scooter>> ListAsync()
    {
        return await _context.Set<Domain.Model.Aggregate.Scooter>()
            .Include(s => s.Brand)
            .Include(s => s.Model)
            .Include(s => s.District)
            .ToListAsync();
    }

    public async Task<Domain.Model.Aggregate.Scooter> GetByIdAsync(int id)
    {
        return await _context.Set<Domain.Model.Aggregate.Scooter>()
            .Include(s => s.Brand)
            .Include(s => s.Model)
            .Include(s => s.District)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Domain.Model.Aggregate.Scooter> CreateAsync(CreateScooterResource resource)
    {
        var scooter = new Domain.Model.Aggregate.Scooter()
        {
            Name = resource.Name,
            Description = resource.Description,
            Image = resource.Image,
            BrandId = resource.BrandId,
            ModelId = resource.ModelId,
            DistrictId = resource.DistrictId
        };
        _context.Add(scooter);
        await _context.SaveChangesAsync();
        return scooter;
    }

    public async Task DeleteAsync(int id)
    {
        var scooter = await _context.Set<Domain.Model.Aggregate.Scooter>().FindAsync(id);
        if (scooter != null)
        {
            _context.Remove(scooter);
            await _context.SaveChangesAsync();
        }
    }
}