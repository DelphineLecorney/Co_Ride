using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Shared.Messaging.SharedMessagingDomainEvents.BookingEvents;

namespace Notification.Application.Consumers.Booking
{
    /// <summary>
    /// Consommateur qui réagit à la création d'une réservation
    /// Envoie un email de confirmation au passager
    /// </summary>
    public class BookingCreatedConsumer : IConsumer<BookingCreatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingCreatedConsumer> _logger;

        public BookingCreatedConsumer(
            INotificationRepository notificationRepository,
            IEmailService emailService,
            ILogger<BookingCreatedConsumer> logger)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BookingCreatedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation(
                "Réception d'un évènement BookingCreatedEvent: Id de la réservation={BookingId}, Passager={PassengerId}, Places réservées={SeatsBooked}",
                @event.BookingId, @event.PassengerId, @event.SeatsBooked);

            // Notification pour le passager
            var notification = new NotificationEntity
            {
                UserId = @event.PassengerId,
                Type = NotificationType.Email,
                Title = "Réservation confirmée",
                Message = $"Votre réservation de {@event.SeatsBooked} place(s) a été confirmée. " +
                    $"Montant total: {@event.TotalPrice}€. Bon voyage!",
                SourceEvent = nameof(BookingCreatedEvent),
                RelatedEntityId = @event.BookingId,
                Status = NotificationStatus.Pending,
                Metadata = new Dictionary<string, object>
                {
                    ["BookinId"] = @event.BookingId,
                    ["TripId"] = @event.TripId,
                    ["SeatsBooked"] = @event.SeatsBooked,
                    ["TotalPrice"] = @event.TotalPrice
                }
            };

            await _notificationRepository.AddAsync(notification);

            try
            {
                var emailSent = await _emailService.SendEmailAsync(
                    to: "passenger@example.com",
                    subject: notification.Title,
                    body: notification.Message
                    );

                if (emailSent)
                {
                    notification.MarkAsSent();
                    _logger.LogInformation("Email envoyé pour BookingCreated: {BookingId}", @event.BookingId);
                }
                else
                {
                    notification.MarkAsFailed("Echec de l'envoi de l'e-mail");
                }

                await _notificationRepository.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogError(ex, "Erreur d'envoi de la notification pour BookingCreated: {BookingId}", @event.BookingId);
            }
        }
    }
}
