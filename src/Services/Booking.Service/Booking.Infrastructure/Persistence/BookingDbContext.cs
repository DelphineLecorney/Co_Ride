using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookingEntity>(entity =>
            {
                entity.ToTable("Bookings");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TripId).IsRequired();
                entity.Property(e => e.PassengerId).IsRequired();
                entity.Property(e => e.SeatsCount).IsRequired();
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired(false);
                entity.Property(e => e.CancelledAt).IsRequired(false);
                entity.Property(e => e.CancellationReason).HasMaxLength(500).IsRequired(false);

                entity.HasIndex(e => e.PassengerId);
                entity.HasIndex(e => e.TripId);
                entity.HasIndex(e => e.Status);
            });
        }
    }
}
