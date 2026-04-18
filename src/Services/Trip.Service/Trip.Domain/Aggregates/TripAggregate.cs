using Shared.Kernel.Domain.Events.Trips;
using Shared.Kernel.Entities;
using Trip.Domain.Enums;

namespace Trip.Domain.Aggregates
{
    public class TripAggregate : AggregateRoot
    {
        public string FromCity { get; private set; } = default!;
        public string ToCity { get; private set; } = default!;
        public int AvailableSeats { get; private set; }
        public TripStatus Status { get; private set; }

        private TripAggregate() { }

        public static TripAggregate Create(
            Guid driverId,
            string fromCity,
            string toCity,
            DateTime departureTime,
            int availableSeats,
            decimal pricePerSeat)
        {
            var trip = new TripAggregate
            {
                Id = Guid.NewGuid(),
                FromCity = fromCity,
                ToCity = toCity,
                AvailableSeats = availableSeats,
                Status = TripStatus.Published
            };

            trip.AddDomainEvent(new TripCreatedDomainEvent(
                trip.Id,
                driverId,
                fromCity,
                toCity,
                departureTime,
                availableSeats,
                pricePerSeat
            ));

            return trip;
        }

        public void Cancel(string reason)
        {
            if (Status == TripStatus.Cancelled)
                throw new InvalidOperationException("Already cancelled");

            Status = TripStatus.Cancelled;

            AddDomainEvent(new TripCancelledDomainEvent(
                Id,
                reason,
                DateTime.UtcNow
            ));
        }
    }

}
