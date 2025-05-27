using FacebookLike.Models;
using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface ICommentService
{
    Task<List<CommentDetails>> GetCommentsByPostAsync(string postId);
    Task AddCommentAsync(string postId, string userId, string content);
}