using System.Security.Cryptography;
using System.Text;

namespace TodoWeb.Application.Helpers
{
    public static class HashHelper
    {
        public static string HashBcrypt(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public static bool VerifyBcrypt(string input, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashed);
        }

        public static string GennerateRandomString(int length)
        {
            StringBuilder s = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                s.Append((char)random.Next(1, 255));
            }
            return s.ToString();
        }

        public static string Hash256(string input)
        {
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            string hash = Convert.ToHexString(hashBytes);
            return hash;
        }
    }
}
