namespace Trip.Infrastructure.Messaging.Contracts
{
    /// <summary>
    /// Événement publié quand un trip est annulé.
    /// Le Booking Service doit annuler toutes les réservations associées.
    /// </summary>
    public record TripCancelledIntegrationEvent
    {
        public Guid TripId { get; init; }
        public string Reason { get; init; } = string.Empty;
        public DateTime CancelledAt { get; init; }
    }
}
