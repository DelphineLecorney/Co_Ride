namespace Shared.Kernel.Domain.Events.Trips
{
    public record TripCreatedDomainEvent(
        Guid TripId,
        Guid DriverId,
        string FromCity,
        string ToCity,
        DateTime DepartureTime,
        int AvailableSeats,
        decimal PricePerSeat
    ) : DomainEvent;
}
