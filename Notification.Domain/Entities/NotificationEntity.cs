using Notification.Domain.Enums;

namespace Notification.Domain.Entities
{
    public class NotificationEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? RecipientEmail { get; set; }
        public string? RecipientPhone { get; set; }
        public NotificationStatus Status { get; set; }
        public string? SourceEvent { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
            SentAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationStatus.Failed;
            ErrorMessage = errorMessage;
        }

        public void MarkAsRead()
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }
}
