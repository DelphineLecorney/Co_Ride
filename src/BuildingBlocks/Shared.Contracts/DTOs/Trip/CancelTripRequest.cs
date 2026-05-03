namespace Shared.Contracts.DTOs.Trip
{
    public record CancelTripRequest
    {
        public Guid TripId { get; init; }
        public Guid DriverId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
