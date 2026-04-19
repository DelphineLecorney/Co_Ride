using MediatR;

namespace Trip.Application.Commands.CreateTrip;

public record CreateTripCommand(
    Guid DriverId,
    string FromCity,
    string ToCity,
    DateTime DepartureTime,
    int TotalSeats,
    decimal PricePerSeat
) : IRequest<Guid>;