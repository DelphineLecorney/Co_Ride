namespace Shared.Contracts.DTOs.Booking
{
    /// <summary>
    /// Requête permettant de créer une nouvelle réservation pour un trajet.
    /// </summary>
    /// <param name="TripId">Identifiant du trajet pour lequel la réservation est effectuée.</param>
    /// <param name="PassengerId">Identifiant du passager qui souhaite réserver.</param>
    /// <param name="SeatsRequested">Nombre de sièges demandés pour la réservation.</param>
    public record CreateBookingRequest(
        Guid TripId,
        Guid PassengerId,
        int SeatsRequested
    );
}
