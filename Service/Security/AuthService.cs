using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;

namespace FacebookLike.Service.Security;

public class AuthService(UserRepository userRepository) : IAuthService
{
    private User? _currentUser;
    public User? CurrentUser => _currentUser;
    public event Action? OnAuthStateChanged;

    public async Task<User?> Authenticate(string email, string password)
    {
        var user = await userRepository.GetByUsernameAndPassword(email, password);
        _currentUser = user;
        OnAuthStateChanged?.Invoke();
        return user;
    }

    public void Logout()
    {
        _currentUser = null;
        OnAuthStateChanged?.Invoke();
    }

    public async Task<User?> GetUserById(string id)
    {
        var user = await userRepository.GetById(id);
        if (user == null)
            throw new Exception("User not found");
        return user;
    }

    public async Task<User> Register(string username, string password, string email, string firstName,
        string lastName, string gender, DateOnly dateOfBirth)
    {
        if (await userRepository.UsernameExists(username))
            throw new Exception("Username already exists");
        if (await userRepository.EmailExists(email))
            throw new Exception("Email already exists");
        var newUser = new User
        {
            Username = username,
            Password = password,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth
        };
        await userRepository.Create(newUser);
        _currentUser = newUser;
        OnAuthStateChanged?.Invoke();
        return newUser;
    }
}