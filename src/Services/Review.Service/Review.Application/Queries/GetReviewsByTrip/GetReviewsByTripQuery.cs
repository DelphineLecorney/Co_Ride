using MediatR;
using Shared.Contracts.DTOs.Review;

namespace Review.Application.Queries.GetReviewsByTrip
{
    public record GetReviewsByTripQuery(Guid TripId)
        : IRequest<List<ReviewDto>>;
}
