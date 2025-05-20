using System;

namespace eUseControl.Domain.Models
{
    public class SupportTable
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public int UserId { get; set; }
        public virtual Userr User { get; set; }
    }
} 