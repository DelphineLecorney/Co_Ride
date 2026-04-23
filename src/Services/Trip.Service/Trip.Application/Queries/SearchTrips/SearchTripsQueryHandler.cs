using Marten;
using Marten.Pagination;
using MediatR;
using Trip.Application.Projections.ReadModels;

namespace Trip.Application.Queries.SearchTrips;

/// <summary>
/// Handler pour rechercher des trips avec filtres et pagination.
/// Utilise LINQ to Marten pour construire des queries efficaces.
/// </summary>
public class SearchTripsQueryHandler : IRequestHandler<SearchTripsQuery, SearchTripsResult>
{
    private readonly IQuerySession _querySession;

    public SearchTripsQueryHandler(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public async Task<SearchTripsResult> Handle(SearchTripsQuery request, CancellationToken cancellationToken)
    {

        var query = _querySession.Query<TripReadModel>()
            .Where(t => t.Status == "Active");

        if (!string.IsNullOrWhiteSpace(request.FromCity))
        {
            query = query.Where(t => t.FromCity.Contains(request.FromCity, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.ToCity))
        {
            query = query.Where(t => t.ToCity.Contains(request.ToCity, StringComparison.OrdinalIgnoreCase));
        }

        if (request.DepartureDate.HasValue)
        {
            var startOfDay = request.DepartureDate.Value.Date;
            var endOfDay = startOfDay.AddDays(1);
            query = query.Where(t => t.DepartureTime >= startOfDay && t.DepartureTime < endOfDay);
        }

        if (request.MinSeats.HasValue)
        {
            query = query.Where(t => t.AvailableSeats >= request.MinSeats.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(t => t.PricePerSeat <= request.MaxPrice.Value);
        }

        query = request.SortBy.ToLower() switch
        {
            "price" => request.Ascending
                ? query.OrderBy(t => t.PricePerSeat)
                : query.OrderByDescending(t => t.PricePerSeat),
            "departuretime" => request.Ascending
                ? query.OrderBy(t => t.DepartureTime)
                : query.OrderByDescending(t => t.DepartureTime),
            _ => query.OrderBy(t => t.DepartureTime)
        };

        var pagedResult = await query.ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        return new SearchTripsResult
        {
            Trips = pagedResult.ToList(),
            TotalCount = (int)pagedResult.TotalItemCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}