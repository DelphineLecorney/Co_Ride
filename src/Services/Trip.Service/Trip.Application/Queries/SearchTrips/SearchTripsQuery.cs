using MediatR;
using Trip.Application.Projections.ReadModels;

namespace Trip.Application.Queries.SearchTrips;

/// <summary>
/// Query pour rechercher des trips avec des critères.
/// Supporte la pagination et le filtrage.
/// </summary>
public record SearchTripsQuery : IRequest<SearchTripsResult>
{
    public string? FromCity { get; init; }
    public string? ToCity { get; init; }
    public DateTime? DepartureDate { get; init; }
    public int? MinSeats { get; init; }
    public decimal? MaxPrice { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public string SortBy { get; init; } = "DepartureTime";
    public bool Ascending { get; init; } = true;
}
