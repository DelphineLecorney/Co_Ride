namespace Trip.Application.Projections.ReadModels;

/// <summary>
/// Read Model pour la recherche et l'affichage des trips.
/// Optimisé pour les queries (dénormalisé).
/// </summary>
public class TripReadModel
{
    public Guid Id { get; set; }

    public Guid DriverId { get; set; }

    public string FromCity { get; set; } = string.Empty;
    public string ToCity { get; set; } = string.Empty;

    public DateTime DepartureTime { get; set; }
    public int AvailableSeats { get; set; }
    public int TotalSeats { get; set; }
    public decimal PricePerSeat { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }

    public bool IsActive => Status == "Active" && AvailableSeats > 0 && DepartureTime > DateTime.UtcNow;
}