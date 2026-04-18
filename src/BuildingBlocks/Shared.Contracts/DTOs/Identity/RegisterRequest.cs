using System.ComponentModel.DataAnnotations;

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
    public record RegisterRequest
    {
        [Required(ErrorMessage = "L'adresse e-mail est requise")]
        [EmailAddress(ErrorMessage = "Format d'e-mail invalide")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit comporter au moins 6 caractères")]
        public string Password { get; init; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; init; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        public string FirstName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Le nom de famille est requis")]
        public string LastName { get; init; } = string.Empty;

        [Phone(ErrorMessage = "Format de numéro de téléphone invalide")]
        public string? PhoneNumber { get; init; }

        public bool RegisterAsDriver { get; init; } = false;
    }
}
