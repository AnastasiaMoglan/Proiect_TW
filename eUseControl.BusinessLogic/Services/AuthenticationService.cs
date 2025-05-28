using System;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Helpers;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Interfaces;

namespace eUseControl.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private const int MaxLoginAttempts = 5;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User ValidateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            if (IsUserLocked(email))
                return null;

            var user = _userRepository.GetUserByEmail(email);

            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                UpdateLoginAttempts(email, true);
                return user;
            }

            UpdateLoginAttempts(email, false);
            return null;
        }

        public bool IsUserLocked(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null) return false;

            return user.LoginAttempts >= MaxLoginAttempts;
        }

        public void UpdateLoginAttempts(string email, bool successful)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null) return;

            if (successful)
            {
                user.LoginAttempts = 0;
                user.LastLoginDate = DateTime.UtcNow;
            }
            else
            {
                user.LoginAttempts++;
            }

            _userRepository.Update(user);
        }
    }
}