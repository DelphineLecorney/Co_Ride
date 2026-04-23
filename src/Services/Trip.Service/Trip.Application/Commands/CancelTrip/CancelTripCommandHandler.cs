using Marten;
using MediatR;
using Microsoft.Extensions.Logging;
using Trip.Domain.Aggregates;

namespace Trip.Application.Commands.CancelTrip;

/// <summary>
/// Handler pour annuler un trip.
/// Charge l'aggregate, applique la logique métier, et sauvegarde les événements.
/// </summary>
public class CancelTripCommandHandler
    : IRequestHandler<CancelTripCommand, CancelTripResponse>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<CancelTripCommandHandler> _logger;

    public CancelTripCommandHandler(
        IDocumentSession session,
        ILogger<CancelTripCommandHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<CancelTripResponse> Handle(CancelTripCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tripAggregate = await _session.Events
                .AggregateStreamAsync<TripAggregate>(request.TripId, token: cancellationToken);

            if (tripAggregate == null)
            {
                _logger.LogWarning("Trip {TripId} non trouvé", request.TripId);
                return new CancelTripResponse(false, $"Trip {request.TripId} non trouvé");
            }

            if (tripAggregate.DriverId != request.DriverId)
            {
                _logger.LogWarning(
                    "Tentative d'annulation par un utilisateur non autorisé. TripId: {TripId}, DriverId demandé: {RequestDriverId}, DriverId réel: {ActualDriverId}",
                    request.TripId, request.DriverId, tripAggregate.DriverId);

                return new CancelTripResponse(false, "Seul le conducteur peut annuler ce trajet");
            }

            tripAggregate.Cancel(request.Reason);

            _session.Events.Append(request.TripId, tripAggregate.GetUncommittedEvents().ToArray());
            await _session.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Trip {TripId} annulé avec succès", request.TripId);

            return new CancelTripResponse(true, "Trip annulé avec succès");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erreur métier lors de l'annulation du trip {TripId}", request.TripId);
            return new CancelTripResponse(false, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur inattendue lors de l'annulation du trip {TripId}", request.TripId);
            throw;
        }
    }
}
