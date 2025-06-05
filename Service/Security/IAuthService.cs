using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service.Security;

public interface IAuthService
{
    Task<User?> Authenticate(string username, string password);
    Task<User?> GetUserById(string id);
    Task<User?> GetCurrentUserAsync();
    Task<bool> Register(string username, string password, string email, string firstName, string lastName, string gender, DateOnly dateOfBirth);
    void Logout();
}