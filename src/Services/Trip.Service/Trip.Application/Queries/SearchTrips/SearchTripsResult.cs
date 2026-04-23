using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trip.Application.Projections.ReadModels;

namespace Trip.Application.Queries.SearchTrips
{
    /// <summary>
    /// Résultat de la recherche avec pagination.
    /// </summary>
    public record SearchTripsResult
    {
        public List<TripReadModel> Trips { get; init; } = new();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
