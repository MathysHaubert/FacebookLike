using System;
using System.Collections.Generic;
using System.Linq;
using FacebookLike.Models;

namespace FacebookLike.Services
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        User GetUserById(int id);
        User Register(string username, string password, string email, string firstName, string lastName);
    }

    public class AuthService : IAuthService
    {
        private readonly List<User> _users;
        private int _nextId = 3; // Commence à 3 car nous avons déjà 2 utilisateurs

        public AuthService()
        {
            // Liste statique d'utilisateurs pour la démonstration
            _users = new List<User>
            {
                new User { Id = 1, Username = "admin", Password = "admin123", Email = "admin@example.com" },
                new User { Id = 2, Username = "user", Password = "user123", Email = "user@example.com" }
            };
        }

        public User Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User Register(string username, string password, string email, string firstName, string lastName)
        {
            if (_users.Any(x => x.Username == username))
            {
                throw new Exception("Ce nom d'utilisateur est déjà pris");
            }

            if (_users.Any(x => x.Email == email))
            {
                throw new Exception("Cette adresse email est déjà utilisée");
            }

            var newUser = new User
            {
                Id = _nextId++,
                Username = username,
                Password = password,
                Email = email
            };

            _users.Add(newUser);
            return newUser;
        }
    }
} 