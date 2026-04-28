using MediatR;

namespace Booking.Application.Commands.CancelBooking
{
    public record CancelBookingCommand(
    Guid BookingId,
    Guid PassengerId,
    string Reason
) : IRequest<CancelBookingResult>;
}
