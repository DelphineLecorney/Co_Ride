namespace Shared.Contracts.DTOs.Trip
{
    /// <summary>
    /// Représente un trajet tel qu'exposé par l'API.
    /// </summary>
    /// <param name="Id">Identifiant unique du trajet.</param>
    /// <param name="DriverId">Identifiant du conducteur.</param>
    /// <param name="DriverName">Nom complet du conducteur.</param>
    /// <param name="From">Lieu de départ.</param>
    /// <param name="To">Lieu d'arrivée.</param>
    /// <param name="DepartureTime">Date et heure prévues du départ.</param>
    /// <param name="AvailableSeats">Nombre de sièges encore disponibles.</param>
    /// <param name="BookedSeats">Nombre de sièges déjà réservés.</param>
    /// <param name="PricePerSeat">Prix demandé par siège.</param>
    /// <param name="Status">Statut actuel du trajet (ex. : Open, Full, Cancelled).</param>
    /// <param name="Description">Description optionnelle du trajet.</param>
    /// <param name="CreatedAt">Date et heure de création du trajet.</param>
    public record TripDto(
        Guid Id,
        Guid DriverId,
        string DriverName,
        string From,
        string To,
        DateTime DepartureTime,
        int AvailableSeats,
        int BookedSeats,
        decimal PricePerSeat,
        string Status,
        string? Description,
        DateTime CreatedAt
    );
}
