using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Shared.Messaging.SharedMessagingDomainEvents.BookingEvents;

namespace Notification.Application.Consumers.Booking
{
    /// <summary>
    /// Consommateur qui réagit à l'annulation d'une réservation
    /// Envoie un email au passager
    /// </summary>
    public class BookingCancelledConsumer : IConsumer<BookingCancelledEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingCancelledConsumer> _logger;

        public BookingCancelledConsumer(
            INotificationRepository notificationRepository,
            IEmailService emailService,
            ILogger<BookingCancelledConsumer> logger)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BookingCancelledEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation(
                "Réception de l'évènement BookingCancelledEvent: Id de la réservation={BookingId}, Raison={Reason}",
                @event.BookingId, @event.PassengerId);

            var notification = new NotificationEntity
            {
                UserId = @event.PassengerId,
                Type = NotificationType.Email,
                Title = "Réservation annulée",
                Message = $"Votre réservation a été annulée. Raison: {@event.Reason}. " +
                     $"Si vous avez effectué un paiement, il sera remboursé sous 3-5 jours ouvrés.",
                SourceEvent = nameof(BookingCancelledEvent),
                RelatedEntityId = @event.BookingId,
                Status = NotificationStatus.Pending,
                Metadata = new Dictionary<string, object>
                {
                    ["BookingId"] = @event.BookingId,
                    ["Reason"] = @event.Reason,
                    ["CancelledAt"] = @event.CancelledAt
                }
            };

            await _notificationRepository.AddAsync(notification);

            try
            {
                var emailSent = await _emailService.SendEmailAsync(
                    to: "passenfer@example.com",
                    subject: notification.Title,
                    body: notification.Message
                    );

                if(emailSent)
                {
                    notification.MarkAsSent();
                    _logger.LogInformation("Email envoyé pour BookingCancelled: {BookingId}", @event.BookingId);

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
                _logger.LogError(ex, "Erreur lors de l'envoi de la notification pour BookingCancelled: {BookingId}", @event.BookingId);
            }
        }
    }
}