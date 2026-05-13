namespace Shared.Contracts.DTOs.Trip
{
    public record SearchTripsResponse
    {
        public List<TripDto> Trips { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
