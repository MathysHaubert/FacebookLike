using FacebookLike.Neo4j.Node;
using FacebookLike.Service;
using Neo4jClient;

namespace FacebookLike.Repository;

public class UserRepository(IGraphClient client)
{
    public async Task<User?> GetByUsernameAndPassword(string username, string password)
    {
        var encodedPassword = PasswordEncoder.Encode(password);
        var user = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Username == username && u.Password == encodedPassword)
            .Return(u => u.As<User>())
            .ResultsAsync;
        return user.SingleOrDefault();
    }

    public async Task<User?> GetById(string id)
    {
        var user = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Id == id) // Adapter si id est une propriété
            .Return(u => u.As<User>())
            .ResultsAsync;
        return user.SingleOrDefault();
    }

    public async Task<bool> UsernameExists(string username)
    {
        var exists = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Username == username)
            .Return(u => u.As<User>())
            .ResultsAsync;
        return exists.Any();
    }

    public async Task<bool> EmailExists(string email)
    {
        var exists = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Email == email)
            .Return(u => u.As<User>())
            .ResultsAsync;
        return exists.Any();
    }

    public async Task Create(User user)
    {
        user.Password = PasswordEncoder.Encode(user.Password);
        await client.Cypher.Create("(u:User $user)").WithParam("user", user).ExecuteWithoutResultsAsync();
    }
} 