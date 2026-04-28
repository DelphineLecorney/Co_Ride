using Booking.Domain.Entities;

namespace Booking.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<BookingEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<BookingEntity>> GetByPassengerIdAsync(Guid passengerId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> CountByPassengerIdAsync(Guid passengerId, CancellationToken cancellationToken = default);
        Task AddAsync(BookingEntity booking, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
