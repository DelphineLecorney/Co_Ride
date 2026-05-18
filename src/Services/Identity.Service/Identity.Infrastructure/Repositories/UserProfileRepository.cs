using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    public class UserProfileRepository(IdentityDbContext context) : IUserProfileRepository
    {
        public Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => context.UserProfiles
                      .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken = default)
            => await context.UserProfiles.AddAsync(profile, cancellationToken);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => context.SaveChangesAsync(cancellationToken);
    }
}
