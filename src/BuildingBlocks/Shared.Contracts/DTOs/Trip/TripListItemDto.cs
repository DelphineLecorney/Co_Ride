namespace Shared.Contracts.DTOs.Trip
{
    /// <summary>
    /// Élément simplifié représentant un trajet dans une liste.
    /// </summary>
    /// <param name="Id">Identifiant unique du trajet.</param>
    /// <param name="DriverName">Nom du conducteur.</param>
    /// <param name="From">Lieu de départ.</param>
    /// <param name="To">Lieu d'arrivée.</param>
    /// <param name="DepartureTime">Date et heure prévues du départ.</param>
    /// <param name="AvailableSeats">Nombre de sièges encore disponibles.</param>
    /// <param name="PricePerSeat">Prix demandé par siège.</param>
    public record TripListItemDto(
        Guid Id,
        string DriverName,
        string From,
        string To,
        DateTime DepartureTime,
        int AvailableSeats,
        decimal PricePerSeat
    );
}
