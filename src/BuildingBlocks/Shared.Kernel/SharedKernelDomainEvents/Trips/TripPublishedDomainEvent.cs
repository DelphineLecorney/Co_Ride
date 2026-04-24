namespace Shared.Kernel.SharedKernelDomainEvents.Trips
{
    public record TripPublishedDomainEvent(
    Guid TripId,
    DateTime PublishedAt
) : DomainEvent;
}
