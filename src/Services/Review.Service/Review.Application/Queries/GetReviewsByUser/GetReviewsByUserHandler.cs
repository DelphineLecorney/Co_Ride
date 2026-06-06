using AutoMapper;
using MediatR;
using Review.Application.Interfaces;
using Shared.Contracts.DTOs.Review;

namespace Review.Application.Queries.GetReviewsByUser
{
    public class GetReviewsByUserHandler(
        IReviewRepository repository,
        IMapper mapper
    ) : IRequestHandler<GetReviewsByUserQuery, List<ReviewDto>>
    {
        public async Task<List<ReviewDto>> Handle(
            GetReviewsByUserQuery query,
            CancellationToken ct)
        {
            var reviews = await repository.GetByRevieweeIdAsync(query.UserId, ct);
            return mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}
