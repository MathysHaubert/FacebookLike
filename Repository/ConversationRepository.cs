using FacebookLike.Neo4j.Node;
using Neo4jClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FacebookLike.Repository
{
    public class ConversationRepository
    {
        private readonly IGraphClient _client;
        public ConversationRepository(IGraphClient client)
        {
            _client = client;
        }

        public async Task<Conversation> GetOrCreateConversationAsync(string userId1, string userId2)
        {
            var (u1, u2) = string.CompareOrdinal(userId1, userId2) < 0 ? (userId1, userId2) : (userId2, userId1);
            var result = await _client.Cypher
                .Match("(c:Conversation)")
                .Where((Conversation c) => c.User1Id == u1 && c.User2Id == u2)
                .Return(c => c.As<Conversation>())
                .ResultsAsync;

            var conversation = result.FirstOrDefault();
            if (conversation != null)
                return conversation;

            var newConv = new Conversation { User1Id = u1, User2Id = u2 };
            await _client.Cypher
                .Create("(c:Conversation $conv)")
                .WithParam("conv", newConv)
                .ExecuteWithoutResultsAsync();
            return newConv;
        }

        public async Task<List<Conversation>> GetConversationsForUserAsync(string userId)
        {
            var result = await _client.Cypher
                .Match("(c:Conversation)")
                .Where("c.User1Id = $userId OR c.User2Id = $userId")
                .WithParam("userId", userId)
                .Return(c => c.As<Conversation>())
                .ResultsAsync;
            return result.ToList();
        }
    }
} 