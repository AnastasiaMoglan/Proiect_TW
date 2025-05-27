using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Domain.Entities
{
    public class LoginRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [StringLength(50)]
        public string IP { get; set; }

        [StringLength(255)]
        public string UserAgent { get; set; }

        public bool IsSuccessful { get; set; }

        public LoginRecord()
        {
            LoginTime = DateTime.UtcNow;
        }
    }
}