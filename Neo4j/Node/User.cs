public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    public string ProfileImageUrl { get; set; } = String.Empty;
    public string? CoverImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? Work { get; set; }
    public string? WorksAt { get; set; }
    public string? Studies { get; set; }
    public string? LivesIn { get; set; }
    public string? From { get; set; }
    public string? Relationship { get; set; }
}