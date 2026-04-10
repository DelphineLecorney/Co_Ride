namespace Shared.Contracts.DTOs.Common
{
    /// <summary>
    /// Représente une erreur standardisée renvoyée par l'API.
    /// </summary>
    /// <param name="Code">Code d’erreur technique ou fonctionnel.</param>
    /// <param name="Message">Message lisible décrivant l’erreur.</param>
    /// <param name="Field">Champ concerné (optionnel), utile pour les erreurs de validation.</param>
    public record ApiError(
        string Code,
        string Message,
        string? Field = null
    );
}
