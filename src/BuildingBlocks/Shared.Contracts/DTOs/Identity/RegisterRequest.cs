namespace Shared.Contracts.DTOs.Identity
{
    /// <summary>
    /// Requête permettant d'enregistrer un nouvel utilisateur.
    /// </summary>
    /// <param name="Email">Adresse e‑mail utilisée pour créer le compte.</param>
    /// <param name="Password">Mot de passe choisi par l'utilisateur.</param>
    /// <param name="FirstName">Prénom de l'utilisateur.</param>
    /// <param name="LastName">Nom de famille de l'utilisateur.</param>
    /// <param name="PhoneNumber">Numéro de téléphone optionnel de l'utilisateur.</param>
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? PhoneNumber = null
    );
}
