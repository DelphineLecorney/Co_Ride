using MediatR;

namespace Trip.Application.Commands.ReserveSeats;

/// <summary>
/// Commande pour réserver des sièges dans un trip.
/// Typiquement appelée par le Booking Service via un événement.
/// </summary>
public record ReserveSeatsCommand : IRequest<ReserveSeatsResponse>
{
    public Guid TripId { get; init; }
    public Guid BookingId { get; init; }
    public int SeatsCount { get; init; }
}
