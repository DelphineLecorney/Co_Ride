
using MediatR;
using Shared.Contracts.DTOs.Review;

namespace Review.Application.Queries.GetReviewsByUser
{
    public record GetReviewsByUserQuery(Guid UserId)
        : IRequest<List<ReviewDto>>;
}