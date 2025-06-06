using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FacebookLike.Service
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();
        private readonly ILogger<NotificationHub> _logger;
        private readonly INotificationService _notificationService;

        public NotificationHub(ILogger<NotificationHub> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.TryRemove(userId, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string recipientId, string senderId, string type, string message)
        {
            _logger.LogInformation($"Notification sent to {recipientId} from {senderId}: {message}");
            var notification = await _notificationService.CreateNotificationAsync(recipientId, senderId, type, message);
            
            if (UserConnections.TryGetValue(recipientId, out var connId))
            {
                await Clients.Client(connId).SendAsync("ReceiveNotification", notification);
                var unreadCount = await _notificationService.GetUnreadCountAsync(recipientId);
                await Clients.All.SendAsync("UpdateNotificationCount", unreadCount);
            }
        }

        public async Task MarkNotificationAsRead(string notificationId, string userId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            _logger.LogInformation($"User {userId} marked notification {notificationId} as read");
            
            if (!string.IsNullOrEmpty(userId))
            {
                var unreadCount = await _notificationService.GetUnreadCountAsync(userId);
                if (UserConnections.TryGetValue(userId, out var connId))
                {
                    await Clients.Client(connId).SendAsync("UpdateNotificationCount", unreadCount);
                }
            }
        }

        public async Task MarkAllNotificationsAsRead(string userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            _logger.LogInformation($"User {userId} marked all notifications as read");
            
            if (UserConnections.TryGetValue(userId, out var connId))
            {
                await Clients.Client(connId).SendAsync("UpdateNotificationCount", 0);
            }
        }
    }
} 