using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Shared.Messaging.SharedMessagingDomainEvents.TripEvents;

namespace Notification.Application.Consumers.Trip
{
    /// <summary>
    /// Consommateur qui réagit à l'annulation d'un trip
    /// Envoie des notifications aux passagers concernés
    /// </summary>
    public class TripCancelledConsumer : IConsumer<TripCancelledEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<TripCancelledConsumer> _logger;

        public TripCancelledConsumer(
            INotificationRepository notificationRepository,
            IEmailService emailService,
            ILogger<TripCancelledConsumer> logger
            )
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<TripCancelledEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation(
                "Réception de l'évènement TripCancelledEvent: Id du trajet={TripId}, Raison={Reason}",
                @event.TripId, @event.Reason);

            // Création d'une notification générique
            var notification = new NotificationEntity
            {
                UserId = Guid.Empty,
                Type = NotificationType.Email,
                Title = "Trajet annulé",
                Message = $"Le trajet a été annulé. Raison: {@event.Reason}",
                SourceEvent = nameof(TripCancelledEvent),
                RelatedEntityId = @event.TripId,
                Status = NotificationStatus.Pending,
                Metadata = new Dictionary<string, object>
                {
                    ["TripId"] = @event.TripId,
                    ["Reason"] = @event.Reason,
                    ["CancelledAt"] = @event.CancelledAt
                }

            };

            await _notificationRepository.AddAsync(notification);

            try
            {
                notification.MarkAsSent();
                await _notificationRepository.UpdateAsync(notification);

                _logger.LogInformation("La notification est créé pour TripCancelled: {TripId}", @event.TripId);
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogError(ex, "Erreur de création de la notification pour TripCancelled: {TripId}", @event.TripId);
            }
        }
    }
}
