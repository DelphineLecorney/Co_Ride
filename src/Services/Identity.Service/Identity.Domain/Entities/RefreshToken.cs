using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Domain.Entities;

/// <summary>
/// Refresh Token pour renouveler l'Access Token
/// Un utilisateur peut avoir plusieurs RefreshTokens actifs (mobile, desktop, web...)
/// </summary>
[Table("RefreshTokens")]
public class RefreshToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();


    [Required]
    public Guid UserId { get; set; }


    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = string.Empty;

  
    [Required]
    public DateTime ExpiresAt { get; set; }

  
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(45)]
    public string? CreatedByIp { get; set; }

    [MaxLength(500)]
    public string? DeviceInfo { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }


    [MaxLength(45)]
    public string? RevokedByIp { get; set; }


    [MaxLength(200)]
    public string? RevokeReason { get; set; }


    [MaxLength(500)]
    public string? ReplacedByToken { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}