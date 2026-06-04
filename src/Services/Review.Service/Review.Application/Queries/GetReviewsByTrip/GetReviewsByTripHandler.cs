using AutoMapper;
using MediatR;
using Review.Application.Interfaces;
using Shared.Contracts.DTOs.Review;

namespace Review.Application.Queries.GetReviewsByTrip
{
    public class GetReviewsByTripHandler(
        IReviewRepository repository,
        IMapper mapper
    ) : IRequestHandler<GetReviewsByTripQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(
            GetReviewsByTripQuery query,
            CancellationToken ct)
        {
            var reviews = await repository.GetByTripIdAsync(query.TripId, ct);
            return mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}
