using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FacebookLike.Service
{
    public class MessageHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UserConnections = new();
        private readonly ILogger<MessageHub> _logger;
        private readonly IMessageService _messageService;

        public MessageHub(ILogger<MessageHub> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
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

        public async Task SendMessage(string conversationId, string senderId, string textContent, string? imageUrl)
        {
            _logger.LogInformation($"Message sent in conversation {conversationId} by user {senderId}: {textContent}");
            var message = await _messageService.SendMessageAsync(conversationId, senderId, textContent, imageUrl);
            await Clients.Group(conversationId).SendAsync("ReceiveMessageFull", message);
            var conv = await _messageService.GetConversationById(conversationId);
            string? recipientId = conv.User1Id == senderId ? conv.User2Id : conv.User1Id;
            if (!string.IsNullOrEmpty(recipientId) && UserConnections.TryGetValue(recipientId, out var connId))
            {
                await Clients.Client(connId).SendAsync("ReceiveNewMessageNotification", message);
                var unreadCount = await _messageService.GetUnreadCountForConversationAsync(conversationId, recipientId);
                await Clients.All.SendAsync("UpdateUnreadCountForConversation", conversationId, unreadCount,
                    recipientId);
            }
        }

        public async Task EditMessage(string messageId, string newTextContent, string? newImageUrl, string conversationId)
        {
            _logger.LogInformation($"Message {messageId} edited by user {Context.ConnectionId}");
            await _messageService.EditMessageAsync(messageId, newTextContent, newImageUrl);
            await Clients.Group(conversationId).SendAsync("MessageEdited", messageId, newTextContent, newImageUrl);
        }

        public async Task DeleteMessage(string messageId, string conversationId)
        {
            _logger.LogInformation($"Message {messageId} deleted by user {Context.ConnectionId}");
            await _messageService.DeleteMessageAsync(messageId);
            await Clients.Group(conversationId).SendAsync("MessageDeleted", messageId);
        }

        public async Task JoinConversation(string conversationId)
        {
            _logger.LogInformation($"User {Context.ConnectionId} joining conversation {conversationId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            _logger.LogInformation($"User {Context.ConnectionId} leaving conversation {conversationId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task MarkConversationAsRead(string conversationId, string userId)
        {
            await _messageService.MarkMessagesAsReadAsync(conversationId, userId);
            _logger.LogInformation($"User {userId} marked conversation {conversationId} as read");
            if (!string.IsNullOrEmpty(userId))
            {
                var unreadCount = await _messageService.GetUnreadCountForConversationAsync(conversationId, userId);
                await Clients.All.SendAsync("UpdateUnreadCountForConversation", conversationId, unreadCount, userId);
            }
        }
    }
} 