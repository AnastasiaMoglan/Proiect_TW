using System;
using System.Text;

namespace eUseControl.Helpers
{
    public static class Security
    {
        public static string HashPassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
    }
} 