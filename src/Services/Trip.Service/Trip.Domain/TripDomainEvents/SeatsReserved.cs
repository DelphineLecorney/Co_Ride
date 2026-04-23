namespace Trip.Domain.TripDomainEvents;

public record SeatsReserved(
    Guid TripId,
    Guid PassengerId,
    int SeatsCount,
    int AvailableSeats,
    DateTime ReservedAt
);
