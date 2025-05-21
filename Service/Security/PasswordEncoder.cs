using System.Security.Cryptography;
using System.Text;

namespace FacebookLike.Service
{
    public static class PasswordEncoder
    {
        public static string Encode(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
