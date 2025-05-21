using FacebookLike.Models;

namespace FacebookLike.Service;

public interface ICommentService
{
    Task<List<Comment>> GetCommentsByPostAsync(string postId);
    Task AddCommentAsync(string postId, string userId, string content);
}