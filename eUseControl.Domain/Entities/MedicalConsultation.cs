// eUseControl.Domain/Entities/MedicalConsultation.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("MedicalConsultation")]
    public class MedicalConsultation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public DateTime PreferredDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ConsultationType { get; set; }
        
        [StringLength(500)]
        public string AdditionalNotes { get; set; }
        
        [Required]
        public string Status { get; set; } = "Pending";
        
        public int? UserId { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}