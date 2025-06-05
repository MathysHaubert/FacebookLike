using FacebookLike.Models;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using FacebookLike.Neo4j.Node;

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
                .Match("(u:User {Id: $userId})", "(p:Post {Id: $postId})")
                .WithParam("userId", userId)
                .WithParam("postId", postId)
                .Create("(c:Comment $comment)")
                .WithParam("comment", comment)
                .Merge("(u)-[:WROTE]->(c)")
                .Merge("(c)-[:COMMENTED]->(p)")
                .ExecuteWithoutResultsAsync();
        }

        public async Task<List<CommentDetails>> GetCommentsByPost(string postId)
        {
            var result = await _client.Cypher
                .Match("(p:Post {Id: $postId})<-[:COMMENTED]-(c:Comment)<-[:WROTE]-(u:User)")
                .WithParam("postId", postId)
                .Return((c, u) => new CommentDetails
                {
                    Comment = c.As<Comment>(),
                    Author = u.As<User>()
                })
                .OrderBy("c.CreatedAt")
                .ResultsAsync;
            return result.ToList();
        }

        public async Task<long> GetCommentsCountByPost(string postId)
        {
            var result = await _client.Cypher
                .Match("(c:Comment)-[:COMMENTED]->(p:Post {Id: $postId})")
                .WithParam("postId", postId)
                .Return(c => c.Count())
                .ResultsAsync;
            return result.SingleOrDefault();
        }
    }
}