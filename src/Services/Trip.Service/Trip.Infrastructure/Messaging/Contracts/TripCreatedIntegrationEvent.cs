namespace Trip.Infrastructure.Messaging.Contracts
{
    /// <summary>
    /// Événement publié quand un trip est créé.
    /// </summary>
    public record TripCreatedIntegrationEvent
    {
        public Guid TripId { get; init; }
        public Guid DriverId { get; init; }
        public string FromCity { get; init; } = string.Empty;
        public string ToCity { get; init; } = string.Empty;
        public DateTime DepartureTime { get; init; }
        public int TotalSeats { get; init; }
        public decimal PricePerSeat { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
