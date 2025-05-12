using FacebookLike.Neo4j.Node;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Neo4j.DataSeeder;

public class InitSeeder
{
    private readonly Neo4JService _neo4JService;

    public InitSeeder(Neo4JService neo4jService)
    {
        _neo4JService = neo4jService;
    }

    public async Task SeedAsync()
    {
        var alice = new User { Username = "alice", Email = "alice@example.com" };
        var bob = new User { Username = "bob", Email = "bob@example.com" };
        var pierre = new User { Username = "pierre", Email = "pierre@example.com" };
        var paul = new User { Username = "paul", Email = "paul@example.com" };

        await _neo4JService.CreateUserAsync(alice);
        await _neo4JService.CreateUserAsync(bob);
        await _neo4JService.CreateUserAsync(pierre);
        await _neo4JService.CreateUserAsync(paul);
        await _neo4JService.CreateFriendshipAsync("alice", "bob");
        await _neo4JService.CreateFriendshipAsync("bob", "alice");
        await _neo4JService.CreateFriendshipAsync("bob", "pierre");
        await _neo4JService.CreateFriendshipAsync("pierre", "alice");
    }
}
