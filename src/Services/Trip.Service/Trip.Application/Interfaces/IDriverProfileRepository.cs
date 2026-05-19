using Trip.Domain.ValueObjects;

namespace Trip.Application.Interfaces
{
    public interface IDriverProfileRepository
    {
        Task<DriverProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(DriverProfile profile, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
