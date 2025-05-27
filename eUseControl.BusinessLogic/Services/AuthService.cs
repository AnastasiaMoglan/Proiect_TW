using System;
using System.Security.Cryptography;
using System.Text;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>Returnează utilizatorul după ID.</summary>
        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        /// <summary>Returnează utilizatorul după username sau email.</summary>
        public User GetUserByUsernameOrEmail(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier cannot be empty", nameof(identifier));
            
            return _userRepository.GetUserByUsernameOrEmail(identifier);
        }

        /// <summary>Validează credentialele.</summary>
        public bool ValidateCredentials(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
                return false;

            var user = _userRepository.GetUserByUsernameOrEmail(usernameOrEmail);
            if (user == null)
                return false;

            var hashedPassword = HashPassword(password, user.Salt);
            return hashedPassword == user.PasswordHash;
        }

        /// <summary>Înregistrează un utilizator nou.</summary>
        public int RegisterUser(string username, string email, string password, string firstName = null, string lastName = null)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username required", nameof(username));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required", nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required", nameof(password));

            string salt = GenerateSalt();
            string passwordHash = HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Salt = salt,
                FirstName = firstName,
                LastName = lastName,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            return _userRepository.RegisterUser(user);
        }

        /// <summary>Actualizează profilul utilizatorului.</summary>
        public void UpdateUserProfile(int userId, string firstName, string lastName, string phone, string address)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Phone = phone;
            user.Address = address;

            _userRepository.UpdateUser(user);
        }

        /// <summary>Schimbă parola utilizatorului după validarea parolei curente.</summary>
        public void ChangePassword(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("New password cannot be empty", nameof(newPassword));

            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            string currentHash = HashPassword(currentPassword, user.Salt);
            if (!string.Equals(currentHash, user.PasswordHash, StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            string newSalt = GenerateSalt();
            string newHash = HashPassword(newPassword, newSalt);

            _userRepository.UpdatePassword(userId, newHash, newSalt);
        }

        /// <summary>Înregistrează încercarea de login.</summary>
        public void RecordLoginAttempt(string email, string ip, string userAgent, bool isSuccessful)
        {
            _userRepository.RecordLoginAttempt(email, ip, userAgent, isSuccessful);
        }

        /// <summary>Verifică dacă username-ul există deja.</summary>
        public bool UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            return _userRepository.UserExists(username, null);
        }

        /// <summary>Verifică dacă email-ul există deja.</summary>
        public bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return _userRepository.UserExists(null, email);
        }

        #region Private Helpers

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            string combined = password + salt;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        #endregion
    }
}
