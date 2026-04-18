using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Repositories;

/// <summary>
/// Repository pour gérer les RefreshTokens
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _context;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(
        IdentityDbContext context,
        ILogger<RefreshTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RefreshToken> SaveAsync(
        Guid userId,
        string token,
        DateTime expiresAt,
        string? ipAddress = null,
        string? deviceInfo = null)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            DeviceInfo = deviceInfo,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Le RefreshToken a été créé pour l'utilisateur {UserId} depuis l'IP {IP}",
            userId, ipAddress ?? "Inconnue"
        );

        return refreshToken;
    }

    public async Task<RefreshToken?> GetActiveAsync(string token)
    {
        return await _context.RefreshTokens
            .Where(t => t.Token == token && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RefreshToken>> GetActiveTokensForUserAsync(Guid userId)
    {
        return await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task RevokeAsync(
        string token,
        string? ipAddress = null,
        string reason = "Révocation manuelle",
        string? replacedByToken = null)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (refreshToken == null)
        {
            _logger.LogWarning("Tentative de révocation d'un token inexistant");
            return;
        }

        if (refreshToken.IsRevoked)
        {
            _logger.LogWarning("Tentative de révocation d'un token déjà révoqué");
            return;
        }

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.RevokeReason = reason;
        refreshToken.ReplacedByToken = replacedByToken;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "RefreshToken révoqué pour l'utilisateur {UserId}. Raison: {Reason}",
            refreshToken.UserId, reason
        );
    }

    public async Task RevokeAllForUserAsync(Guid userId, string reason = "Déconnexion de tous les appareils")
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            token.RevokeReason = reason;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Tous les RefreshTokens ont été révoqués pour l'utilisateur {UserId}. Nombre: {Count}. Raison: {Reason}",
            userId, tokens.Count, reason
        );
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _context.RefreshTokens
            .Where(t => t.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync();

        _context.RefreshTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Suppression de {Count} RefreshTokens expirés",
            expiredTokens.Count
        );
    }

    public async Task<int> CountActiveTokensForUserAsync(Guid userId)
    {
        return await _context.RefreshTokens
            .CountAsync(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow);
    }
}