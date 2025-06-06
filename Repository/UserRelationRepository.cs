using FacebookLike.Neo4j.Node;
using Neo4jClient;

namespace FacebookLike.Service.Neo4jService;

public class UserRelationRepository(IGraphClient client)
{
    public async Task CreateFriendship(string fromId, string toId)
    {
        await client.Cypher
            .Match("(a:User {Id: $from}), (b:User {Id: $to})")
            .WithParam("from", fromId)
            .WithParam("to", toId)
            .Merge("(a)-[:FRIEND]->(b)")
            .ExecuteWithoutResultsAsync();
    }
    
    public async Task DeleteFriendship(string fromId, string toId)
    {
        await client.Cypher
            .Match("(a:User {Id: $from})-[r:FRIEND]->(b:User {Id: $to})")
            .WithParam("from", fromId)
            .WithParam("to", toId)
            .Delete("r")
            .ExecuteWithoutResultsAsync();
    }

    public async Task<List<string>> GetFriendIds(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.As<User>().Id)
            .ResultsAsync;
        return result.ToList();
    }

    public async Task<List<User>> GetFriends(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.As<User>())
            .ResultsAsync;
        return result.ToList();
    }
    
    public async Task<long> GetFollowersCount(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User)-[:FRIEND]->(b:User {Id: $userId})")
            .WithParam("userId", userId)
            .Return(b => b.Count())
            .ResultsAsync;
        return result.SingleOrDefault();
    }
    
    public async Task<long> GetFollowingCount(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.Count())
            .ResultsAsync;
        return result.SingleOrDefault();
    }
    
    public async Task<bool> IsFollowing(string userId, string targetUserId)
    {
        var result = await client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User {Id: $targetUserId})")
            .WithParam("userId", userId)
            .WithParam("targetUserId", targetUserId)
            .Return(b => b.Count())
            .ResultsAsync;
        return result.SingleOrDefault() > 0;
    }
    
    public async Task<List<User>> GetFollowers(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User)-[:FRIEND]->(b:User {Id: $userId})")
            .WithParam("userId", userId)
            .Return(a => a.As<User>())
            .ResultsAsync;
        return result.ToList();
    }
    
    public async Task<List<User>> GetFollowing(string userId)
    {
        var result = await client.Cypher
            .Match("(a:User {Id: $userId})-[:FRIEND]->(b:User)")
            .WithParam("userId", userId)
            .Return(b => b.As<User>())
            .ResultsAsync;
        return result.ToList();
    }
} 