using Booking.Application.DTOs;
using Booking.Application.Interfaces;
using MediatR;

namespace Booking.Application.Queries.GetUserBookings
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, GetUserBookingsResult>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetUserBookingsQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<GetUserBookingsResult> Handle(
            GetUserBookingsQuery request,
            CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetByPassengerIdAsync(
                request.PassengerId,
                request.Page,
                request.PageSize,
                cancellationToken
            );

            var totalCount = await _bookingRepository.CountByPassengerIdAsync(
                request.PassengerId,
                cancellationToken
            );

            var bookingsDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                TripId = b.TripId,
                PassengerId = b.PassengerId,
                SeatsCount = b.SeatsCount,
                TotalPrice = b.TotalPrice,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                CancelledAt = b.CancelledAt,
                CancellationReason = b.CancellationReason
            }).ToList();

            return new GetUserBookingsResult
            {
                Bookings = bookingsDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
