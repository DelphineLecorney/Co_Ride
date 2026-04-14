
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using System.Security.Claims;

namespace Identity.Application.Interfaces
{
    /// <summary>
    /// Interface pour le service de gestion des tokens JWT, incluant la génération 
    /// de tokens d'accès et de rafraîchissement, ainsi que la validation des tokens expirés.
    /// </summary>
    public interface IJwtTokenService
    {
        TokenDto GenerateToken(ApplicationUser user);
        string GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
