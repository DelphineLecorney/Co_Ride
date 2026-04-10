namespace Shared.Messaging.Events
{
    /// <summary>
    /// Cette classe définit les événements liés aux réservations (bookings) dans le système de covoiturage.
    /// Elle contient des records pour les événements de création, de confirmation et d'annulation de réservation,
    /// permettant ainsi de structurer et de centraliser la gestion des événements liés aux réservations.
    /// </summary>
    public class BookingEvents
    {
        public record BookingCreatedEvent(
            Guid BookingId,
            Guid TripId,
            Guid PassengerId,
            int SeatsBooked,
            decimal TotalPrice
        );

        public record BookingConfirmedEvent(
            Guid BookingId,
            Guid TripId,
            Guid PassengerId,
            DateTime ConfirmedAt
        );

        public record BookingCancelledEvent(
            Guid BookingId,
            string Reason
        );
    }
}
