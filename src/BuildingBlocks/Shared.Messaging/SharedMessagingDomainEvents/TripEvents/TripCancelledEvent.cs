namespace Shared.Messaging.SharedMessagingDomainEvents.TripEvents
{
    public record TripCancelledEvent(
        Guid TripId,
        string Reason,
        DateTime CancelledAt
    );
}
