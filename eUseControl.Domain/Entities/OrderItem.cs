using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        
        public int ProductId { get; set; }
        
        public int Quantity { get; set; } = 1;
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [NotMapped]
        public string ProductName { get; set; }
        
        [NotMapped]
        public string ImageUrl { get; set; }
        
        [NotMapped]
        public decimal Total => (Price - Discount) * Quantity;
    }
}