using Marten;
using Trip.Application.Interfaces;
using Trip.Domain.ValueObjects;

namespace Trip.Infrastructure.Repositories
{
    public class DriverProfileRepository(IDocumentSession session)
        : IDriverProfileRepository
    {
        public async Task<DriverProfile?> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct = default)
        => await session
            .Query<DriverProfile>()
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);

        public async Task AddAsync(
            DriverProfile profile,
            CancellationToken ct = default)
        {
            session.Store(profile);
            await session.SaveChangesAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => session.SaveChangesAsync(ct);
    }
}
