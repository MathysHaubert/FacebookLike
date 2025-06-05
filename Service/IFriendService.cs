using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IFriendService
{
    Task<List<User>> GetFriendsAsync(string userId);
} 