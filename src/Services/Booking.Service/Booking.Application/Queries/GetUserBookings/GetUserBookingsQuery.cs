using MediatR;

namespace Booking.Application.Queries.GetUserBookings
{
    public record GetUserBookingsQuery(
        Guid PassengerId,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<GetUserBookingsResult>;
}
