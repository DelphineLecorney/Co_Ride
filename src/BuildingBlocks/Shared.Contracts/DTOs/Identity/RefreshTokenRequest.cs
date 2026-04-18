using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts.DTOs.Identity
{
    /// <summary>
    /// Requête de refresh token
    /// </summary>
    public record RefreshTokenRequest
    {
        [Required(ErrorMessage = "Un jeton d'actualisation est requis")]
        public string RefreshToken { get; init; } = string.Empty;
    }
}
