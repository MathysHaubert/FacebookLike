using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FacebookLike.Service.Security;

public class AuthService : IAuthService
{
    private readonly UserRepository _userRepository;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(UserRepository userRepository, AuthenticationStateProvider authStateProvider, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _authStateProvider = authStateProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User?> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByUsernameAndPassword(email, password);
        if (user == null) return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return user;
    }

    public async void Logout()
    {
        try
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public async Task<User?> GetUserById(string id) => await _userRepository.GetById(id);

    public async Task<User?> GetCurrentUserAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return null;
        return await _userRepository.GetById(userId);
    }

    public async Task<Boolean> Register(string username, string password, string email, string firstName, string lastName, string gender, DateOnly dateOfBirth)
    {
        if (await _userRepository.UsernameExists(username))
            throw new Exception("Username already exists");
        if (await _userRepository.EmailExists(email))
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
        await _userRepository.Create(newUser);
        return true;
    }

    public User? GetCurrentUser()
    {
        return CurrentUser;
    }

    public User? CurrentUser
    {
        get
        {
            var authStateTask = _authStateProvider.GetAuthenticationStateAsync();
            if (!authStateTask.IsCompletedSuccessfully)
                return null;
            var userId = authStateTask.Result.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;
            // Attention : ici on fait un appel synchrone, ce qui peut bloquer. Préfère GetCurrentUserAsync pour la vraie logique.
            var userTask = _userRepository.GetById(userId);
            if (!userTask.IsCompletedSuccessfully)
                return null;
            return userTask.Result;
        }
    }
}