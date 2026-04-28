namespace Shared.Messaging.SharedMessagingDomainEvents.TripEvents
{
    public record TripCreatedEvent(
    Guid TripId,
    Guid DriverId,
    string FromCity,
    string ToCity,
    DateTime DepartureTime,
    int AvailableSeats,
    decimal PricePerSeat
);
}
