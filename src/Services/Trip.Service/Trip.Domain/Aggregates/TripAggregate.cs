using Shared.Kernel.Domain.Events.Trips;
using Shared.Kernel.Entities;
using Trip.Domain.Enums;
using Trip.Domain.TripDomainEvents;
using Trip.Domain.ValueObjects;

namespace Trip.Domain.Aggregates;

public class TripAggregate : AggregateRoot
{
    public Guid DriverId { get; private set; }
    public Address From { get; private set; } = default!;
    public Address To { get; private set; } = default!;
    public DateTime DepartureTime { get; private set; }
    public int TotalSeats { get; private set; }
    public int AvailableSeats { get; private set; }
    public Price PricePerSeat { get; private set; } = default!;
    public TripStatus Status { get; private set; }
    public string? CancellationReason { get; private set; }

    private TripAggregate() { }

    public TripAggregate(IEnumerable<object> events)
    {
        foreach (var e in events)
        {
            When(e);
        }
    }

    public static TripAggregate Create(
        Guid driverId,
        Address from,
        Address to,
        DateTime departureTime,
        int totalSeats,
        Price pricePerSeat)
    {
        var trip = new TripAggregate();

        var @event = new TripCreated(
            Guid.NewGuid(),
            driverId,
            from.City,
            to.City,
            departureTime,
            totalSeats,
            pricePerSeat.Amount,
            DateTime.UtcNow
        );

        trip.Apply(@event);

        trip.AddDomainEvent(new TripCreatedDomainEvent(
            @event.TripId,
            driverId,
            from.City,
            to.City,
            departureTime,
            totalSeats,
            pricePerSeat.Amount
        ));

        return trip;
    }

    public void ReserveSeats(Guid passengerId, int seatsCount)
    {
        if (Status != TripStatus.Published)
            throw new InvalidOperationException("Le trajet n'est pas disponible");

        if (seatsCount > AvailableSeats)
            throw new InvalidOperationException("Pas assez de places disponibles");

        AvailableSeats -= seatsCount;

        AddDomainEvent(new SeatsReservedDomainEvent(
            Id,
            passengerId,
            seatsCount,
            DateTime.UtcNow
        ));

        if (AvailableSeats == 0)
        {
            Status = TripStatus.Full;
        }
    }

    public void Cancel(string reason)
    {
        if (Status == TripStatus.Cancelled)
            throw new InvalidOperationException("Déjà annulé");

        Status = TripStatus.Cancelled;
        CancellationReason = reason;

        AddDomainEvent(new TripCancelledDomainEvent(
            Id,
            reason,
            DateTime.UtcNow
        ));
    }

    private void When(object @event)
    {
        switch (@event)
        {
            case TripCreated e:
                Apply(e);
                break;

            case SeatsReserved e:
                Apply(e);
                break;

            case TripCancelled e:
                Apply(e);
                break;
        }
    }

    public void Apply(TripCreated e)
    {
        Id = e.TripId;
        DriverId = e.DriverId;
        From = Address.Create(e.FromCity);
        To = Address.Create(e.ToCity);
        DepartureTime = e.DepartureTime;
        TotalSeats = e.TotalSeats;
        AvailableSeats = e.TotalSeats;
        PricePerSeat = Price.Create(e.PricePerSeat);
        Status = TripStatus.Published;
    }

    public void Apply(SeatsReserved e)
        {
            AvailableSeats -= e.SeatsCount;
            if (AvailableSeats == 0)
            {
                Status = TripStatus.Full;
            }
    }

    public void Apply(TripCancelled e)
    {
        Status = TripStatus.Cancelled;
        CancellationReason = e.Reason;
    }
}