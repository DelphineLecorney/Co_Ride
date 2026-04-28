using Booking.Application.DTOs;
using MediatR;


namespace Booking.Application.Queries.GetBookingById
{
    public record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingDto?>;

}
