#region

using FacebookLike.Neo4j.Node;
using FacebookLike.Service;
using FacebookLike.Service.Security;
using Neo4jClient;

#endregion

namespace FacebookLike.Repository;

public class UserRepository(IGraphClient client)
{
    public async Task<User?> GetByUsernameAndPassword(string email, string password)
    {
        var encodedPassword = PasswordEncoder.Encode(password);
        var user = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Email == email && u.Password == encodedPassword)
            .Return(u => u.As<User>())
            .ResultsAsync;
        return user.SingleOrDefault();
    }

    public async Task<User?> GetById(string id)
    {
        var user = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Id == id)
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
        if (string.IsNullOrEmpty(user.Id))
            user.Id = Guid.NewGuid().ToString();
        await client.Cypher.Create("(u:User $user)").WithParam("user", user).ExecuteWithoutResultsAsync();
    }

    public async Task Update(User user)
    {
        await client.Cypher
            .Match("(u:User)")
            .Where((User u) => u.Id == user.Id)
            .Set("u = $user")
            .WithParam("user", user)
            .ExecuteWithoutResultsAsync();
    }

    public async Task<List<User>> SearchUsers(string searchTerm)
    {
        var users = await client.Cypher
            .Match("(u:User)")
            .Where((User u) => 
                u.FirstName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                u.LastName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                u.Username.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
            .Return(u => u.As<User>())
            .ResultsAsync;
        return users.ToList();
    }
} 