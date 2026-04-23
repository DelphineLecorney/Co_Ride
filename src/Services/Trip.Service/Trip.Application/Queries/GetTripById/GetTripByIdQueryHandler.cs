using Marten;
using MediatR;
using Trip.Application.Projections.ReadModels;

namespace Trip.Application.Queries.GetTripById;

/// <summary>
/// Handler pour récupérer un trip par son ID depuis le Read Model.
/// </summary>
public class GetTripByIdQueryHandler : IRequestHandler<GetTripByIdQuery, TripReadModel?>
{
    private readonly IQuerySession _querySession;

    public GetTripByIdQueryHandler(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public async Task<TripReadModel?> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        var trip = await _querySession
            .LoadAsync<TripReadModel>(request.TripId, cancellationToken);

        return trip;
    }
}