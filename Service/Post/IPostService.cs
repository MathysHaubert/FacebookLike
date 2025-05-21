using FacebookLike.Models;

namespace FacebookLike.Service;

public interface IPostService
{
    Task<List<Post>> GetPostsAsync();
    Task<List<PostWithAuthor>> GetFriendsPostsAsync(string userId, int page, int pageSize);
}