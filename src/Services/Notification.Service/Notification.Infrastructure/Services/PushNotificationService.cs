using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(ILogger<PushNotificationService> logger)
        {
            _logger = logger;
        }
        public Task<bool> SendPushNotificationAsync(
            Guid userId, 
            string title, 
            string message, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Notification push : UserId={UserId}, Title={Title}, Message={Message}",
                userId, title, message);

            // Voir Firebase Cloud Messaging
            return Task.FromResult(true);
        }
    }
}
