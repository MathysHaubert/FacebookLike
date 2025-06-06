using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;

namespace FacebookLike.Service
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationService(NotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> CreateNotificationAsync(string recipientId, string senderId, string type, string message)
        {
            return await _notificationRepository.CreateNotificationAsync(recipientId, senderId, type, message);
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            return await _notificationRepository.GetNotificationsAsync(userId);
        }

        public async Task<long> GetUnreadCountAsync(string userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task MarkAsReadAsync(string notificationId)
        {
            await _notificationRepository.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            await _notificationRepository.MarkAllAsReadAsync(userId);
        }
    }
} 