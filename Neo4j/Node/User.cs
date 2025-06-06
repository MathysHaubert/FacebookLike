public enum PrivacyLevel
{
    Public,
    FriendsOnly,
    Private
}

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    public string ProfileImageUrl { get; set; } = String.Empty;

    public string? Bio { get; set; }
    public string? Work { get; set; }
    public string? WorksAt { get; set; }
    public string? Studies { get; set; }
    public string? LivesIn { get; set; }
    public string? Relationship { get; set; }
    
    public PrivacyLevel PrivacyLevel { get; set; } = PrivacyLevel.FriendsOnly;
}