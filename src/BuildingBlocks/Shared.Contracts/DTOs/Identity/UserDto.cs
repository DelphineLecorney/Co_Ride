namespace Shared.Contracts.DTOs.Identity
{
    /// <summary>
    /// Représente un utilisateur tel qu'exposé par l'API.
    /// </summary>
    /// <param name="Id">Identifiant unique de l'utilisateur.</param>
    /// <param name="Email">Adresse e‑mail de l'utilisateur.</param>
    /// <param name="FirstName">Prénom de l'utilisateur.</param>
    /// <param name="LastName">Nom de famille de l'utilisateur.</param>
    /// <param name="PhoneNumber">Numéro de téléphone de l'utilisateur (optionnel).</param>
    /// <param name="CreatedAt">Date et heure de création du compte.</param>
    public record UserDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        List<string> Roles,
        bool IsEmailVerified,
        decimal ReputationScore,
        int ReviewCount,
        DateTime CreatedAt
    )
    {
        public string FullName => $"{FirstName} {LastName}";
        public string Initials => $"{(FirstName.Length > 0 ? FirstName[0] : ' ')}{(LastName.Length > 0 ? LastName[0] : ' ')}";
    }
}
