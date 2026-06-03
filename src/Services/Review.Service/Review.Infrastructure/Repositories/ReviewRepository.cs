using Microsoft.EntityFrameworkCore;
using Review.Application.Interfaces;
using Review.Infrastructure.Persistence;
using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Infrastructure.Repositories
{
    public class ReviewRepository(ReviewDbContext context) : IReviewRepository
    {
        Task<ReviewEntity?> IReviewRepository.GetByTripAndReviewerAsync(Guid tripId, Guid reviewerId, CancellationToken cancellationToken)
        {
            return context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TripId == tripId
                               && x.ReviewerId == reviewerId, cancellationToken);
        }

        async Task<IReadOnlyList<ReviewEntity>> IReviewRepository.GetByRevieweeIdAsync(Guid revieweeId, CancellationToken cancellationToken)
        {
            return await context.Reviews
                .AsNoTracking()
                .Where(x => x.RevieweeId == revieweeId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        async Task<IReadOnlyList<ReviewEntity>> IReviewRepository.GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken)
        {
            return await context.Reviews
                .AsNoTracking()
                .Where(x => x.TripId == tripId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ReviewEntity review, CancellationToken cancellationToken = default)
        {
            await context.Reviews.AddAsync(review, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
