namespace FacebookLike.Neo4j.Node;

public class Conversation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string User1Id { get; set; }
    public required string User2Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 