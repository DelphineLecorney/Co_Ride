namespace Shared.Contracts.DTOs.Identity
{
    /// <summary>
    /// Réponse renvoyée après une authentification réussie.
    /// </summary>
    /// <param name="AccessToken">Jeton d'accès JWT permettant d'accéder aux ressources protégées.</param>
    /// <param name="RefreshToken">Jeton permettant d'obtenir un nouvel access token sans se reconnecter.</param>
    /// <param name="ExpiresAt">Date et heure d'expiration du jeton d'accès.</param>
    /// <param name="User">Informations sur l'utilisateur authentifié.</param>
    public record AuthResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserDto User
    );
}
