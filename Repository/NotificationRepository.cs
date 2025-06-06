using FacebookLike.Neo4j.Node;
using Neo4jClient;

namespace FacebookLike.Repository
{
    public class NotificationRepository
    {
        private readonly IGraphClient _client;

        public NotificationRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task<Notification> CreateNotificationAsync(string recipientId, string senderId, string type, string message)
        {
            var notification = new Notification
            {
                RecipientId = recipientId,
                SenderId = senderId,
                Type = type,
                Message = message,
                IsRead = false
            };

            await _client.Cypher
                .Match("(r:User {Id: $recipientId}), (s:User {Id: $senderId})")
                .Create("(n:Notification $notification)")
                .Create("(s)-[:SENT]->(n)")
                .Create("(n)-[:SENT_TO]->(r)")
                .WithParams(new
                {
                    recipientId,
                    senderId,
                    notification
                })
                .ExecuteWithoutResultsAsync();

            return notification;
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            var result = await _client.Cypher
                .Match("(n:Notification)-[:SENT_TO]->(u:User {Id: $userId})")
                .Match("(s:User)-[:SENT]->(n)")
                .With("n, s")
                .OrderByDescending("n.createdAt")
                .WithParams(new { userId })
                .Return((n, s) => new
                {
                    Notification = n.As<Notification>(),
                    Sender = s.As<User>()
                })
                .ResultsAsync;

            return result.Select(r =>
            {
                r.Notification.Sender = r.Sender;
                return r.Notification;
            }).ToList();
        }

        public async Task<long> GetUnreadCountAsync(string userId)
        {
            var result = await _client.Cypher
                .Match("(n:Notification)-[:SENT_TO]->(u:User {Id: $userId})")
                .Where("NOT n.IsRead")
                .WithParams(new { userId })
                .Return(n => n.Count())
                .ResultsAsync;
            
            return result.FirstOrDefault();
        }

        public async Task MarkAsReadAsync(string notificationId)
        {
            await _client.Cypher
                .Match("(n:Notification {Id: $notificationId})")
                .Set("n.isRead = true")
                .WithParams(new { notificationId })
                .ExecuteWithoutResultsAsync();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            await _client.Cypher
                .Match("(n:Notification)-[:SENT_TO]->(u:User {Id: $userId})")
                .Set("n.IsRead = true")
                .WithParams(new { userId })
                .ExecuteWithoutResultsAsync();
        }
    }
} 