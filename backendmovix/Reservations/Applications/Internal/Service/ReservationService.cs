using backendmovix.Reservations.Domain.Model.Aggregate;

namespace backendmovix.Reservations.Applications.Internal.Service;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> ListAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }
}
