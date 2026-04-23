namespace Trip.Application.Commands.CancelTrip;

public record CancelTripResponse(
    bool Success,
    string Message
);
