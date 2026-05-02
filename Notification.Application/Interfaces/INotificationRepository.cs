using Notification.Domain.Entities;

namespace Notification.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<NotificationEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<List<NotificationEntity>> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<int> CountUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken = default);
        Task UpdateAsync(NotificationEntity notification, CancellationToken cancellationToken = default);
    }

}
