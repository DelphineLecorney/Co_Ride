namespace Shared.Contracts.DTOs.Identity
{
    /// <summary>
    /// Requête utilisée pour authentifier un utilisateur.
    /// </summary>
    /// <param name="Email">Adresse e‑mail de l'utilisateur.</param>
    /// <param name="Password">Mot de passe associé au compte.</param>
    public record LoginRequest(
        string Email,
        string Password
    );
}
