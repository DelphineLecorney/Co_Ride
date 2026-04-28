namespace Shared.Messaging.SharedMessagingDomainEvents.BookingEvents
{
    public record BookingConfirmedEvent(
        Guid BookingId,
        Guid TripId,
        Guid PassengerId,
        DateTime ConfirmedAt
    );
}
