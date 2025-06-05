using FacebookLike.Neo4j.Node;
using FacebookLike.Service.GoogleCloud;
using Neo4jClient;
using FacebookLike.Repository;

namespace FacebookLike.Service;

public class MessageService : IMessageService
{
    private readonly IGraphClient _client;
    private readonly StorageService _storageService;
    private readonly ConversationRepository _conversationRepository;

    public MessageService(IGraphClient client, StorageService storageService, ConversationRepository conversationRepository)
    {
        _client = client;
        _storageService = storageService;
        _conversationRepository = conversationRepository;
    }

    public async Task<List<Message>> GetMessagesAsync(string conversationId)
    {
        var result = await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.ConversationId == conversationId)
            .Return(m => m.As<Message>())
            .OrderBy("m.SentAt")
            .ResultsAsync;
        return result.ToList();
    }

    public async Task<Message> SendMessageAsync(string conversationId, string senderId, string? textContent, string? imageUrl)
    {
        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            TextContent = textContent,
            ImageUrl = imageUrl,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        await _client.Cypher
            .Create("(m:Message $message)")
            .WithParam("message", message)
            .ExecuteWithoutResultsAsync();
        
        var result = await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.ConversationId == conversationId && m.SenderId == senderId && m.SentAt == message.SentAt)
            .Return(m => m.As<Message>())
            .OrderByDescending("m.SentAt")
            .Limit(1)
            .ResultsAsync;
        return result.FirstOrDefault() ?? message;
    }

    public async Task EditMessageAsync(string messageId, string newTextContent, string? newImageUrl)
    {
        await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.Id == messageId)
            .Set("m.TextContent = $textContent, m.ImageUrl = $imageUrl")
            .WithParam("textContent", newTextContent)
            .WithParam("imageUrl", newImageUrl)
            .ExecuteWithoutResultsAsync();
    }

    public async Task DeleteMessageAsync(string messageId)
    {
        await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.Id == messageId)
            .Delete("m")
            .ExecuteWithoutResultsAsync();
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        return await _storageService.UploadAndGetUrlAsync(imageStream, fileName);
    }

    public async Task<long> GetUnreadCountAsync(string userId)
    {
        var result = await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.IsRead == false && m.SenderId != userId)
            .Return(m => m.Count())
            .ResultsAsync;
        return result.SingleOrDefault();
    }

    public async Task MarkMessagesAsReadAsync(string conversationId, string userId)
    {
        await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.ConversationId == conversationId && m.SenderId != userId && m.IsRead == false)
            .Set("m.IsRead = true")
            .ExecuteWithoutResultsAsync();
    }

    public async Task<Conversation> GetOrCreateConversationAsync(string userId1, string userId2)
    {
        return await _conversationRepository.GetOrCreateConversationAsync(userId1, userId2);
    }

    public async Task<Conversation> GetConversationById(string conversationId)
    {
        var result = await _client.Cypher
            .Match("(c:Conversation)")
            .Where((Conversation c) => c.Id == conversationId)
            .Return(c => c.As<Conversation>())
            .ResultsAsync;
        return result.FirstOrDefault();
    }

    public async Task<List<Conversation>> GetConversationsForUserAsync(string userId)
    {
        return await _conversationRepository.GetConversationsForUserAsync(userId);
    }

    public async Task<long> GetUnreadCountForConversationAsync(string conversationId, string userId)
    {
        var result = await _client.Cypher
            .Match("(m:Message)")
            .Where((Message m) => m.ConversationId == conversationId && m.IsRead == false && m.SenderId != userId)
            .Return(m => m.Count())
            .ResultsAsync;
        return result.SingleOrDefault();
    }
} 