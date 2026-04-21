namespace Trip.Infrastructure.Messaging.Contracts
{
    /// <summary>
    /// Événement publié quand des sièges sont réservés.
    /// Permet de notifier les autres services de la mise à jour.
    /// </summary>
    public record SeatsReservedIntegrationEvent
    {
        public Guid TripId { get; init; }
        public Guid BookingId { get; init; }
        public int SeatsReserved { get; init; }
        public int AvailableSeats { get; init; }
        public DateTime ReservedAt { get; init; }
    }
}
