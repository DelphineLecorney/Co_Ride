namespace Shared.Messaging.SharedMessagingDomainEvents.BookingEvents
{
    public record BookingCreatedEvent(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        int SeatsBooked,
        decimal TotalPrice,
        DateTime CreatedAt
    );
}
