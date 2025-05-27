namespace FacebookLike.Neo4j.Node;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Username { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    
    public required string Password { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    
}
