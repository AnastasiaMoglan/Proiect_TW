using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Entities;

namespace eUseControl.Domain.Models
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
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        
        [StringLength(50)]
        public string IPAddress { get; set; }
        
        public bool Success { get; set; }
        
        public int? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}