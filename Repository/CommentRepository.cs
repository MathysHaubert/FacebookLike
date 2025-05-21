using FacebookLike.Models;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacebookLike.Repository
{
    public class CommentRepository
    {
        private readonly IGraphClient _client;
        public CommentRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task AddComment(string postId, string userId, string content)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                PostId = postId,
                AuthorId = userId,
                Content = content,
                CreatedAt = DateTime.Now
            };
            await _client.Cypher
                .Match("(u:User {Id: $userId}), (p:Post {Id: $postId})")
                .WithParam("userId", userId)
                .WithParam("postId", postId)
                .Create("(c:Comment $comment)<-[:WROTE]-(u)-[:COMMENTED]->(p)")
                .WithParam("comment", comment)
                .ExecuteWithoutResultsAsync();
        }

        public async Task<List<Comment>> GetCommentsByPost(string postId)
        {
            var result = await _client.Cypher
                .Match("(p:Post {Id: $postId})<-[:COMMENTED]-(u:User)-[:WROTE]->(c:Comment)")
                .WithParam("postId", postId)
                .Return((c, u) => c.As<Comment>())
                .ResultsAsync;
            return result.ToList();
        }

        public async Task<long> GetCommentsCountByPost(string postId)
        {
            var result = await _client.Cypher
                .Match("(p:Post {Id: $postId})<-[:COMMENTED]-(u:User)-[:WROTE]->(c:Comment)")
                .WithParam("postId", postId)
                .Return(c => c.Count())
                .ResultsAsync;
            return result.SingleOrDefault();
        }
    }
} 