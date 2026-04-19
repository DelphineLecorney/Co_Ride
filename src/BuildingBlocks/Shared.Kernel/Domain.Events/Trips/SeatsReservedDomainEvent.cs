namespace Shared.Kernel.Domain.Events.Trips;

public record SeatsReservedDomainEvent(
    Guid TripId,
    Guid PassengerId,
    int SeatsCount,
    DateTime ReservedAt
) : DomainEvent;