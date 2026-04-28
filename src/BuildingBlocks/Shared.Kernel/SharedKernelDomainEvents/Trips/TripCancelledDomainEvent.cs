namespace Shared.Kernel.SharedKernelDomainEvents.Trips
{
    public record TripCancelledDomainEvent(
    Guid TripId,
    string Reason,
    DateTime CancelledAt
) : DomainEvent;
}
