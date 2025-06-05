namespace FacebookLike.Neo4j.Node;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ConversationId { get; set; }
    public required string SenderId { get; set; }
    public string? TextContent { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
} 