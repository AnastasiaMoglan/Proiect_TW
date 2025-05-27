using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public bool IsRead { get; set; } = false;
        
        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        
        public DateTime? DateRead { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "New";
        
        public bool ResponseSent { get; set; } = false;
        
        [StringLength(50)]
        public string IPAddress { get; set; }
    }
}