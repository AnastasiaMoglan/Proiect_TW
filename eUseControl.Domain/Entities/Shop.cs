using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Shop")]
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [StringLength(255)]
        public string ImageUrl { get; set; }
        
        [Required]
        [StringLength(100)]
        public string OpeningHours { get; set; }
        
        [Column]
        public decimal Latitude { get; set; }
        
        [Column]
        public decimal Longitude { get; set; }
        
        public bool HasParkingAvailable { get; set; }
        
        public bool IsServiceCenter { get; set; }
        
        public bool HasOptician { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}