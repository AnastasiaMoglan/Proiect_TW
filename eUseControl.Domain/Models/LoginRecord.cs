using System;

namespace eUseControl.Domain.Models
{
    public class LoginRecord
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; }
        public bool Success { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
} 