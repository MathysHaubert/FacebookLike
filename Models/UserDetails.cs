namespace FacebookLike.Models;

public class UserDetails
{
    public User User { get; set; }
    public long FollowersCount { get; set; }
    public long FollowingCount { get; set; }
    public List<PostDetails> Posts { get; set; }
}