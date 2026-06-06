using MediatR;
using Review.Domain.Enums;
using Shared.Contracts.DTOs.Review;

namespace Review.Application.Commands.CreateReview
{
    public record CreateReviewCommand(
        Guid TripId,
        Guid ReviewerId,
        Guid RevieweeId,
        ReviewerType ReviewerType,
        int Rating,
        string? Comment
    ) : IRequest<ReviewDto>;
}
