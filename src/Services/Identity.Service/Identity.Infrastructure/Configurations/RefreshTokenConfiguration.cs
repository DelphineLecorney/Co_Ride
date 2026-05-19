using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens_Token");

        builder.HasIndex(e => new { e.UserId, e.IsRevoked, e.ExpiresAt })
            .HasDatabaseName("IX_RefreshTokens_UserIdActive");

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.CreatedByIp)
            .HasMaxLength(45);

        builder.Property(e => e.DeviceInfo)
            .HasMaxLength(500);

        builder.Property(e => e.RevokedByIp)
            .HasMaxLength(45);

        builder.Property(e => e.RevokeReason)
            .HasMaxLength(200);

        builder.Property(e => e.ReplacedByToken)
            .HasMaxLength(500);
    }
}
