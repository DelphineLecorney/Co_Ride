using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Services
{
    /// <summary>
    /// Service d'envoi d'emails (implémentation stub pour l'instant)
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly bool _enabled;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _enabled = configuration.GetValue<bool>("Email:Enabled", false);
        }
        public Task<bool> SendEmailAsync(
            string to,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            if(!_enabled)
            {
                _logger.LogInformation(
                    "Le service de messagerie est désactivé, le message aurait été envoyé: To={To}, Subject={Subject}",
                    to, subject);
                return Task.FromResult(true);
            }

            // Voir SendGrid/SMTP
            _logger.LogInformation(
                "Envoi d'un e-mail: To={To}, Subject={Subject}, Body={Body}",
                to, subject, body);

            return Task.FromResult(true);
        }
    }
}
