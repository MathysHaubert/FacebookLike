using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IMessageService
{
    Task<List<Message>> GetMessagesAsync(string conversationId);
    Task<Message> SendMessageAsync(string conversationId, string senderId, string? textContent, string? imageUrl);
    Task EditMessageAsync(string messageId, string newTextContent, string? newImageUrl);
    Task DeleteMessageAsync(string messageId);
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
    Task<long> GetUnreadCountAsync(string userId);
    Task MarkMessagesAsReadAsync(string conversationId, string userId);
    Task<Conversation> GetOrCreateConversationAsync(string userId1, string userId2);
    Task<Conversation> GetConversationById(string conversationId);
    Task<List<Conversation>> GetConversationsForUserAsync(string userId);
    Task<long> GetUnreadCountForConversationAsync(string conversationId, string userId);
} 