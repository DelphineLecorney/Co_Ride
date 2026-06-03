using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<ReviewEntity?> GetByTripAndReviewerAsync(Guid tripId, Guid reviewerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ReviewEntity>> GetByRevieweeIdAsync(Guid revieweeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ReviewEntity>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default);
        Task AddAsync(ReviewEntity review, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
