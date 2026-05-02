namespace Shared.Messaging.SharedMessagingDomainEvents.BookingEvents
{
    public record BookingCancelledEvent(
        Guid BookingId,
        Guid PassengerId,
        string Reason,
        DateTime CancelledAt
    );
}
