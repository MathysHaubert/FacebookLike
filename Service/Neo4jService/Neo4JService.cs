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
        var session = _driver.AsyncSession();
        try
        {
            await session.RunAsync(query, new { user.Username, user.Email });
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task<bool> UsersExistAsync(params string[] usernames)
    {
        var query = "MATCH (u:User) WHERE u.username IN $usernames RETURN count(u) as count";
        var session = _driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new { usernames });
            var record = await result.SingleAsync();
            return record["count"].As<int>() == usernames.Length;
        }
        finally
        {
            await session.CloseAsync();
        }
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