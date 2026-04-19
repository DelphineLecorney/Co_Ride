namespace Trip.Domain.Events;

public record SeatsReserved(
    Guid TripId,
    Guid PassengerId,
    int SeatsCount,
    DateTime ReservedAt
);
