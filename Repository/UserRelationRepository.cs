using Neo4jClient;

namespace FacebookLike.Service.Neo4jService;

public class UserRelationRepository
{
    private readonly IGraphClient _client;
    public UserRelationRepository(IGraphClient client)
    {
        _client = client;
    }

    public async Task CreateFriendship(string fromUsername, string toUsername)
    {
        await _client.Cypher
            .Match("(a:User {Username: $from}), (b:User {Username: $to})")
            .WithParam("from", fromUsername)
            .WithParam("to", toUsername)
            .Create("(a)-[:FRIEND]->(b)")
            .ExecuteWithoutResultsAsync();
    }
} 