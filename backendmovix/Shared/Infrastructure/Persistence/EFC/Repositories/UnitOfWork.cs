using backendmovix.Shared.Domain.Repositories;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace backendmovix.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWord
{
    // inheritedDoc
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}