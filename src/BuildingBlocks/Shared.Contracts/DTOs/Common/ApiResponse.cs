namespace Shared.Contracts.DTOs.Common
{
    /// <summary>
    /// Modèle générique pour encapsuler toute réponse API.
    /// </summary>
    /// <typeparam name="T">Type de la donnée renvoyée en cas de succès.</typeparam>
    /// <param name="Success">Indique si l’opération a réussi.</param>
    /// <param name="Data">Données renvoyées (null en cas d’échec).</param>
    /// <param name="Message">Message informatif ou de confirmation.</param>
    /// <param name="Errors">Liste de messages d’erreurs éventuels.</param>
    public record ApiResponse<T>(
        bool Success,
        T? Data,
        string? Message = null,
        IEnumerable<string>? Errors = null
    );
}
