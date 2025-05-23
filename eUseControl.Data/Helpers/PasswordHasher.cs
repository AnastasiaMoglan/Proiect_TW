using System;
using System.Security.Cryptography;
using System.Text;

namespace eUseControl.Data.Helpers
{
    public static class PasswordHasher
    {
        // Hash a password using SHA256
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
                
            using (var sha256 = SHA256.Create())
            {
                // Convert the string to a byte array first, to be processed
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                
                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                
                // Convert back to a string
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashString;
            }
        }
        
        // Verify a password against a hash
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;
                
            string hashedPassword = HashPassword(password);
            return string.Equals(hashedPassword, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}