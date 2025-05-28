using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(128)]
        public string Salt { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int LoginAttempts { get; set; }

        public DateTime? LastLogin { get; set; }
    }
} 