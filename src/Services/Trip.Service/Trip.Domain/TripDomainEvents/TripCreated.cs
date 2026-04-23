namespace Trip.Domain.TripDomainEvents;

public record TripCreated(
    Guid TripId,
    Guid DriverId,
    string FromCity,
    string ToCity,
    DateTime DepartureTime,
    int TotalSeats,
    decimal PricePerSeat,
    DateTime CreatedAt
);
