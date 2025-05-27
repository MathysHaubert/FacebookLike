using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;

namespace FacebookLike.Service
{
    public class CommentService(CommentRepository commentRepository) : ICommentService
    {
        public async Task<List<CommentDetails>> GetCommentsByPostAsync(string postId)
        {
            return await commentRepository.GetCommentsByPost(postId);
        }

        public async Task AddCommentAsync(string postId, string userId, string content)
        {
            await commentRepository.AddComment(postId, userId, content);
        }
    }
} 