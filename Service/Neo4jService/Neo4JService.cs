using FacebookLike.Neo4j.Node;
using Neo4j.Driver;

namespace FacebookLike.Service.Neo4jService;

public class Neo4JService
{
    private readonly IDriver _driver;

    public Neo4JService(IDriver driver)
    {
        _driver = driver;
    }

    public async Task CreateUserAsync(User user)
    {
        var query = "CREATE (u:User {username: $Username, Email: $Email})";
        await using var session = _driver.AsyncSession();
        await session.RunAsync(query, new { user.Username, user.Email });
    }

    public async Task<bool> UsersExistAsync(params string[] usernames)
    {
        var query = "MATCH (u:User) WHERE u.username IN $usernames RETURN count(u) as count";
        await using var session = _driver.AsyncSession();
        var result = await session.RunAsync(query, new { usernames });
        var record = await result.SingleAsync();
        return record["count"].As<int>() == usernames.Length;
    }

    public async Task CreateFriendshipAsync(string fromUsername, string toUsername)
    {
        var query = @"
            MATCH (a:User {username: $from}), (b:User {username: $to})
            CREATE (a)-[:FRIEND]->(b)";
        var session = _driver.AsyncSession();
        try
        {
            await session.RunAsync(query, new { from = fromUsername, to = toUsername });
        }
        finally
        {
            await session.CloseAsync();
        }
    }
}