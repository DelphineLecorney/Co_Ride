namespace Booking.Application.DTOs
{
    public record BookingDto
    {
        public Guid Id { get; init; }
        public Guid TripId { get; init; }
        public Guid PassengerId { get; init; }
        public int SeatsCount { get; init; }
        public decimal TotalPrice { get; init; }
        public string Status { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public DateTime? CancelledAt { get; init; }
        public string? CancellationReason { get; init; }
    }
}
