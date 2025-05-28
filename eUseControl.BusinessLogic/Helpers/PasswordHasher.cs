using System;
using System.Security.Cryptography;

namespace eUseControl.BusinessLogic.Helpers
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = new Rfc2898DeriveBytes(password, salt, Iterations).GetBytes(HashSize);
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] hash = Convert.FromBase64String(storedHash);
            byte[] testHash = new Rfc2898DeriveBytes(password, salt, Iterations).GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hash[i] != testHash[i])
                    return false;
            }

            return true;
        }
    }
}