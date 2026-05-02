using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationRepository notificationRepository, 
            ILogger<NotificationsController> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        // Récupérer les notifications d'un utilisateur
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserNotifications(
            Guid userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId, page, pageSize);
            var unreadCount = await _notificationRepository.CountUnreadByUserIdAsync(userId);

            return Ok(new
            {
                notifications,
                unreadCount,
                page,
                pageSize
            });
        }

        // Récupérer une notification par ID
        [HttpPost("{id}/mark-read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);

            if (notification == null)
            {
                return NotFound(new { message = $"Notification {id} pas trouvé" });
            }

            notification.MarkAsRead();
            await _notificationRepository.UpdateAsync(notification);

            return Ok(new { message = "Notification marqué comme lue" });
        }
    }
}
