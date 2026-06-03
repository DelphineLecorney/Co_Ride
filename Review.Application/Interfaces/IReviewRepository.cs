using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<ReviewEntity?> GetByTripAndReviewerAsync(Guid tripId, Guid reviewerId, CancellationToken ct = default);
        Task<List<ReviewEntity>> GetByRevieweeIdAsync(Guid revieweeId, CancellationToken ct = default);
        Task<List<ReviewEntity>> GetByTripIdAsync(Guid tripId, CancellationToken ct = default);
        Task AddAsync(ReviewEntity review, CancellationToken ct = default);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}

