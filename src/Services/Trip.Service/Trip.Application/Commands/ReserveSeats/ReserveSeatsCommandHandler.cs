using Marten;
using MediatR;
using Trip.Domain.Aggregates;

namespace Trip.Application.Commands.ReserveSeats;

public class ReserveSeatsCommandHandler : IRequestHandler<ReserveSeatsCommand, ReserveSeatsResponse>
{
    private readonly IDocumentSession _session;

    public ReserveSeatsCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<ReserveSeatsResponse> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
    {
        var trip = await _session.Events
            .AggregateStreamAsync<TripAggregate>(request.TripId, token: cancellationToken);


        if (trip is null)
        {
            return new ReserveSeatsResponse(false, "Trip not found", 0);
        }

        trip.ReserveSeats(request.BookingId, request.SeatsCount);

        _session.Events.Append(trip.Id, trip.GetUncommittedEvents());
        await _session.SaveChangesAsync(cancellationToken);

        return new ReserveSeatsResponse(true, "Seats reserved successfully", trip.AvailableSeats);
    }
}

