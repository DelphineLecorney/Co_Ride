namespace Shared.Kernel.Domain.Events.Trips
{
    public record TripCancelledDomainEvent(
        Guid TripId,
        string Reason,
        DateTime CancelledAt
    ) : DomainEvent;
}
