namespace Trip.Domain.TripDomainEvents;
public record TripCancelled(
    Guid TripId,
    string Reason,
    DateTime CancelledAt
);
