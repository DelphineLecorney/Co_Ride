namespace Shared.Contracts.DTOs.Booking
{
    /// <summary>
    /// Représente une réservation effectuée pour un trajet.
    /// </summary>
    /// <param name="Id">Identifiant unique de la réservation.</param>
    /// <param name="TripId">Identifiant du trajet réservé.</param>
    /// <param name="PassengerId">Identifiant du passager ayant effectué la réservation.</param>
    /// <param name="SeatsBooked">Nombre de sièges réservés.</param>
    /// <param name="TotalPrice">Prix total payé pour la réservation.</param>
    /// <param name="Status">Statut actuel de la réservation (ex. : Pending, Confirmed, Cancelled).</param>
    /// <param name="CreatedAt">Date et heure de création de la réservation.</param>
    public record BookingDto(
        Guid Id,
        Guid TripId,
        Guid PassengerId,
        int SeatsBooked,
        decimal TotalPrice,
        string Status,
        DateTime CreatedAt
    );
}
