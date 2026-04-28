using Booking.Application.DTOs;
using Booking.Application.Interfaces;
using MediatR;

namespace Booking.Application.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto?>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
