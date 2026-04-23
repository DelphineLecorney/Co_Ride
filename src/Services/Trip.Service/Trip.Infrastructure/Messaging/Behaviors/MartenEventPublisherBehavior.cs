using Marten;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Trip.Domain.TripDomainEvents;
using Trip.Infrastructure.Messaging.Contracts;

namespace Trip.Infrastructure.Messaging.Behaviors;

/// <summary>
/// Pipeline behavior MediatR qui publie automatiquement les événements de domaine
/// vers RabbitMQ après chaque commande.
/// </summary>
public class MartenEventPublisherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IDocumentSession _session;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MartenEventPublisherBehavior<TRequest, TResponse>> _logger;

        public MartenEventPublisherBehavior(
            IDocumentSession session,
        IPublishEndpoint publishEndpoint,
        ILogger<MartenEventPublisherBehavior<TRequest, TResponse>> logger)
    {
        _session = session;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        var pendingEvents = _session.PendingChanges.Streams()
            .SelectMany(stream => stream.Events)
            .ToList();

        foreach (var @event in pendingEvents)
        {
            await PublishIntegrationEvent(@event.Data, cancellationToken);
        }

        return response;
    }

    private async Task PublishIntegrationEvent(object domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            switch (domainEvent)
            {
                case TripCreated tripCreated:
                    var tripCreatedEvent = new TripCreatedIntegrationEvent
                    {
                        TripId = tripCreated.TripId,
                        DriverId = tripCreated.DriverId,
                        FromCity = tripCreated.FromCity,
                        ToCity = tripCreated.ToCity,
                        DepartureTime = tripCreated.DepartureTime,
                        TotalSeats = tripCreated.TotalSeats,
                        PricePerSeat = tripCreated.PricePerSeat,
                        CreatedAt = tripCreated.CreatedAt
                    };
                    await _publishEndpoint.Publish(tripCreatedEvent, cancellationToken);
                    _logger.LogInformation(
                        "Published TripCreatedIntegrationEvent for TripId: {TripId}",
                        tripCreated.TripId);
                    break;

                case TripCancelled tripCancelled:
                    var tripCancelledEvent = new TripCancelledIntegrationEvent
                    {
                        TripId = tripCancelled.TripId,
                        Reason = tripCancelled.Reason,
                        CancelledAt = tripCancelled.CancelledAt
                    };
                    await _publishEndpoint.Publish(tripCancelledEvent, cancellationToken);
                    _logger.LogInformation(
                        "Published TripCancelledIntegrationEvent for TripId: {TripId}",
                        tripCancelled.TripId);
                    break;

                case SeatsReserved seatsReserved:

                    var seatsReservedEvent = new SeatsReservedIntegrationEvent
                    {
                        TripId = seatsReserved.TripId,
                        PassengerId = seatsReserved.PassengerId,
                        SeatsReserved = seatsReserved.SeatsCount,
                        AvailableSeats = seatsReserved.AvailableSeats,
                        ReservedAt = seatsReserved.ReservedAt
                    };

                    await _publishEndpoint.Publish(seatsReservedEvent, cancellationToken);
                    _logger.LogInformation(
                        "Published SeatsReservedIntegrationEvent for TripId: {TripId}, BookingId: {BookingId}",
                        seatsReserved.TripId, seatsReserved.PassengerId);
                    break;

                default:
                    _logger.LogDebug(
                        "Domain event type {EventType} not configured for integration event publishing",
                        domainEvent.GetType().Name);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to publish integration event for domain event {EventType}",
                domainEvent.GetType().Name);
        }
    }
}