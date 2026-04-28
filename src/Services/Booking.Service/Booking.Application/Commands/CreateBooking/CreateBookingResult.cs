namespace Booking.Application.Commands.CreateBooking
{
    public record CreateBookingResult(
        Guid BookingId,
        bool Success,
        string Message
    );
}
