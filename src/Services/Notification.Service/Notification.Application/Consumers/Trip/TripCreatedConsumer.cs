using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Shared.Messaging.SharedMessagingDomainEvents.TripEvents;

namespace Notification.Application.Consumers.Trip
{
    /// <summary>
    /// Consommateur qui réagit à la création d'un trip
    /// Envoie un email de confirmation au conducteur
    /// </summary>
    public class TripCreatedConsumer : IConsumer<TripCreatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<TripCreatedConsumer> _logger;

        public TripCreatedConsumer(
            INotificationRepository notificationRepository,
            IEmailService emailService,
            ILogger<TripCreatedConsumer> logger
            )
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TripCreatedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation(
                "Réception d'un évènement TripCreatedEvent: Id du trajet={TripId}, Conducteur={DriverId}, {From} -> {To}",
                @event.TripId, @event.DriverId, @event.FromCity, @event.ToCity);

            var notification = new NotificationEntity
            {
                UserId = @event.DriverId,
                Type = NotificationType.Email,
                Title = "Trajet avec succès",
                Message = $"Votre trajet {@event.FromCity} -> {@event.ToCity} " +
                            $"le {@event.DepartureTime:dd/MM/yyyy} a été créé avec succès." +
                            $"{@event.AvailableSeats} places disponibles à {@event.PricePerSeat}€/place.",
                SourceEvent = nameof(TripCreatedEvent),
                RelatedEntityId = @event.TripId,
                Status = NotificationStatus.Pending,
                Metadata = new Dictionary<string, object>
                {
                    ["TripId"] = @event.TripId,
                    ["FromCity"] = @event.FromCity,
                    ["ToCity"] = @event.ToCity,
                    ["DepartureTime"] = @event.DepartureTime,
                    ["AvailableSeats"] = @event.AvailableSeats,
                    ["PricePerSeat"] = @event.PricePerSeat
                }
            };

            // Sauvegarder dans MongoDB
            await _notificationRepository.AddAsync(notification);

            try
            {
                var emailSent = await _emailService.SendEmailAsync(
                    to: "driver@example.com",
                    subject: notification.Title,
                    body: notification.Message
                );

                if(emailSent)
                {
                    notification.MarkAsSent();
                    _logger.LogInformation("Email envoyé pour TripCreated: {TripId}", @event.TripId);
                }
                else
                {
                    notification.MarkAsFailed("Echec de l'envoi de l'e-mail");
                    _logger.LogWarning("Echec de l'envoi de l'e-mail pour TripCreated: {TripId}", @event.TripId);
                }

                await _notificationRepository.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogError(ex, "Erreur lors de l'envoi de la notification pour TripCreated: {TripId}", @event.TripId);
            }
        }
    }
}