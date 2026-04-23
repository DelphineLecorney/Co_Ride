namespace Shared.Kernel.Domain.Events.Trips
{
    public record TripPublishedDomainEvent(
        Guid TripId,
        DateTime PublishedAt
    ) : DomainEvent;
}
