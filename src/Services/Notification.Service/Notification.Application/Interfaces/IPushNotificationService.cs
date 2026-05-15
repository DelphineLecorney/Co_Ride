namespace Notification.Application.Interfaces
{
    public interface IPushNotificationService
    {
        Task<bool> SendPushNotificationAsync(
            Guid userId, 
            string title, 
            string message, 
            CancellationToken cancellationToken = default);
    }
}
