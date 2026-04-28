using MediatR;

namespace Booking.Application.Commands.CreateBooking
{
    public record CreateBookingCommand(
        Guid TripId,
        Guid PassengerId,
        int SeatsCount
    ) : IRequest<CreateBookingResult>;
}
