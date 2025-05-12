using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IAuthService
{
    public User? CurrentUser { get; }
    public event Action? OnAuthStateChanged;
    
    Task<User?> Authenticate(string username, string password);
    Task<User?> GetUserById(string id);
    Task<User> Register(string username, string password, string email, string firstName, string lastName, string gender, DateOnly dateOfBirth);
    void Logout();
}