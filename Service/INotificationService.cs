using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(string recipientId, string senderId, string type, string message);
        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task<long> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(string notificationId);
        Task MarkAllAsReadAsync(string userId);
    }
} 