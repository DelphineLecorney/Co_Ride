
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Infrastructure.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(1000);

            builder.Property(x => x.ReviewerType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Un reviewer ne peut évaluer qu'une fois par trajet
            builder.HasIndex(x => new { x.TripId, x.ReviewerId })
                .IsUnique();
        }
    }
}
