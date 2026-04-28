namespace Shared.Messaging.SharedMessagingDomainEvents.BookingEvents
{
    public record BookingCancelledEvent(
        Guid BookingId,
        string Reason
    );
}
