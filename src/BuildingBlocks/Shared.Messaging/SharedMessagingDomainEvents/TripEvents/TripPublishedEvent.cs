namespace Shared.Messaging.SharedMessagingDomainEvents.TripEvents
{
    public record TripPublishedEvent(
        Guid TripId,
        DateTime PublishedAt
    );
}
