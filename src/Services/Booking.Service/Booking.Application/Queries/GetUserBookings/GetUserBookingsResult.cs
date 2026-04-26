using Booking.Application.DTOs;

namespace Booking.Application.Queries.GetUserBookings
{
    public record GetUserBookingsResult
    {
        public List<BookingDto> Bookings { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
