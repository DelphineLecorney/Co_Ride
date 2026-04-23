namespace Trip.Infrastructure.Messaging.Contracts
{
    /// <summary>
    /// Événement reçu du Booking Service quand une réservation est annulée.
    /// Le Trip Service doit libérer les sièges.
    /// </summary>
    public record BookingCancelledIntegrationEvent
    {
        public Guid BookingId { get; init; }
        public Guid TripId { get; init; }
        public int SeatsCount { get; init; }
        public DateTime CancelledAt { get; init; }
    }
}
