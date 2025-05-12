using FacebookLike.Models;

namespace FacebookLike.Service;

public interface IPostService
{
    Task<List<Post>> GetPostsAsync();
    void LikePost(string postId);
    void UnlikePost(string postId);
    void AddOrRemoveLike(Post post);
}