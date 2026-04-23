namespace Trip.Application.Commands.ReserveSeats
{
    public record ReserveSeatsResponse(
        bool Success,
        string Message,
        int AvailableSeats
        );
}