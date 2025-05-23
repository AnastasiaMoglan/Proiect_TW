using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [StringLength(50)]
        public string Gender { get; set; }
        
        [StringLength(50)]
        public string VisionType { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}