using FacebookLike.Models;
using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IPostService
{
    Task<List<PostDetails>> GetFriendsPostsAsync(string userId, int page, int pageSize);
}