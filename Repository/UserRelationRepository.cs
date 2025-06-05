using FacebookLike.Neo4j.Node;
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
            .Merge("(a)-[:FRIEND]->(b)")
            .ExecuteWithoutResultsAsync();
    }

    public async Task<List<string>> GetFriendIds(string userId)
    {
        var result = await _client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.As<User>().Id)
            .ResultsAsync;
        return result.ToList();
    }

    public async Task<List<User>> GetFriends(string userId)
    {
        var result = await _client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.As<User>())
            .ResultsAsync;
        return result.ToList();
    }
} 