using Shared.Contracts.DTOs.Trip;

namespace Booking.Application.Interfaces
{
    public interface ITripServiceClient
    {
        Task<TripDto?> GetTripAsync(Guid tripId, CancellationToken cancellationToken = default);
        Task<bool> ReserveSeatsAsync(Guid tripId, Guid bookingId, int seatsCount, CancellationToken cancellationToken = default);
    }
}
