using AutoMapper;
using MassTransit;
using MediatR;
using Review.Application.Interfaces;
using Shared.Contracts.DTOs.Review;
using ReviewEntity = Review.Domain.Entities.Review;


namespace Review.Application.Commands.CreateReview
{
    public class CreateReviewCommandHandler(
        IReviewRepository repository,
        IPublishEndpoint publishEndpoint,
        IMapper mapper
    ) : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        public async Task<ReviewDto> Handle(
            CreateReviewCommand command,
            CancellationToken cancellationToken)
        {

            var existing = await repository.GetByTripAndReviewerAsync(
                command.TripId, command.ReviewerId, cancellationToken);

            if (existing is not null)
            {
                throw new InvalidOperationException("Vous avez déjà évalué ce trajet.");
            }

            var review = ReviewEntity.Create(
                command.TripId,
                command.ReviewerId,
                command.RevieweeId,
                command.ReviewerType,
                command.Rating,
                command.Comment
            );

            await repository.AddAsync(review, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new ReviewCreatedEvent(
                review.RevieweeId,
                review.Rating,
                review.CreatedAt
            ), cancellationToken);

            return mapper.Map<ReviewDto>(review);
        }
    }
}
