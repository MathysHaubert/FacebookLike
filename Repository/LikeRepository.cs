using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacebookLike.Repository
{
    public class LikeRepository(IGraphClient client)
    {
        public async Task AddLike(string postId, string userId)
        {
            await client.Cypher
                .Match("(u:User {Id: $userId}), (p:Post {Id: $postId})")
                .WithParam("userId", userId)
                .WithParam("postId", postId)
                .Merge("(u)-[:LIKED]->(p)")
                .ExecuteWithoutResultsAsync();
        }

        public async Task RemoveLike(string postId, string userId)
        {
            await client.Cypher
                .Match("(u:User {Id: $userId})-[l:LIKED]->(p:Post {Id: $postId})")
                .WithParam("userId", userId)
                .WithParam("postId", postId)
                .Delete("l")
                .ExecuteWithoutResultsAsync();
        }

        public async Task<long> GetLikesCountByPost(string postId)
        {
            var result = await client.Cypher
                .Match("(u:User)-[l:LIKED]->(p:Post {Id: $postId})")
                .WithParam("postId", postId)
                .Return(l => l.Count())
                .ResultsAsync;
            return result.SingleOrDefault();
        }

        public async Task<bool> HasUserLiked(string postId, string userId)
        {
            var result = await client.Cypher
                .Match("(u:User {Id: $userId})-[l:LIKED]->(p:Post {Id: $postId})")
                .WithParam("userId", userId)
                .WithParam("postId", postId)
                .Return(l => l.Count())
                .ResultsAsync;
            return result.SingleOrDefault() > 0;
        }
    }
} 