using FacebookLike.Models;
using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IPostService
{
    Task<List<PostDetails>> GetFriendsPostsAsync(string userId, int page, int pageSize);
    Task<PostDetails> GetPostByIdAsync(string postId, string userId);
    Task<PostDetails> CreatePostAsync(string userId, string content, Stream imageStream, string fileName);
    Task ToggleLikeAsync(string postId, string userId);
}