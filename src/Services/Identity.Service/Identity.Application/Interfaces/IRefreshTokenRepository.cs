using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

/// <summary>
/// Interface du repository RefreshToken
/// </summary>
public interface IRefreshTokenRepository
{
    Task<RefreshToken> SaveAsync(
        Guid userId,
        string token,
        DateTime expiresAt,
        string? ipAddress = null,
        string? deviceInfo = null);

    Task<RefreshToken?> GetActiveAsync(string token);

    Task<List<RefreshToken>> GetActiveTokensForUserAsync(Guid userId);

    Task RevokeAsync(
        string token,
        string? ipAddress = null,
        string reason = "Manual revocation",
        string? replacedByToken = null);

    Task RevokeAllForUserAsync(Guid userId, string reason = "Logout all devices");
    Task DeleteExpiredTokensAsync();
    Task<int> CountActiveTokensForUserAsync(Guid userId);
}
