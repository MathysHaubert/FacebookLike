namespace FacebookLike.Models;

public sealed record UserProfile
{
    public string? UserName { get; init; }
    public string? AvatarUrl { get; init; }
    public string? CoverUrl { get; init; }
    public long Followers { get; init; }
    public string? Bio { get; init; }
    public string? Work { get; init; }
    public string? WorksAt { get; init; }
    public string? Studies { get; init; }
    public string? LivesIn { get; init; }
    public string? From { get; init; }
    public string? Relationship { get; init; }
}