using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Enums;

namespace Notification.Infrastructure.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<NotificationEntity> _notifications;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(IMongoDatabase database, ILogger<NotificationRepository> logger)
        {
            _notifications = database.GetCollection<NotificationEntity>("notifications");
            _logger = logger;
        }
        public async Task AddAsync(
            NotificationEntity notification, 
            CancellationToken cancellationToken = default)
        {
            await _notifications.InsertOneAsync(notification, cancellationToken: cancellationToken);
            _logger.LogDebug("Notification créée: {NotificationId}", notification.Id);
        }

        public async Task<int> CountUnreadByUserIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<NotificationEntity>.Filter.And(
                Builders<NotificationEntity>.Filter.Eq(n => n.UserId, userId),
                Builders<NotificationEntity>.Filter.Ne(n => n.Status, NotificationStatus.Read)
                );

            return (int)await _notifications.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task<NotificationEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(n => n.Id, id);
            return await _notifications.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<NotificationEntity>> GetByUserIdAsync(
            Guid userId, 
            int page = 1, 
            int pageSize = 20, 
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(n => n.UserId, userId);
            var sort = Builders<NotificationEntity>.Sort.Descending(n => n.CreatedAt);

            return await _notifications
                .Find(filter)
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(
            NotificationEntity notification, 
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(n => n.Id, notification.Id);
            await _notifications.ReplaceOneAsync(filter, notification, cancellationToken: cancellationToken);
            _logger.LogDebug("Notification mise à jour: {NotificationId}", notification.Id);
        }
    }
}
