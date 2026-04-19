namespace Trip.Domain.Events;

public record TripCancelled(
    Guid TripId,
    string Reason,
    DateTime CancelledAt
);
