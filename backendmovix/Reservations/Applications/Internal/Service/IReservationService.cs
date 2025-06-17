namespace backendmovix.Reservations.Applications.Internal.Service;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> ListAsync();
    Task<Reservation> CreateAsync(Reservation reservation);
}
