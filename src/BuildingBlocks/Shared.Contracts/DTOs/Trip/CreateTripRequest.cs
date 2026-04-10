namespace Shared.Contracts.DTOs.Trip
{
    /// <summary>
    /// Requête permettant de créer un nouveau trajet.
    /// </summary>
    /// <param name="DriverId">Identifiant du conducteur qui propose le trajet.</param>
    /// <param name="From">Lieu de départ du trajet.</param>
    /// <param name="To">Lieu d'arrivée du trajet.</param>
    /// <param name="DepartureTime">Date et heure de départ prévues.</param>
    /// <param name="AvailableSeats">Nombre de sièges disponibles pour les passagers.</param>
    /// <param name="PricePerSeat">Prix demandé par siège.</param>
    /// <param name="Description">Description optionnelle du trajet (infos pratiques, remarques, etc.).</param>
    public record CreateTripRequest(
        Guid DriverId,
        string From,
        string To,
        DateTime DepartureTime,
        int AvailableSeats,
        decimal PricePerSeat,
        string? Description = null
    );
}
