using Microsoft.EntityFrameworkCore;
using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Infrastructure.Persistence
{
    public class ReviewDbContext : DbContext
    {
        public ReviewDbContext(DbContextOptions<ReviewDbContext> options)
            : base(options) { }

        public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ReviewDbContext).Assembly);
        }
    }
}
