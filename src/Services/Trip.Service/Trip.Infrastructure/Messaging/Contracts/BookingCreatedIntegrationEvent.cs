namespace Trip.Infrastructure.Messaging.Contracts
{
    /// <summary>
    /// Événement reçu du Booking Service quand une réservation est créée.
    /// Le Trip Service doit réserver les sièges.
    /// </summary>
    public record BookingCreatedIntegrationEvent
    {
        public Guid BookingId { get; init; }
        public Guid TripId { get; init; }
        public Guid PassengerId { get; init; }
        public int SeatsCount { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
