using Marten;
using MediatR;
using Trip.Domain.Aggregates;
using Trip.Domain.ValueObjects;

namespace Trip.Application.Commands.CreateTrip;

public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, Guid>
{
    private readonly IDocumentSession _session;

    public CreateTripCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<Guid> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var trip = TripAggregate.Create(
            request.DriverId,
            Address.Create(request.FromCity),
            Address.Create(request.ToCity),
            request.DepartureTime,
            request.TotalSeats,
            Price.Create(request.PricePerSeat)
        );

        _session.Events.StartStream<TripAggregate>(trip.Id,trip.GetUncommittedEvents());

        await _session.SaveChangesAsync(cancellationToken);

        return trip.Id;
    }
}
