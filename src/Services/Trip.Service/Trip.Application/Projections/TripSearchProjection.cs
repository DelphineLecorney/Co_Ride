using Marten.Events.Aggregation;
using Trip.Application.Projections.ReadModels;
using Trip.Domain.TripDomainEvents;


namespace Trip.Application.Projections;

/// <summary>
/// Projection Inline qui maintient à jour le TripReadModel à partir des événements.
/// Marten applique cette projection en temps réel (Inline) pour chaque événement.
/// </summary>
public class TripSearchProjection : SingleStreamProjection<TripReadModel, Guid>
{
    /// <summary>
    /// Crée le Read Model initial lors de la création d'un trip.
    /// </summary>
    public static TripReadModel Create(TripCreated @event)
    {
        return new TripReadModel
        {
            Id = @event.TripId,
            DriverId = @event.DriverId,

            FromCity = @event.FromCity,
            ToCity = @event.ToCity,

            DepartureTime = @event.DepartureTime,
            TotalSeats = @event.TotalSeats,
            AvailableSeats = @event.TotalSeats,
            PricePerSeat = @event.PricePerSeat,

            Status = "Active",
            CreatedAt = @event.CreatedAt
        };
    }

    /// <summary>
    /// Met à jour le Read Model quand des sièges sont réservés.
    /// </summary>
    public static void Apply(SeatsReserved @event, TripReadModel model)
    {
        model.AvailableSeats -= @event.SeatsCount;

        if (model.AvailableSeats <= 0)
        {
            model.Status = "Full";
        }
    }

    /// <summary>
    /// Met à jour le Read Model quand un trip est annulé.
    /// </summary>
    public static void Apply(TripCancelled @event, TripReadModel model)
    {
        model.Status = "Cancelled";
        model.CancelledAt = @event.CancelledAt;
        model.CancellationReason = @event.Reason;
    }
}